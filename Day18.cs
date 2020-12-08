using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication5
{
    class Day18
    {
        public void Calculate()
        {
            bool[,] lights = new bool[100,100];

            var input = File.ReadAllLines("day18.txt");
            for(int y= 0; y < 100; y++)
            for (int x = 0; x < 100; x++)
                lights[x, y] = (input[y][x] == '#');

            for (int i = 0; i < 100; i++)
            {
                //Print(lights);

                lights[0, 0] = true;
                lights[0, 99] = true;
                lights[99, 0] = true;
                lights[99, 99] = true;

                var lights2 = new bool[100,100];
                GetNextState(lights, lights2);
                lights = lights2;
            }


            lights[0, 0] = true;
            lights[0, 99] = true;
            lights[99, 0] = true;
            lights[99, 99] = true;

            int numOn = 0;
            for (int y = 0; y < 100; y++)
            for (int x = 0; x < 100; x++)
            {
                if (lights[x, y])
                    numOn++;
            }


            Console.WriteLine(numOn);
        }

        private void Print(bool[,] lights)
        {
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                    Console.Write(lights[x,y]? '#' : '.');
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        private void GetNextState(bool[,] lights, bool[,] lights2)
        {
            for (int y = 0; y < 100; y++)
            for (int x = 0; x < 100; x++)
            {
                int neighbors = NumNeighbors(lights, x, y);
                if (lights[x, y])
                {
                    lights2[x, y] = (neighbors == 2 || neighbors == 3);
                }
                else
                {
                    lights2[x, y] = (neighbors == 3);
                }
            }
        }

        private int NumNeighbors(bool[,] lights, int x, int y)
        {
            Func<int, bool> validX = xx => (xx >= 0 && xx < 100);
            Func<int, bool> validY = yy => (yy >= 0 && yy < 100);

            var xs = new int[] {x - 1, x, x + 1};
            var ys = new int[] {y - 1, y, y + 1};
            var allCombos = from x1 in xs
                from y1 in ys
                where (x1 != x || y1 != y)
                where validX(x1) && validY(y1)
                select lights[x1, y1];

            return allCombos.Sum(v => v ? 1 : 0);
        }
    }
}