using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using System.Transactions;
using PizzaCooking.Domain;
using Utilities.Deterministic;


namespace DemoConsole
{
    class Program
    {
        static void Main(string [] args)
        {
            Pizzeria n1= new Pizzeria();
            using (var iter = n1.Work())
            {
                while (iter.MoveNext())
                {
                }
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}