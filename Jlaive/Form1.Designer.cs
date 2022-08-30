namespace Jlaive
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openButton = new System.Windows.Forms.Button();
            this.antiDebug = new System.Windows.Forms.CheckBox();
            this.buildButton = new System.Windows.Forms.Button();
            this.meltFile = new System.Windows.Forms.CheckBox();
            this.hidden = new System.Windows.Forms.CheckBox();
            this.antiVM = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.optionsPage = new System.Windows.Forms.TabPage();
            this.runas = new System.Windows.Forms.CheckBox();
            this.encryptionPage = new System.Windows.Forms.TabPage();
            this.refreshKeys = new System.Windows.Forms.Button();
            this.iv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.key = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.binderPage = new System.Windows.Forms.TabPage();
            this.removeFile = new System.Windows.Forms.Button();
            this.addFile = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.outputPage = new System.Windows.Forms.TabPage();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.optionsPage.SuspendLayout();
            this.encryptionPage.SuspendLayout();
            this.binderPage.SuspendLayout();
            this.outputPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "File path:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 35);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(469, 22);
            this.textBox1.TabIndex = 1;
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openButton.Location = new System.Drawing.Point(485, 34);
            this.openButton.Margin = new System.Windows.Forms.Padding(2);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 25);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // antiDebug
            // 
            this.antiDebug.AutoSize = true;
            this.antiDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antiDebug.Location = new System.Drawing.Point(4, 4);
            this.antiDebug.Margin = new System.Windows.Forms.Padding(2);
            this.antiDebug.Name = "antiDebug";
            this.antiDebug.Size = new System.Drawing.Size(101, 22);
            this.antiDebug.TabIndex = 6;
            this.antiDebug.Text = "Anti Debug";
            this.antiDebug.UseVisualStyleBackColor = true;
            // 
            // buildButton
            // 
            this.buildButton.Location = new System.Drawing.Point(11, 359);
            this.buildButton.Margin = new System.Windows.Forms.Padding(2);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(550, 48);
            this.buildButton.TabIndex = 7;
            this.buildButton.Text = "Build";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // meltFile
            // 
            this.meltFile.AutoSize = true;
            this.meltFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meltFile.Location = new System.Drawing.Point(4, 56);
            this.meltFile.Margin = new System.Windows.Forms.Padding(2);
            this.meltFile.Name = "meltFile";
            this.meltFile.Size = new System.Drawing.Size(80, 22);
            this.meltFile.TabIndex = 8;
            this.meltFile.Text = "Melt file";
            this.meltFile.UseVisualStyleBackColor = true;
            // 
            // hidden
            // 
            this.hidden.AutoSize = true;
            this.hidden.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hidden.Location = new System.Drawing.Point(4, 82);
            this.hidden.Margin = new System.Windows.Forms.Padding(2);
            this.hidden.Name = "hidden";
            this.hidden.Size = new System.Drawing.Size(117, 22);
            this.hidden.TabIndex = 9;
            this.hidden.Text = "Hide console";
            this.hidden.UseVisualStyleBackColor = true;
            // 
            // antiVM
            // 
            this.antiVM.AutoSize = true;
            this.antiVM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antiVM.Location = new System.Drawing.Point(4, 30);
            this.antiVM.Margin = new System.Windows.Forms.Padding(2);
            this.antiVM.Name = "antiVM";
            this.antiVM.Size = new System.Drawing.Size(80, 22);
            this.antiVM.TabIndex = 10;
            this.antiVM.Text = "Anti VM";
            this.antiVM.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.optionsPage);
            this.tabControl1.Controls.Add(this.encryptionPage);
            this.tabControl1.Controls.Add(this.binderPage);
            this.tabControl1.Controls.Add(this.outputPage);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 65);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(548, 290);
            this.tabControl1.TabIndex = 11;
            // 
            // optionsPage
            // 
            this.optionsPage.Controls.Add(this.runas);
            this.optionsPage.Controls.Add(this.hidden);
            this.optionsPage.Controls.Add(this.meltFile);
            this.optionsPage.Controls.Add(this.antiDebug);
            this.optionsPage.Controls.Add(this.antiVM);
            this.optionsPage.Location = new System.Drawing.Point(4, 26);
            this.optionsPage.Margin = new System.Windows.Forms.Padding(2);
            this.optionsPage.Name = "optionsPage";
            this.optionsPage.Padding = new System.Windows.Forms.Padding(2);
            this.optionsPage.Size = new System.Drawing.Size(540, 260);
            this.optionsPage.TabIndex = 0;
            this.optionsPage.Text = "Options";
            this.optionsPage.UseVisualStyleBackColor = true;
            // 
            // runas
            // 
            this.runas.AutoSize = true;
            this.runas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runas.Location = new System.Drawing.Point(4, 108);
            this.runas.Margin = new System.Windows.Forms.Padding(2);
            this.runas.Name = "runas";
            this.runas.Size = new System.Drawing.Size(121, 22);
            this.runas.TabIndex = 11;
            this.runas.Text = "Run as admin";
            this.runas.UseVisualStyleBackColor = true;
            // 
            // encryptionPage
            // 
            this.encryptionPage.Controls.Add(this.refreshKeys);
            this.encryptionPage.Controls.Add(this.iv);
            this.encryptionPage.Controls.Add(this.label3);
            this.encryptionPage.Controls.Add(this.key);
            this.encryptionPage.Controls.Add(this.label2);
            this.encryptionPage.Location = new System.Drawing.Point(4, 26);
            this.encryptionPage.Margin = new System.Windows.Forms.Padding(2);
            this.encryptionPage.Name = "encryptionPage";
            this.encryptionPage.Padding = new System.Windows.Forms.Padding(2);
            this.encryptionPage.Size = new System.Drawing.Size(540, 260);
            this.encryptionPage.TabIndex = 1;
            this.encryptionPage.Text = "Encryption";
            this.encryptionPage.UseVisualStyleBackColor = true;
            // 
            // refreshKeys
            // 
            this.refreshKeys.Location = new System.Drawing.Point(158, 58);
            this.refreshKeys.Margin = new System.Windows.Forms.Padding(2);
            this.refreshKeys.Name = "refreshKeys";
            this.refreshKeys.Size = new System.Drawing.Size(114, 25);
            this.refreshKeys.TabIndex = 9;
            this.refreshKeys.Text = "Refresh keys";
            this.refreshKeys.UseVisualStyleBackColor = true;
            this.refreshKeys.Click += new System.EventHandler(this.refreshKeys_Click);
            // 
            // iv
            // 
            this.iv.Location = new System.Drawing.Point(46, 31);
            this.iv.Margin = new System.Windows.Forms.Padding(2);
            this.iv.Name = "iv";
            this.iv.ReadOnly = true;
            this.iv.Size = new System.Drawing.Size(226, 23);
            this.iv.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "IV:";
            // 
            // key
            // 
            this.key.Location = new System.Drawing.Point(46, 4);
            this.key.Margin = new System.Windows.Forms.Padding(2);
            this.key.Name = "key";
            this.key.ReadOnly = true;
            this.key.Size = new System.Drawing.Size(226, 23);
            this.key.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Key:";
            // 
            // binderPage
            // 
            this.binderPage.Controls.Add(this.removeFile);
            this.binderPage.Controls.Add(this.addFile);
            this.binderPage.Controls.Add(this.listBox1);
            this.binderPage.Location = new System.Drawing.Point(4, 26);
            this.binderPage.Margin = new System.Windows.Forms.Padding(2);
            this.binderPage.Name = "binderPage";
            this.binderPage.Padding = new System.Windows.Forms.Padding(2);
            this.binderPage.Size = new System.Drawing.Size(540, 260);
            this.binderPage.TabIndex = 2;
            this.binderPage.Text = "Binder";
            this.binderPage.UseVisualStyleBackColor = true;
            // 
            // removeFile
            // 
            this.removeFile.Location = new System.Drawing.Point(122, 221);
            this.removeFile.Margin = new System.Windows.Forms.Padding(2);
            this.removeFile.Name = "removeFile";
            this.removeFile.Size = new System.Drawing.Size(116, 34);
            this.removeFile.TabIndex = 2;
            this.removeFile.Text = "Remove file";
            this.removeFile.UseVisualStyleBackColor = true;
            this.removeFile.Click += new System.EventHandler(this.removeFile_Click);
            // 
            // addFile
            // 
            this.addFile.Location = new System.Drawing.Point(2, 221);
            this.addFile.Margin = new System.Windows.Forms.Padding(2);
            this.addFile.Name = "addFile";
            this.addFile.Size = new System.Drawing.Size(116, 34);
            this.addFile.TabIndex = 1;
            this.addFile.Text = "Add file";
            this.addFile.UseVisualStyleBackColor = true;
            this.addFile.Click += new System.EventHandler(this.addFile_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(2, 5);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(532, 208);
            this.listBox1.TabIndex = 0;
            // 
            // outputPage
            // 
            this.outputPage.Controls.Add(this.listBox2);
            this.outputPage.Location = new System.Drawing.Point(4, 26);
            this.outputPage.Margin = new System.Windows.Forms.Padding(2);
            this.outputPage.Name = "outputPage";
            this.outputPage.Padding = new System.Windows.Forms.Padding(2);
            this.outputPage.Size = new System.Drawing.Size(540, 260);
            this.outputPage.TabIndex = 3;
            this.outputPage.Text = "Output";
            this.outputPage.UseVisualStyleBackColor = true;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 17;
            this.listBox2.Location = new System.Drawing.Point(4, 4);
            this.listBox2.Margin = new System.Windows.Forms.Padding(2);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(532, 242);
            this.listBox2.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(571, 418);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buildButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jlaive";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.optionsPage.ResumeLayout(false);
            this.optionsPage.PerformLayout();
            this.encryptionPage.ResumeLayout(false);
            this.encryptionPage.PerformLayout();
            this.binderPage.ResumeLayout(false);
            this.outputPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.CheckBox antiDebug;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.CheckBox meltFile;
        private System.Windows.Forms.CheckBox hidden;
        private System.Windows.Forms.CheckBox antiVM;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage optionsPage;
        private System.Windows.Forms.TabPage encryptionPage;
        private System.Windows.Forms.TabPage binderPage;
        private System.Windows.Forms.TabPage outputPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox key;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button addFile;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox iv;
        private System.Windows.Forms.Button refreshKeys;
        private System.Windows.Forms.Button removeFile;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.CheckBox runas;
    }
}
