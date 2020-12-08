using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day11
    {
        public void Calculate()
        {
            string input = "vzbxkghb";

            do
            {
                input = Inc(input);

            } while (!Rule1(input) || !Rule2(input) || !Rule3(input));

            Console.WriteLine(input);

        }

        private bool Rule1(string s)
        {
            for (int i = 0; i < s.Length - 3; i++)
            {
                if (s[i] == s[i + 1] - 1 && s[i + 1] == s[i + 2] - 1)
                    return true;
            }

            return false;
        }

        private bool Rule2(string s)
        {
            return !s.Contains('i') && !s.Contains('o') && !s.Contains('l');
        }

        private bool Rule3(string s)
        {
            var r = new Regex(@".*(.)\1.*(.)\2.*");
            return r.IsMatch(s);
        }

        private string Inc(string inp)
        {
            return Inc(inp, inp.Length - 1);
        }

        private string Inc(string inp, int idx)
        {
            var c = inp[idx];
            if (c == 'z')
            {
                inp = inp.Remove(idx, 1).Insert(idx, "a");
                return Inc(inp, idx - 1);
            }

            var q = (int)c;
            var z = (char)(q + 1);
            var y = z.ToString();
            return inp.Remove(idx, 1).Insert(idx, y);
        }
    }
}