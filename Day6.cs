using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    public class Day6
    {

        public void CalculateB()
        {
            int[,] lights = new int[1000, 1000];

            var regParse = new Regex(
                "^(?<cmd>turn on|turn off|toggle) (?<cord1>\\d*,\\d*) through (?<cord2>\\d*,\\d*)",
                RegexOptions.Compiled);

            var input = File.ReadAllLines("day6.txt");
            foreach (var line in input)
            {
                var match = regParse.Match(line);

                Func<int, int> toggleFunc;
                if (match.Groups["cmd"].Value == "turn on")
                    toggleFunc = b => b + 1;
                else if (match.Groups["cmd"].Value == "turn off")
                    toggleFunc = b => Math.Max(0, b - 1);
                else if (match.Groups["cmd"].Value == "toggle")
                    toggleFunc = b => b + 2;
                else
                    throw new Exception("Invalid command " + match.Groups["cmd"].Value);

                var start = match.Groups["cord1"].Value.Split(',').Select(int.Parse).ToArray();
                var stop = match.Groups["cord2"].Value.Split(',').Select(int.Parse).ToArray();

                int startX = start[0];
                int stopX = stop[0];
                int startY = start[1];
                int stopY = stop[1];

                for (int x = startX; x <= stopX; x++)
                {
                    for (int y = startY; y <= stopY; y++)
                    {
                        lights[x, y] = toggleFunc(lights[x, y]);
                    }
                }
            }

            int numOn = 0;
            for (int x = 0; x < 1000; x++)
            for (int y = 0; y < 1000; y++)
                numOn += lights[x, y];

            Console.WriteLine("Number of lights " + numOn);
        }

        public void Calculate()
        {
            bool[,] lights = new bool[1000, 1000];

            var regParse = new Regex(
                "^(?<cmd>turn on|turn off|toggle) (?<cord1>\\d*,\\d*) through (?<cord2>\\d*,\\d*)",
                RegexOptions.Compiled);

            var input = File.ReadAllLines("day6.txt");
            foreach (var line in input)
            {
                var match = regParse.Match(line);

                Func<bool, bool> toggleFunc;
                if (match.Groups["cmd"].Value == "turn on")
                    toggleFunc = b => true;
                else if (match.Groups["cmd"].Value == "turn off")
                    toggleFunc = b => false;
                else if (match.Groups["cmd"].Value == "toggle")
                    toggleFunc = b => !b;
                else
                    throw new Exception("Invalid command " + match.Groups["cmd"].Value);

                var start = match.Groups["cord1"].Value.Split(',').Select(int.Parse).ToArray();
                var stop = match.Groups["cord2"].Value.Split(',').Select(int.Parse).ToArray();

                int startX = start[0];
                int stopX = stop[0];
                int startY = start[1];
                int stopY = stop[1];

                int numVisited = 0;
                for(int x = startX; x <= stopX; x++)
                {
                    for(int y = startY; y <= stopY; y++)
                    {
                        lights[x, y] = toggleFunc(lights[x, y]);
                        numVisited++;
                    }
                }
            }

            int numOn = 0;
            for (int x = 0; x < 1000; x++)
            for (int y = 0; y < 1000; y++)
                numOn += (lights[x, y]) ? 1 : 0;

            Console.WriteLine("Number of lights " + numOn);
        }
    }
}