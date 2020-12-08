using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day14
    {
        public void Calculate()
        {
            var inputs = File.ReadAllLines("day14.txt");
            var regex = new Regex(@"(?<name>\w+) can fly (?<speed>\d+) km/s for (?<time>\d+) seconds, but then must rest for (?<rest>\d+) seconds\.", RegexOptions.Compiled);

            var list = new List<Rudolf>();
              
            foreach (var line in inputs)
            {
                var match = regex.Match(line);
                var r = new Rudolf
                {
                    Name = match.Groups["name"].Value,
                    Speed = int.Parse(match.Groups["speed"].Value),
                    SpeedTime = int.Parse(match.Groups["time"].Value),
                    SleepTime = int.Parse(match.Groups["rest"].Value)
                };

                //if(r.Name == "Blitzen")
                list.Add(r);
            }

            for (int sec = 0; sec < 2503; sec++)
            {
                foreach (var rudolf in list)
                {
                    rudolf.Step();
                }

                int maxPoint = list.Max(r => r.Distance);
                foreach (var rudolf in list.Where(r => r.Distance == maxPoint))
                {
                    rudolf.AddPoint();
                }
            }

            var max = list.OrderByDescending(r => r.Distance).First();
            var max2 = list.OrderByDescending(r => r.Points).First();
        }

        class Rudolf
        {
            public string Name;
            public int Speed;
            public int SpeedTime;
            public int SleepTime;

            public bool IsRunning = true;
            public int Distance;
            public int Count;

            public void Step()
            {
                if (IsRunning)
                {
                    Distance += Speed;
                    Count++;
                    if (Count >= SpeedTime)
                    {
                        Count = 0;
                        IsRunning = false;
                        return;
                    }
                    return;
                }

                Count++;
                if (Count >= SleepTime)
                {
                    Count++;
                    if (Count > SleepTime)
                    {
                        Count = 0;
                        IsRunning = true;
                    }
                }
            }

            public int Points = 0;

            public void AddPoint()
            {
                Points++;
            }
        }
    }
}