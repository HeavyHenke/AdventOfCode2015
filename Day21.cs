using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication5
{
    internal class Day21
    {
        const int StartBossHitPoints = 100;
        const int BossDamage = 8;
        const int BossArmor = 2;

        public void CalculateA()
        {
            int goldSpent;

            List<Node> weaponNodes = new List<Node>
            {
                new Node(8, 4, 0),
                new Node(10, 5, 0),
                new Node(25, 6, 0),
                new Node(40, 7, 0),
                new Node(74, 8, 0)
            };

            List<Node> armorNodes = new List<Node>();
            foreach (var node in weaponNodes)
            {
                armorNodes.Add(node.Add(13, 0, 1));
                armorNodes.Add(node.Add(31, 0, 2));
                armorNodes.Add(node.Add(53, 0, 3));
                armorNodes.Add(node.Add(75, 0, 4));
                armorNodes.Add(node.Add(102, 0, 5));
            }

            List<Node> rings = new List<Node>
            {
                new Node(25,1,0),
                new Node(50,2,0),
                new Node(100,3,0),
                new Node(20,0,1),
                new Node(40,0,2),
                new Node(80,0,3),
            };
            List<Node> ringNodes = new List<Node>();
            foreach (var node in weaponNodes.Concat(armorNodes))
            {
                // One ring:
                foreach (var ring in rings)
                {
                    ringNodes.Add(node.Add(ring));
                }

                // Two rings:
                var q = from r1 in rings
                        from r2 in rings
                        where r1._goldSpent != r2._goldSpent
                        select r1.Add(r2);
                foreach (var twoRings in q)
                {
                    ringNodes.Add(node.Add(twoRings));
                }

            }

            List<Node> winNodes = new List<Node>();
            foreach (var node in weaponNodes.Concat(armorNodes).Concat(ringNodes))
            {
                if (DoIWin(node._damage, node._armor))
                    winNodes.Add(node);
            }


            var sheepestWin = winNodes.OrderBy(n => n._goldSpent).First();
            // 158 too low

        }

        public void Calculate()
        {
            int goldSpent;

            List<Node> weaponNodes = new List<Node>
            {
                new Node(8, 4, 0),
                new Node(10, 5, 0),
                new Node(25, 6, 0),
                new Node(40, 7, 0),
                new Node(74, 8, 0)
            };

            List<Node> armorNodes = new List<Node>();
            foreach (var node in weaponNodes)
            {
                armorNodes.Add(node.Add(13, 0, 1));
                armorNodes.Add(node.Add(31, 0, 2));
                armorNodes.Add(node.Add(53, 0, 3));
                armorNodes.Add(node.Add(75, 0, 4));
                armorNodes.Add(node.Add(102, 0, 5));
            }

            List<Node> rings = new List<Node>
            {
                new Node(25,1,0),
                new Node(50,2,0),
                new Node(100,3,0),
                new Node(20,0,1),
                new Node(40,0,2),
                new Node(80,0,3),
            };
            List<Node> ringNodes = new List<Node>();
            foreach (var node in weaponNodes.Concat(armorNodes))
            {
                // One ring:
                foreach (var ring in rings)
                {
                    ringNodes.Add(node.Add(ring));
                }

                // Two rings:
                var q = from r1 in rings
                    from r2 in rings
                    where r1._goldSpent != r2._goldSpent
                    select r1.Add(r2);
                foreach (var twoRings in q)
                {
                    ringNodes.Add(node.Add(twoRings));
                }

            }

            List<Node> looseNodes = new List<Node>();
            foreach (var node in weaponNodes.Concat(armorNodes).Concat(ringNodes))
            {
                if (!DoIWin(node._damage, node._armor))
                    looseNodes.Add(node);
            }


            var expensivestLost = looseNodes.OrderByDescending(n => n._goldSpent).First();
        }

        class Node
        {
            public readonly int _goldSpent;
            public readonly int _damage;
            public readonly int _armor;

            public Node(int cost, int damage, int armor)
            {
                _goldSpent = cost;
                _damage = damage;
                _armor = armor;
            }

            public Node Add(int cost, int damage, int armor)
            {
                return new Node(_goldSpent + cost, _damage + damage, _armor + armor);
            }

            public Node Add(Node node)
            {
                return new Node(_goldSpent + node._goldSpent, _damage + node._damage, _armor + node._armor);
            }

        }


        private bool DoIWin(int damage, int armor)
        {
            int playerHitPoints = 100;
            int bossHitPoints = 100;

            while (true)
            {
                int d = Math.Max(1, damage - BossArmor);
                bossHitPoints -= d;
                if (bossHitPoints < 0)
                    return true;

                d = Math.Max(1, BossDamage - armor);
                playerHitPoints -= d;
                if (playerHitPoints < 0)
                    return false;
            }

        }

    }
}