using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day16
    {
        public void Calculate()
        {
            var inputs = File.ReadAllLines("day16.txt");
            var suenr = new Regex(@"Sue (?<suenr>\w+)");
            var children = new Regex(@"(?<type>children): (?<val>\w+)");
            var cats = new Regex(@"(?<type>cats): (?<val>\w+)");
            var samoyeds = new Regex(@"(?<type>samoyeds): (?<val>\w+)");
            var pomeranians = new Regex(@"(?<type>pomeranians): (?<val>\w+)");
            var akitas = new Regex(@"(?<type>akitas): (?<val>\w+)");
            var vizslas = new Regex(@"(?<type>vizslas): (?<val>\w+)");
            var goldfish = new Regex(@"(?<type>goldfish): (?<val>\w+)");
            var trees = new Regex(@"(?<type>trees): (?<val>\w+)");
            var cars = new Regex(@"(?<type>cars): (?<val>\w+)");
            var perfumes = new Regex(@"(?<type>perfumes): (?<val>\w+)");

            var regexes = new[] {children, cats, samoyeds, pomeranians, akitas, vizslas, goldfish, trees, cars, perfumes};
            var correct = new Dictionary<string, Func<int, bool>>
            {
                {"children", f =>  f == 3},
                {"cats", f => f > 7},
                {"samoyeds", f => f == 2},
                {"pomeranians", f => f < 3},
                {"akitas", f => f == 0},
                {"vizslas", f => f == 0},
                {"goldfish", f => f < 5},
                {"trees", f => f > 3},
                {"cars", f => f == 2},
                {"perfumes", f => f == 1}
            };

            foreach (var input in inputs)
            {
                int sueNr = int.Parse(suenr.Match(input).Groups["suenr"].Value);
                bool success = true;
                foreach (var regex in regexes)
                {
                    var m = regex.Match(input);
                    if(!m.Success) continue;

                    if(!correct[m.Groups["type"].Value](int.Parse(m.Groups["val"].Value)))
                    {
                        success = false;
                        break;
                    }
                }

                if(success)
                    Console.WriteLine(input);
            }

        }
    }
}