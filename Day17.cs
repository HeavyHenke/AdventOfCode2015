using System;
using System.Collections.Generic;

namespace ConsoleApplication5
{
    class Day17
    {
        private static readonly List<int> Sizes = new List<int> { 50,44,11,49,42,46,18,32,26,40,21,7,18,43,10,47,36,24,22,40 };
        //private static readonly List<int> Sizes = new List<int> { 20, 15, 10, 5, 5 };
        private const int Total = 150;

        public void CalculateA()
        {
            var search = new Queue<Node>();

            search.Enqueue(new Node(0, 0));
            search.Enqueue(new Node(0, 1));


            var valids = new List<Node>();
            int minimumNum = int.MaxValue;

            while (search.Count > 0)
            {
                var n = search.Dequeue();
                if (n.SizeIx == Sizes.Count - 2)
                {
                    var valid = new Node(n, 0);
                    if (valid.TotalSoFar == Total)
                    {
                        valids.Add(valid);
                    }

                    valid = new Node(n, 1);
                    if (valid.TotalSoFar == Total)
                    {
                        valids.Add(valid);
                    }

                    continue;
                }

                var next0 = new Node(n, 0);
                search.Enqueue(next0);

                var next1 = new Node(n, 1);
                if (next1.TotalSoFar <= Total)
                    search.Enqueue(next1);

            }

            Console.WriteLine($"Valids {valids.Count}");
        }

        public void CalculateB()
        {
            var search = new Queue<Node>();

            search.Enqueue(new Node(0, 0));
            search.Enqueue(new Node(0, 1));


            var valids = new List<Node>();
            int minimumNum = int.MaxValue;

            while (search.Count > 0)
            {
                var n = search.Dequeue();
                if (n.SizeIx == Sizes.Count-2)
                {
                    if ((150 - n.TotalSoFar)% Sizes[Sizes.Count-1] == 0)
                    {
                        var valid = new Node(n, 0);
                        if (valid.TotalSoFar == 150)
                        {
                            if (valid.NumContainers() < minimumNum)
                            {
                                valids.Clear();
                                minimumNum = valid.NumContainers();
                                valids.Add(valid);
                            }
                            else if(valid.NumContainers() == minimumNum)
                                valids.Add(valid);
                        }

                        valid = new Node(n, 1);
                        if (valid.TotalSoFar == 150)
                        {
                            if (valid.NumContainers() < minimumNum)
                            {
                                valids.Clear();
                                minimumNum = valid.NumContainers();
                                valids.Add(valid);
                            }
                            else if (valid.NumContainers() == minimumNum)
                                valids.Add(valid);
                        }
                    }
                    continue;
                }

                var next0 = new Node(n, 0);
                search.Enqueue(next0);

                var next1 = new Node(n, 1);
                if (next1.TotalSoFar <= 150)
                    search.Enqueue(next1);

            }

            Console.WriteLine($"Valids {valids.Count}");
        }

        class Node
        {
            public int SizeIx;
            public int Num;
            public int TotalSoFar;
            public Node Prev;

            public Node(int ix, int num)
            {
                SizeIx = ix;
                Num = num;
                TotalSoFar = Sizes[ix]*num;
            }

            public Node(Node prev, int num)
            {
                Prev = prev;
                SizeIx = prev.SizeIx + 1;
                Num = num;
                TotalSoFar = prev.TotalSoFar + Sizes[SizeIx]*num;
            }

            public string Print()
            {
                return Prev?.Print() + $"\n{Sizes[SizeIx]} * {Num}";
            }

            public int NumContainers()
            {
                return Num + (Prev?.NumContainers() ?? 0);
            }
        }
    }
}