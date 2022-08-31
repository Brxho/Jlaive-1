// Modified https://github.com/ch2sh/BatCloak

using System;
using System.Collections.Generic;
using System.Text;

namespace BatCloak
{
    internal class FileObfuscation
    {
        public string Process(string contents, int level)
        {
            List<string> lines = new List<string>(contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            lines.Insert(0, "rem https://github.com/ch2sh/Jlaive");
            StringBuilder builder = new StringBuilder();
            LineObfuscation obfuscator = new LineObfuscation(level);
            builder.AppendLine("@echo off");
            builder.Append(obfuscator.Boilerplate);
            for (int i = 0; i < lines.Count; i++)
            {
                if ((lines[i].StartsWith("rem") || lines[i].StartsWith("::")) && !lines[i].Contains("BatCloak")) continue;
                LineObfResult result = obfuscator.Process(lines[i]);
                builder.AppendLine(string.Join(Environment.NewLine, result.Sets));
                builder.AppendLine(result.Result);
            }
            return builder.ToString().TrimEnd('\r', '\n');
        }
    }
}
