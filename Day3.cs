using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication5
{
    public class Day3
    {
        private Dictionary<Coordinate, int> _presentsPerHouse = new Dictionary<Coordinate, int>();

        struct Coordinate
        {
            private readonly int X;
            private readonly int Y;

            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Coordinate GoInDirection(char dir)
            {
                switch (dir)
                {
                    case '^': return new Coordinate(X, Y-1);
                    case 'v': return new Coordinate(X, Y + 1);
                    case '<': return new Coordinate(X-1, Y);
                    case '>': return new Coordinate(X+1, Y);
                    default: throw new ArgumentException("Invalid direction: " + dir);
                }
            }

            public override int GetHashCode()
            {
                return Y << 16 ^ X;
            }
        }

        public void Calculate()
        {
            string input =  File.ReadAllText("day3.txt");

            var coord = new Coordinate(0, 0);
            var coord2 = new Coordinate(0, 0);
            AddPresentToHouse(coord);

            bool nextIsCoord = true;

            foreach (var dir in input)
            {
                if (nextIsCoord)
                {
                    coord = coord.GoInDirection(dir);
                    AddPresentToHouse(coord);
                }
                else
                {
                    coord2 = coord2.GoInDirection(dir);
                    AddPresentToHouse(coord2);
                }

                nextIsCoord = !nextIsCoord;
            }
        }

        private void AddPresentToHouse(Coordinate coord)
        {
            if (_presentsPerHouse.ContainsKey(coord))
                _presentsPerHouse[coord]++;
            else
                _presentsPerHouse[coord] = 1;
        }
    }
}