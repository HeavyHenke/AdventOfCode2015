using System;

namespace ConsoleApplication5
{
    internal class Day25
    {
        public void Calculate()
        {
            long num = 20151125;

            int row = 1;
            int col = 1;
            int maxRow = 1;
            while (true)
            {
                if (row == 1)
                {
                    maxRow = row = maxRow + 1;
                    col = 1;
                }
                else
                {
                    row--;
                    col++;
                }

                num = NextNum(num);


                if (row == 2978 && col == 3083)
                {
                    Console.WriteLine($"Found it: {num}");
                    return;
                }
            }

        }

        private long NextNum(long num)
        {
            return (num*252533)%33554393;
        }
    }
}