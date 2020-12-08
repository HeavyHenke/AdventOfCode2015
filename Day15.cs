using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day15
    {
        public void Calculate()
        {
            var input = File.ReadAllLines("day15.txt");
            var regex = new Regex(@"(?<name>\w+): capacity (?<capacity>-?\d+), durability (?<durability>-?\d+), flavor (?<flavor>-?\d+), texture (?<texture>-?\d+), calories (?<calories>-?\d+)");

            var list = new List<Ingrediens>();
            foreach (var ingredien in input)
            {
                var match = regex.Match(ingredien);
                var ing = new Ingrediens
                {
                    Name = match.Groups["name"].Value,
                    Capacity = int.Parse(match.Groups["capacity"].Value),
                    Durability = int.Parse(match.Groups["durability"].Value),
                    Flavor = int.Parse(match.Groups["flavor"].Value),
                    Texture = int.Parse(match.Groups["texture"].Value),
                    Calories = int.Parse(match.Groups["calories"].Value)
                };

                list.Add(ing);
            }

            var searchList = new Stack<Node>();
            for (int i = 0; i <= 100; i++)
            {
                var node = new Node(list[0], i);
                searchList.Push(node);
            }


            int highScore = int.MinValue;
            Node bestNode;
            while (searchList.Count > 0)
            {
                var node = searchList.Pop();

                if (node.IngIx == list.Count - 2)
                {
                    var lastNode = new Node(node, list[node.IngIx+1], node.AmountLeft);
                    if (lastNode.Calories() == 500)
                    {
                        int score = lastNode.Score();
                        if (score > highScore)
                        {
                            highScore = score;
                            bestNode = lastNode;
                        }
                    }
                    continue;
                }

                for (int amount = 0; amount <= node.AmountLeft; amount++)
                {
                    var nextNode = new Node(node, list[node.IngIx+1], amount);
                    searchList.Push(nextNode);
                }
            }

        }

        class Node
        {
            public Node Prev;
            public Ingrediens Ing;
            public int Amount;

            public int AmountLeft;

            public int IngIx;

            public Node(Ingrediens ing, int amount)
            {
                Ing = ing;
                Amount = amount;
                AmountLeft = 100 - amount;
                IngIx = 0;
            }

            public Node(Node prev, Ingrediens ing, int amount)
            {
                Prev = prev;
                Ing = ing;
                Amount = amount;
                AmountLeft = prev.AmountLeft - amount;
                IngIx = prev.IngIx + 1;
            }

            public int Score()
            {
                var allNodes = Nodes().ToList();
                int capacity = allNodes.Sum(n => n.Ing.Capacity * n.Amount);
                int durability = allNodes.Sum(n => n.Ing.Durability * n.Amount);
                int flavor = allNodes.Sum(n => n.Ing.Flavor * n.Amount);
                int texture = allNodes.Sum(n => n.Ing.Texture * n.Amount);

                if (capacity < 0) capacity = 0;
                if (durability < 0) durability = 0;
                if (flavor < 0) flavor = 0;
                if (texture < 0) texture = 0;

                return capacity*durability*flavor*texture;
            }

            public int Calories()
            {
                return Nodes().Sum(n => n.Ing.Calories*n.Amount);
            }

            private IEnumerable<Node> Nodes()
            {
                yield return this;
                if (Prev != null)
                    foreach (var node in Prev.Nodes())
                        yield return node;
            }
        }

        class Ingrediens
        {
            public string Name;
            public int Capacity;
            public int Durability;
            public int Flavor;
            public int Texture;
            public int Calories;
        }
    }
}