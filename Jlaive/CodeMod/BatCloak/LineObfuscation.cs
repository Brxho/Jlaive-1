// Modified https://gitlab.com/ch2sh/BatCloak

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlaive;

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

        private string _setvar { get; }
        private string _equalsvar { get; }
        private Random _rng { get; }
        private RandomString _rngStr { get; }

        public LineObfuscation(int level)
        {
            Variables = new List<string>();
            Level = level;
            _rng = new Random();
            _rngStr = new RandomString(_rng);
            _setvar = _rngStr.Get(4);
            _equalsvar = _rngStr.Get(4);
            var builder = new StringBuilder();
            builder.AppendLine($"set \"{_setvar}=set \"");
            builder.AppendLine($"%{_setvar}%\"{_equalsvar}==\"");
            Boilerplate = builder.ToString();
        }

        public LineObfResult Process(string code)
        {
            var amount = 5;
            if (Level > 1) amount -= Level;
            amount *= 2;

            var setlines = new List<string>();
            var splitted = new List<string>();
            var sc = string.Empty;
            var invar = false;
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

            var result = new LineObfResult() { Result = string.Empty };
            var newvars = new List<string>();
            for (var i = 0; i < splitted.Count; i++)
            {
                string name;
                if (i < Variables.Count) name = Variables[i];
                else
                {
                    name = _rngStr.Get(10);
                    newvars.Add(name);
                }
                setlines.Add($"%{_setvar}%\"{name}%{_equalsvar}%{splitted[i]}\"");
                result.Result += $"%{name}%";
            }
            Variables.AddRange(newvars);
            result.Sets = setlines.OrderBy(x => _rng.Next()).ToArray();
            return result;
        }
    }
}
