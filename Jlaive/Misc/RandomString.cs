using System;
using System.Collections.Generic;
using System.Linq;

namespace Jlaive
{
    internal class RandomString
    {
        private readonly Random rng;
        private readonly List<string> usedstrings;
        private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public RandomString(Random rng)
        {
            this.rng = rng;
            usedstrings = new List<string>();
        }

        public string Get(int length)
        {
            string ret = string.Empty;
            while (ret == string.Empty)
            {
                ret = new string(Enumerable.Repeat(chars, length).Select(s => s[rng.Next(s.Length)]).ToArray());
                ret = usedstrings.Contains(ret) ? string.Empty : ret;
            }
            usedstrings.Add(ret);
            return ret;
        }
    }
}
