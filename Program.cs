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
            new Day24A().Calculate();

            Console.WriteLine("\nDone.");
            Console.ReadKey();
        }

    }
}
