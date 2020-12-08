using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication5
{
    public class Day2
    {
        public void Calculate()
        {
            string[] input = File.ReadAllLines("day2.txt");
            long totalPaperNeed = 0;
            long totalRibbonNeed = 0;

            foreach (var s in input)
            {
                var parts = s.Split('x').Select(int.Parse).ToArray();
                int[] sides = {parts[0]*parts[1], parts[0]*parts[2], parts[1]*parts[2]};

                int paperNeed = 2* sides[0] + 2* sides[1] + 2* sides[2];
                int extra = Math.Min(Math.Min(sides[0], sides[1]), sides[2]);
                totalPaperNeed += paperNeed + extra;

                totalRibbonNeed += CalcRibbonNeed(parts[0], parts[1], parts[2]);
            }

        }

        private int CalcRibbonNeed(int a, int b, int c)
        {
            int[] ribbonSides = {2*a + 2*b, 2*a + 2*c, 2*c + 2*b};
            int smallestSide = Math.Min(Math.Min(ribbonSides[0], ribbonSides[1]), ribbonSides[2]);
            int need = smallestSide + (a * b * c);
            return need;
        }
    }
}