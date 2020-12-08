using System;
using System.IO;

namespace ConsoleApplication5
{
    public class Day8
    {
        public void Calculate()
        {
            var input = File.ReadAllLines("day8.txt");

            int totLength = 0;
            int parsedLength = 0;

            foreach (var line in input)
            {
                string output = "\"";
                foreach (var chr in line)
                {
                    if (chr == '"')
                        output += "\\\"";
                    else if (chr == '\\')
                        output += "\\\\";
                    else
                        output += chr;
                }
                output += "\"";

                totLength += line.Length;
                parsedLength += output.Length;

                //var work = line.Replace(@"\\", @"/");
                //work = work.Replace("\\\"", "\"");

                //int idx = work.IndexOf("\\x");
                //while (idx > 0)
                //{
                //    work = work.Remove(idx, 4);
                //    work = work.Insert(idx, "¿");
                //    idx = work.IndexOf("\\x");
                //}

                //totLength += line.Length;
                //parsedLength += work.Length - 2;
            }

            Console.WriteLine("Diff: " + ( totLength - parsedLength));
        }
    }
}