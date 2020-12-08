using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    internal class Day23
    {
        public void Calculate()
        {
            var parser = new Regex(@"(hlf|tpl|inc|jmp|jie|jio) (\w|[\+-]\d+) ?,? ?(.*)");

            var input = File.ReadAllLines("Day23.txt");

            var program = new List<Action>();

            foreach (var instr in input)
            {
                var m  = parser.Match(instr);

                Action op;
                int offset;
                switch (m.Groups[1].Value)
                {
                    case "hlf":
                        op = () => { _a = _a/2; _ip++; };
                        break;
                    case "tpl":
                        op = () => { _a = _a*3; _ip++; };
                        break;
                    case "inc":
                        if (m.Groups[2].Value == "a")
                            op = () => { _a = _a + 1; _ip++; };
                        else
                            op = () => { _b = _b + 1; _ip++; };
                        break;
                    case "jmp":
                        offset = int.Parse(m.Groups[2].Value);
                        op = () => _ip += offset;
                        break;
                    case "jie":
                        offset = int.Parse(m.Groups[3].Value);
                        op = () => _ip = ((_a & 1) == 0) ? _ip + offset : _ip+1;
                        break;
                    case "jio":
                        offset = int.Parse(m.Groups[3].Value);
                        op = () => _ip = (_a == 1) ? _ip + offset : _ip+1;
                        break;
                    default:
                        throw new Exception("Unknown instruction");
                }
                program.Add(op);
            }

            _a = 1;
            while (_ip < program.Count)
            {
                program[_ip]();
            }

            Console.WriteLine($"B is {_b}");
        }

        private int _a;
        private int _b;
        private int _ip;
    }
}