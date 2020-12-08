using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day12
    {
        public void Calculate()
        {
            var input = File.ReadAllText("day12.txt");
            var o = new Obj(input);
            int c = o.Calc();

            // To low  2664
            // To low  46454
            // To high 89491
        }


        class Obj
        {
            private readonly string _inner;
            private readonly IList<Obj> _children = new List<Obj>();

            private int Length => _inner.Length + _children.Sum(c => c.Length);

            private readonly bool _isObj;

            public Obj(string input)
            {
                var sb = new StringBuilder();
                _isObj = input[0] == '{';

                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '}' && _isObj)
                    {
                        sb.Append(input[i]);
                        _inner = sb.ToString();
                        return;
                    }
                    if (input[i] == ']' && _isObj == false)
                    {
                        sb.Append(input[i]);
                        _inner = sb.ToString();
                        return;
                    }

                    if (i > 0 && (input[i] == '{' || input[i] == '['))
                    {
                        var child = new Obj(input.Substring(i));
                        _children.Add(child);
                        i += child.Length - 1;
                    }
                    else
                    {
                        sb.Append(input[i]);
                    }
                }
            }

            private static readonly Regex FindNumber = new Regex(@"(-?\d+)", RegexOptions.Compiled);

            public int Calc()
            {
                var match = FindNumber.Match(_inner);
                int sum = 0;

                if (_isObj && _inner.Contains("\"red\""))
                    return 0;

                while (match.Success)
                {
                    sum += int.Parse(match.Groups[0].Value);
                    match = FindNumber.Match(_inner, match.Groups[0].Index + match.Groups[0].Length);
                }

                var childrenSum = _children.Sum(c => c.Calc());

                return sum + childrenSum;
            }
        }


    }
}