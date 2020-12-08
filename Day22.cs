using System;
using System.Collections.Generic;

namespace ConsoleApplication5
{
    internal class Day22
    {
        public void Calculate()
        {
            var start = new Node();
            var searchQueue = new Queue<Node>();
            searchQueue.Enqueue(start);

            int iter = 0;
            
            while (searchQueue.Count > 0)
            {
                if(iter %1000000 == 0)
                    Console.WriteLine($"Iteration {iter} has {searchQueue.Count} nodes in queue");
                iter++;


                var node = searchQueue.Dequeue();

                // Hard effects
                node._stats._playerHit--;
                if (node._stats.IsLoose)
                    continue;

                // Player turn
                node._stats.DoEffects();
                if (node._stats.IsWin)
                {
                    Win(node);
                    continue;
                }
                if (node._stats.IsLoose)
                {
                    continue;
                }

                var nextNodes = new List<Node>();

                // Missile
                if (node._stats._playerMana >= 53)
                {
                    var missile = new Node(node, "missile", 53);
                    missile._stats._bossHit -= 4;
                    nextNodes.Add(missile);
                }

                // Drain
                if (node._stats._playerMana >= 73)
                {
                    var drain = new Node(node, "drain", 73);
                    drain._stats._bossHit -= 2;
                    drain._stats._playerHit += 2;
                    nextNodes.Add(drain);
                }

                // Shield 
                if (node._stats._shieldTurns == 0 && node._stats._playerMana >= 113)
                {
                    var shield = new Node(node, "shiled", 113);
                    shield._stats._shieldTurns = 6;
                    nextNodes.Add(shield);
                }

                // Poison 
                if (node._stats._poisonTurns == 0 && node._stats._playerMana >= 173)
                {
                    var poison = new Node(node, "poision", 173);
                    poison._stats._poisonTurns = 6;
                    nextNodes.Add(poison);
                }

                // Recharge 
                if (node._stats._rechargeTurns == 0 && node._stats._playerMana >= 229)
                {
                    var recharge = new Node(node, "recharge", 229);
                    recharge._stats._rechargeTurns = 5;
                    nextNodes.Add(recharge);
                }


                // Boss turn on each node
                foreach (var nextNode in nextNodes)
                {
                    if (nextNode._stats.IsWin)
                    {
                        Win(nextNode);
                        continue;
                    }

                    nextNode._stats.DoEffects();
                    if (nextNode._stats.IsWin)
                    {
                        Win(nextNode);
                        continue;
                    }

                    var damage = 8;
                    if (nextNode._stats._shieldTurns > 0)
                        damage -= 7;
                    nextNode._stats._playerHit -= damage;

                    if (nextNode._stats.IsWin)
                    {
                        Win(nextNode);
                        continue;
                    }

                    if (nextNode._stats.IsLoose)
                        continue;

                    if(_cheepestWin == null || _cheepestWin._manaSpent > nextNode._manaSpent)
                        searchQueue.Enqueue(nextNode);
                }
            }

            Console.WriteLine($"Cheepest win costed {_cheepestWin._manaSpent} mana");
        }

        private Node _cheepestWin;
        private void Win(Node node)
        {
            if (_cheepestWin == null)
            {
                _cheepestWin = node;
            }

            else if (node._manaSpent < _cheepestWin._manaSpent)
            {
                _cheepestWin = node;
            }
        }


        class Node
        {
            private readonly string _name;
            public Stats _stats;
            public int _manaSpent;
            private Node _prev;

            public Node()
            {
                _stats = new Stats
                {
                    _bossHit = 55,
                    _playerHit = 50,
                    _playerMana = 500
                };
            }

            public Node(Node node, string name, int manaSpent)
            {
                _prev = node;
                _name = name;
                _stats = node._stats.Clone();
                _stats._playerMana -= manaSpent;
                _manaSpent = node._manaSpent + manaSpent;
            }
        }

   

        class Stats
        {
            public int _playerHit;
            public int _playerMana;
            public int _bossHit;

            public int _shieldTurns = 0;
            public int _poisonTurns = 0;
            public int _rechargeTurns = 0;

            public bool IsWin => _bossHit <= 0 && _playerHit > 0;
            public bool IsLoose => _playerHit <= 0;

            public Stats Clone()
            {
                var clone = new Stats();
                clone._playerHit = _playerHit;
                clone._playerMana = _playerMana;
                clone._bossHit = _bossHit;
                clone._shieldTurns = _shieldTurns;
                clone._poisonTurns = _poisonTurns;
                clone._rechargeTurns = _rechargeTurns;
                return clone;
            }

            public void DoEffects()
            {
                if (_shieldTurns > 0)
                {
                    _shieldTurns--;
                }
                if (_poisonTurns > 0)
                {
                    _bossHit -= 3;
                    _poisonTurns--;
                }
                if (_rechargeTurns > 0)
                {
                    _playerMana += 101;
                    _rechargeTurns--;
                }
            }
        }
    }
}