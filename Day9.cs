using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day9
    {
        public void Calculate()
        {
            var input = File.ReadLines("day9.txt");
            var regex = new Regex(@"(?<from>\w*) to (?<to>\w*) = (?<distance>\d*)");

            var allTowns = new HashSet<string>();
            var distances = new Dictionary<Tuple<string, string>, int>();
            foreach (var line in input)
            {
                var match = regex.Match(line);
                string from = match.Groups["from"].Value;
                string to = match.Groups["to"].Value;
                int distance = int.Parse(match.Groups["distance"].Value);

                distances.Add(Tuple.Create(from, to), distance);
                distances.Add(Tuple.Create(to, from), distance);

                allTowns.Add(from);
                allTowns.Add(to);
            }


            var queue = new Queue<SearchNode>();
            foreach (var t in allTowns)
            {
                queue.Enqueue(new SearchNode(t));
            }


            SearchNode bestNode = null;
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.VisitedTowns == allTowns.Count)
                {
                    if (bestNode == null || node.TotalDistance > bestNode.TotalDistance)
                        bestNode = node;
                    continue;
                }

                foreach (var t in allTowns.Except(node.Visited))
                {
                    int distance = distances[Tuple.Create(node.Town, t)];
                    queue.Enqueue(new SearchNode(node, t, distance));
                }
            }
        }


        class SearchNode
        {
            public SearchNode Parent;
            public string Town;

            public HashSet<string> Visited;

            public int TotalDistance;
            public int VisitedTowns => Visited.Count;

            public SearchNode(string startingTown)
            {
                Town = startingTown;
                Visited = new HashSet<string> {startingTown};
                TotalDistance = 0;
            }

            public SearchNode(SearchNode parent, string town, int distance)
            {
                Parent = parent;
                Town = town;
                TotalDistance = parent.TotalDistance + distance;
                Visited = new HashSet<string>(parent.Visited) {town};
            }

        }
        
    }
}