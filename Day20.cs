using System;
using System.Collections.Generic;

namespace ConsoleApplication5
{
    internal class Day20
    {
        public void Calculate()
        {
            int house = 1;
            while (true)
            {
                var factors = Factor(house);
                int numPresents = 0;
                foreach (var factor in factors)
                {
                    if(house / factor <= 50)
                        numPresents += factor*11;
                }

                if(house% 10000 == 0)
                    Console.WriteLine($"calculating on house {house}");

                if (numPresents >= 29000000)
                {
                    Console.WriteLine($"House {house} gets {numPresents} presents");
                    return;
                }

                house++;
            }
        }


        public List<int> Factor(int number)
        {
            List<int> factors = new List<int>();
            int max = (int)Math.Sqrt(number);  //round down
            for (int factor = 1; factor <= max; ++factor)
            { //test from 1 to the square root, or the int below it, inclusive.
                if (number % factor == 0)
                {
                    factors.Add(factor);
                    if (factor != number / factor)
                    { // Don't add the square root twice!  Thanks Jon
                        factors.Add(number / factor);
                    }
                }
            }
            return factors;
        }
    }
}