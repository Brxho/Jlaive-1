using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jlaive
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            if (path.IndexOf(Path.GetTempPath(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                MessageBox.Show("Jlaive cannot be run from a ZIP file! Please extract before running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\bin")) Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\bin");
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
