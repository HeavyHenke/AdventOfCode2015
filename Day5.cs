using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication5
{
    public class Day5
    {
        private readonly HashSet<char> _vowels = new HashSet<char>("aeiou"); 

        public void Calculate()
        {
            var input = File.ReadAllLines("Day5.txt");
            int numNice = input.Count(IsNice2);
            Console.WriteLine("Antal nice: " + numNice);
        }

        private bool IsNice(string input)
        {
            int numVovels = input.Count(c => _vowels.Contains(c));
            if (numVovels < 3) return false;

            bool foundDouble = false;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                {
                    foundDouble = true;
                    break;
                }
            }
            if (!foundDouble) return false;

            if (input.Contains("ab")) return false;
            if (input.Contains("cd")) return false;
            if (input.Contains("pq")) return false;
            if (input.Contains("xy")) return false;

            return true;
        }

        private bool IsNice2(string input)
        {
            bool foundRepeate = false;
            bool foundPlusOne = false;
            for (int i = 0; i < input.Length - 2; i++)
            {
                if (foundRepeate == false)
                {
                    var searchString = input[i] + "" + input[i + 1];
                    if (input.IndexOf(searchString, i + 2) > 0)
                    {
                        foundRepeate = true;
                    }
                }

                if (foundPlusOne == false)
                {
                    if (input[i] == input[i + 2])
                    {
                        foundPlusOne = true;
                    }
                }

                if (foundRepeate && foundPlusOne)
                    break;
            }
            return foundRepeate && foundPlusOne;
        }
    }
}