// Modified https://github.com/qleex/BatCloak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatCloak
{
    internal struct LineObfResult
    {
        public string[] Sets;
        public string Result;
    }

    internal class LineObfuscation
    {
        public List<string> Variables { get; set; }
        public int Level { get; set; }
        public string Boilerplate { get; }

        private string setvar { get; }
        private string equalsvar { get; }
        private List<string> usedstrings { get; }
        private Random rng { get; }
        private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public LineObfuscation(int level)
        {
            Variables = new List<string>();
            Level = level;
            rng = new Random();
            usedstrings = new List<string>();
            setvar = RandomString(4);
            equalsvar = RandomString(4);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"set \"{setvar}=set \"");
            builder.AppendLine($"%{setvar}%\"{equalsvar}==\"");
            Boilerplate = builder.ToString();
        }

        public LineObfResult Process(string code)
        {
            int amount = 5;
            if (Level > 1) amount -= Level;
            amount *= 2;

            List<string> setlines = new List<string>();
            List<string> splitted = new List<string>();
            string sc = string.Empty;
            bool invar = false;
            foreach (char c in code)
            {
                if (c == '%')
                {
                    invar = !invar;
                    sc += c;
                    continue;
                }
                if ((c == ' ' || c == '\'' || c == '.') && invar)
                {
                    invar = false;
                    sc += c;
                    continue;
                }
                if (!invar && sc.Length >= amount)
                {
                    splitted.Add(sc);
                    invar = false;
                    sc = string.Empty;
                }
                sc += c;
            }
            splitted.Add(sc);

            LineObfResult result = new LineObfResult() { Result = string.Empty };
            List<string> newvars = new List<string>();
            for (int i = 0; i < splitted.Count; i++)
            {
                string name;
                if (i < Variables.Count) name = Variables[i];
                else
                {
                    name = RandomString(10);
                    newvars.Add(name);
                }
                setlines.Add($"%{setvar}%\"{name}%{equalsvar}%{splitted[i]}\"");
                result.Result += $"%{name}%";
            }
            Variables.AddRange(newvars);
            result.Sets = setlines.OrderBy(x => rng.Next()).ToArray();
            return result;
        }

        private string RandomString(int length)
        {
            string ret = string.Empty;
            while (ret == string.Empty)
            {
                ret = new string(Enumerable.Repeat(chars, length).Select(s => s[rng.Next(s.Length)]).ToArray());
                if (usedstrings.Contains(ret)) ret = string.Empty;
            }
            usedstrings.Add(ret);
            return ret;
        }
    }
}
