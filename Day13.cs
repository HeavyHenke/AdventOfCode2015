using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Day13
    {
        public void Calculate()
        {
            var input = File.ReadAllLines("day13.txt");
            var regex = new Regex(@"(\w+) would (\w+) (\d+) happiness units by sitting next to (\w+).");

            var happyness = new Dictionary<Tuple<string, string>, int>();

            var users = new HashSet<string>();

            foreach (var line in input)
            {
                var match = regex.Match(line);
                string user = match.Groups[1].Value;
                int factor = (match.Groups[2].Value == "gain") ? 1 : -1;
                int change = factor * int.Parse(match.Groups[3].Value);
                string other = match.Groups[4].Value;

                happyness.Add(Tuple.Create(user, other), change);
                users.Add(user);
            }

            foreach (var user in users)
            {
                happyness.Add(Tuple.Create("me", user), 0);
                happyness.Add(Tuple.Create(user, "me"), 0);
            }
            users.Add("me");

            var searchQueue = new Queue<Node>();
            foreach (var user in users)
            {
                searchQueue.Enqueue(new Node(user));
            }

            int maxHappy = int.MinValue;
            Node maxNode = null;

            while (searchQueue.Count > 0)
            {
                var node = searchQueue.Dequeue();
                foreach (var nextUser in users.Except(node.ChainedUsers))
                {
                    var nextNode = new Node(nextUser, node);
                    if (nextNode.ChainedUsers.Count == users.Count)
                    {
                        var happy = nextNode.CalculateHappiness(happyness);
                        if (happy > maxHappy)
                        {
                            maxHappy = happy;
                            maxNode = nextNode;
                        }
                    }
                    else
                        searchQueue.Enqueue(nextNode);

                }
            }
        }

        class Node
        {
            private readonly string _user;
            private Node _prev;

            private Node First
            {
                get
                {
                    if (_prev == null)
                        return this;
                    return _prev.First;
                }
            }

            public readonly HashSet<string> ChainedUsers;

            public Node(string start)
            {
                _user = start;
                ChainedUsers = new HashSet<string> { _user };
            }

            public Node(string user, Node prev)
            {
                _user = user;
                _prev = prev;
                ChainedUsers = new HashSet<string>(prev.ChainedUsers) { _user };
            }

            public int CalculateHappiness(Dictionary<Tuple<string, string>, int> happyVector)
            {
                int happy = 0;

                happy += happyVector[Tuple.Create(_user, _prev._user)];
                happy += happyVector[Tuple.Create(_user, First._user)];
                happy += happyVector[Tuple.Create(First._user, _user)];

                happy += _prev.CalculateHappiness(happyVector, this);

                return happy;
            }

            private int CalculateHappiness(Dictionary<Tuple<string, string>, int> happyVector, Node next)
            {
                int happy = 0;

                happy += happyVector[Tuple.Create(_user, next._user)];

                if (_prev != null)
                {
                    happy += happyVector[Tuple.Create(_user, _prev._user)];
                    happy += _prev.CalculateHappiness(happyVector, this);
                }

                return happy;
            }


            private string GetConfig()
            {
                return _user + ' ' + _prev?.GetConfig();
            }

        }
    }
}