using System;
using System.Collections;
using System.Globalization;
using System.Net.Configuration;
using System.Security.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            new Day24().CalculateA();

            Console.WriteLine("\nDone.");
            Console.ReadKey();
        }

    }
}
