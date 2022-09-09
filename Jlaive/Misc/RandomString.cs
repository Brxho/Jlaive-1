using System;
using System.Collections.Generic;
using System.Linq;

namespace Jlaive
{
    internal class RandomString
    {
        private readonly Random _rng;
        private readonly List<string> _usedStrings;
        private const string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public RandomString(Random rng)
        {
            _rng = rng;
            _usedStrings = new List<string>();
        }

        public string Get(int length)
        {
            string ret = string.Empty;
            while (ret == string.Empty)
            {
                ret = new string(Enumerable.Repeat(_chars, length).Select(s => s[_rng.Next(s.Length)]).ToArray());
                ret = _usedStrings.Contains(ret) ? string.Empty : ret;
            }
            _usedStrings.Add(ret);
            return ret;
        }
    }
}
