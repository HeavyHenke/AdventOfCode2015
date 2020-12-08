using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication5
{

    internal class Day24A
    {
        public void Calculate()
        {
            var input = File.ReadAllLines("Day24_test.txt")
                .Select(int.Parse)
                .ToList();

            var groupWeight = input.Sum() / 3;
        }

        public IEnumerable<List<int>> GetWaysToCreateSum(ICollection<int> packets, int targetWeight)
        {
            foreach (var pack in packets)
            {
                if(pack = targetWeight)
            }
        }
    }

    internal class Day24
    {
        public void Calculate()
        {
            var input = File.ReadAllLines("Day24.txt");
            List<int> weights = input.Select(int.Parse).ToList();
            var tot = weights.Sum();
            var perBox = tot/4;

            var node = new SumNode();
            var possibleSum = FindPossibleSum(node, weights, 0, perBox).ToList();

            var ordered = possibleSum.OrderBy(p => p._packages.Count).ThenBy(p2 => p2.QuantumEntanglement()).ToList();

            for (int a = 0; a < ordered.Count; a++)
            {
                var nodeA = ordered[a];

                for (int b = a + 1; b < ordered.Count; b++)
                {
                    var nodeB = ordered[b];
                    if(!nodeA.IsCompatible(nodeB))
                        continue;

                    for (int c = b + 1; c < ordered.Count; c++)
                    {
                        var nodeC = ordered[c];
                        if (!nodeA.IsCompatible(nodeC) || !nodeB.IsCompatible(nodeC))
                            continue;

                        for (int d = c + 1; d < ordered.Count; d++)
                        {
                            var nodeD = ordered[d];

                            if (nodeA.IsCompatible(nodeD) && nodeB.IsCompatible(nodeD) && nodeC.IsCompatible(nodeD))
                            {
                                Console.WriteLine($"Found it: {nodeA.QuantumEntanglement()}");
                                return;
                            }
                        }
                    }
                }
            }

        }

        private IEnumerable<SumNode> FindPossibleSum(SumNode startNode, List<int> weightsToTry, int startIdx, int sumToFind)
        {
            for(int i = startIdx; i < weightsToTry.Count; i++)
            {
                var node = new SumNode(startNode, weightsToTry[i]);

                if(node._sum > sumToFind)
                    yield break;

                if (node._sum == sumToFind)
                    yield return node;

                foreach (var child in FindPossibleSum(node, weightsToTry, i+1, sumToFind))
                    yield return child;
            }
        }

        class SumNode
        {
            public readonly HashSet<int> _packages;
            public readonly int _sum;

            public SumNode()
            {
                _packages = new HashSet<int>();
                _sum = 0;
            }

            public SumNode(SumNode prev, int package)
            {
                _packages = new HashSet<int>(prev._packages) {package};
                _sum = prev._sum + package;
            }

            public bool IsCompatible(SumNode other)
            {
                return _packages.Overlaps(other._packages) == false;
            }

            public long QuantumEntanglement()
            {
                long quantumEntanglement = _packages.First();
                foreach (var pack in _packages.Skip(1))
                {
                    quantumEntanglement *= pack;
                }
                return quantumEntanglement;
            }
        }
    }
}