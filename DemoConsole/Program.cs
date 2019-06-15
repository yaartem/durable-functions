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
        static void Main()
        {
            Pizzeria n1= new Pizzeria();
            n1.Work();
            Console.ReadKey();
        }
    }
}