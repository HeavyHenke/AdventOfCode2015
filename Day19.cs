using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NGenerics.DataStructures.Queues;

namespace ConsoleApplication5
{
    internal class Day19A
    {

        public void CalculateA()
        {
            var input = File.ReadAllLines("day19.txt");
            var repl = new List<(string org, string repl)>();

            string goal = "";
            foreach (var line in input)
            {
                if (line == "")
                    continue;
                if (line.Contains("=>"))
                {
                    var parts = line.Split(new[] {" => "}, StringSplitOptions.RemoveEmptyEntries);
                    repl.Add((parts[0], parts[1]));
                }
                else
                {
                    goal = line;
                }
            }

            var found = new HashSet<string>();
            foreach (var kvp in repl)
            {
                foreach (var variant in GetAllVariants(goal, kvp.org, kvp.repl))
                    found.Add(variant);
            }

            Console.WriteLine("Num variants: " + found.Count);
        }


        public void CalculateB()
        {
            var input = File.ReadAllLines("day19.txt");
            var repl = new List<(string org, string repl)>();

            string goal = "";
            foreach (var line in input)
            {
                if (line == "")
                    continue;
                if (line.Contains("=>"))
                {
                    var parts = line.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
                    var search = (parts[0] == "e") ? "" : parts[0];
                    repl.Add((search, parts[1]));
                }
                else
                {
                    goal = line;
                }
            }

            repl = repl.OrderBy(r => r.repl.Length).ToList();

            int maxSearchDepth = int.MaxValue;
            var searchStack = new  Stack<(string mol, int depth)>();
            searchStack.Push((goal, 0));
            while (searchStack.Count > 0)
            {
                var node = searchStack.Pop();
                if(node.depth + 1 >= maxSearchDepth)
                    continue;

                foreach (var kvp in repl)
                {
                    foreach (var variant in GetAllVariants(node.mol, kvp.repl, kvp.org))
                    {
                        if (variant == "")
                        {
                            maxSearchDepth = node.depth + 1;
                            Console.WriteLine("One possibility: " + maxSearchDepth);
                            continue;
                        }

                        searchStack.Push((variant, node.depth + 1));
                    }
                }
            }

            Console.WriteLine("Found fastest solution to be: " + maxSearchDepth);
        }

        private IEnumerable<string> GetAllVariants(string mol, string search, string repl)
        {
            int pos = 0;
            while (true)
            {
                pos = mol.IndexOf(search, pos, StringComparison.Ordinal);
                if(pos < 0)
                    yield break;
                yield return mol.Substring(0, pos) + repl + mol.Substring(pos + search.Length);
                pos++;
            }
        }
    }

    internal class Day19
    {
        private List<Replacement> _replacements = new List<Replacement>();

        public class Replacement
        {
            public string _search;
            public string _replace;
            public Regex _regex;
            public int _cost = 1;
            public bool _isToE;

            public Replacement(string line)
            {
                var match = Regex.Match(line, @"(\w+) => (\w+)");
                //_search = match.Groups[1].Value;
                //_replace = match.Groups[2].Value;
                _search = match.Groups[2].Value;
                _replace = match.Groups[1].Value;

                _isToE = (_replace == "e");

                _regex = new Regex(_search, RegexOptions.Compiled);


            }

            public Replacement(string line, int cost)
                :this(line)
            {
                _cost = cost;
            }
        }

        public void Calculate()
        {
            var input = File.ReadAllLines("day19.txt");

            string goal = "";
            foreach (var line in input)
            {
                if(line == "")
                    continue;
                if (line.Contains("=>"))
                {
                    //replacements.Add(Tuple.Create(match.Groups[1].Value, match.Groups[2].Value));
                    _replacements.Add(new Replacement(line));
                }
                else
                {
                    goal = line;
                }
            }

            _replacements.Add(new Replacement("H => ORnPBPMgAr", 4));
            _replacements.Add(new Replacement("P => CaCaCaSiThCaCaSiThCaCaPBSiRnFAr", 14));
            _replacements.Add(new Replacement("Ca => CaCaSiThCaCaSiThCaCaCaCaCaCaSiRnFYFAr", 15));
            _replacements.Add(new Replacement("Ca => CaSiRnPTiTiBFYPBFAr", 8));

            _replacements.Add(new Replacement("Ca => PTiBPTiRnCaSiAlAr", 7));
            _replacements.Add(new Replacement("PB => PTiBPTiRnCaSiAlAr", 6));

            _replacements.Add(new Replacement("P => SiRnCaCaFAr", 3));

            _replacements.Add(new Replacement("Ca => CaCaSiThPRnFAr", 5));

            _replacements.Add(new Replacement("Ca => PBCaSiRnMgAr", 4));

            _replacements.Add(new Replacement("Ca => CaCaSiThCaSiRnTiMgAr", 7));
            _replacements.Add(new Replacement("Ca => SiThSiThCaCaSiRnMgAr", 7));

            _replacements.Add(new Replacement("P => CaCaSiRnFAr", 3));

            _replacements.Add(new Replacement("B => TiBPTiRnCaSiAlAr", 6));

            _replacements.Add(new Replacement("Ca => CaPTiRnFAr", 3));
            _replacements.Add(new Replacement("PB => CaPTiRnFAr", 2));

            _replacements.Add(new Replacement("Ca => PBPBCaCaSiThCaPBSiThPRnFAr", 14));
            _replacements.Add(new Replacement("PB => PBPBCaCaSiThCaPBSiThPRnFAr", 13));

            _replacements.Add(new Replacement("Ca => SiThCaSiThCaSiThCaPTiBSiRnFYFAr", 13));
            _replacements.Add(new Replacement("Ca => CaCaPRnFAr", 3));

            _replacements.Add(new Replacement("Ca => CaPRnFAr", 2));

            _replacements.Add(new Replacement("SiRnSiAl => SiRnCaCaCaSiThCaRnCaFAr", 6));


            _replacements.Sort((r1, r2) => -r1._search.Length.CompareTo(r2._search.Length));

            var nodes = Search(goal);
            foreach (var node in nodes)
            {
                Console.WriteLine("Solution: " + node._depth);
            }


            //var parts = SplitOnAr(goal).ToList();
            //foreach (var part in parts)
            //{
            //    var nodes = SearchNoAr(part).ToList();
            //    Console.WriteLine(part + " => " + nodes.Count);
            //}

        }

        private IEnumerable<string> SplitOnAr(string input)
        {
            int searchStart = 0;
            int idx = input.IndexOf("Ar", StringComparison.Ordinal);
            while (idx > 0)
            {
                var split = input.Substring(searchStart, idx + 2 - searchStart);
                yield return split;

                searchStart = idx + 2;
                idx = input.IndexOf("Ar", searchStart, StringComparison.Ordinal);
            }

            yield return input.Substring(searchStart);
        }

        private IEnumerable<Node> SearchNoAr(string startNode)
        {
            var searchQueue = new Stack<Node>();
            searchQueue.Push(new Node(startNode));

            var visited = new Dictionary<string, int>();

            while (searchQueue.Count > 0)
            {
                var mol = searchQueue.Pop();

                foreach (var replacement in _replacements)
                {
                    var possibilities = GetReplacements(mol._molecule, replacement);

                    foreach (var possibility in possibilities)
                    {
                        if (possibility.Count(p => p == 'e') > 1)
                            continue;

                        int alreadyVisitedDepth;
                        if (visited.TryGetValue(possibility, out alreadyVisitedDepth))
                        {
                            if (alreadyVisitedDepth <= mol._depth + 1)
                                continue;
                        }
                        visited[possibility] = mol._depth + 1;

                        var node = new Node(mol, possibility, replacement._cost);
                        if (possibility.Contains("Ar") == false)
                        {
                            yield return node;
                            continue;
                        }

                        searchQueue.Push(node);
                    }
                }
            }
        }

        private IEnumerable<Node> Search(string startNode)
        {
            var searchQueue = new PriorityQueue<Node, int>(PriorityQueueType.Maximum);
            searchQueue.Add(new Node(startNode), 0);

            var visited = new Dictionary<string, int>();
            int numVisited = 0;

            DateTime checkPoint = DateTime.Now;

            while (searchQueue.Count > 0)
            {
                var mol = searchQueue.Dequeue();

                foreach (var replacement in _replacements)
                {
                    var possibilities = GetReplacements(mol._molecule, replacement);

                    foreach (var possibility in possibilities)
                    {
                        if(mol._hasE && possibility.Contains('e'))
                            continue;

                        int alreadyVisitedDepth;
                        if (visited.TryGetValue(possibility, out alreadyVisitedDepth))
                        {
                            if (alreadyVisitedDepth <= mol._depth + 1)
                                continue;
                        }
                        visited[possibility] = mol._depth + 1;

                        numVisited++;
                        if (numVisited % 100000 == 0)
                        {
                            var time = (DateTime.Now - checkPoint);
                            Console.WriteLine($"Wokring on mol {possibility}, visited {numVisited} molecules so far. ({time.TotalSeconds}) {searchQueue.Count} {mol._depth}");
                            checkPoint = DateTime.Now;
                        }

                        var node = new Node(mol, possibility, replacement._cost);
                        if (possibility == "e")
                        {
                            yield return node;
                            continue;
                        }

                        if(node._depth <= 207)
                            searchQueue.Enqueue(node, node._depth);
                    }
                }
            }
        }

        class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                return x._depth.CompareTo(y._depth);
            }
        }

        [DebuggerDisplay("{_molecule} - {_depth}")]
        class Node
        {
            public Node _prev;
            public string _molecule;
            public int _depth;
            public bool _hasE;

            public Node(string start)
            {
                _molecule = start;
                _depth = 0;
                _hasE = start.Contains('e');
            }

            public Node(Node prev, string mol, int cost)
            {
                _prev = prev;
                _molecule = mol;
                _depth = prev._depth + cost;
                _hasE = mol.Contains('e');
            }
        }

        private IEnumerable<string> GetReplacements(string input, Replacement repl)
        {
            int idx = input.IndexOf(repl._search, StringComparison.Ordinal);
            while (idx >= 0)
            {
                string str2 = input.Remove(idx, repl._search.Length);
                str2 = str2.Insert(idx, repl._replace);
                yield return str2;

                idx = input.IndexOf(repl._search, idx + 1, StringComparison.Ordinal);
            }
        }
    }
}