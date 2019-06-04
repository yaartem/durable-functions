using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using System.Transactions;
using PizzaCooking.Domain;

namespace DemoConsole
{

    public enum Names
    {
        Артём, Андрей, Антон, Борис,
        Валентин, Валера, Василий, Глеб,
        Джамал, Демид, Егор, Закир,
        Исмаил, Иван, Илья, Карен,
        Леонид, Мухаммед, Никита, Олег,
        Павел, Руслан, Сергей, Спартак,
        Тимур, Умар, Хасан, Шамиль,
        Эмиль, Юрий, Ян, Ярослав
    }

    class Program
    {
        static void Main()
        {
            Random rnd = new Random();

            var sw = Stopwatch.StartNew();

            var startTime = new DateTime(2019,3,27, 9,0,0);
            var currentTime = startTime;

            var orders = new Queue<Order>();
            var ordernum = 0;

            Names name;
            var groupDeliverers=new List<Deliverer>();
            for (int i = 1; i <= 20; i++)
            {
                name = (Names) rnd.Next(0, 32);
                groupDeliverers.Add(new Deliverer(i,name.ToString()));
            }
            var groupPizzamakers = new List<Pizzamaker>();
            for (int i = 1; i <= 10; i++)
            {
                name = (Names)rnd.Next(0, 32);
                groupPizzamakers.Add(new Pizzamaker(i, name.ToString()));
            }

            var currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();

            var count = 0;

            Alice a = new Alice();
            DateTime FraseSaid = currentTime;
            
            if (currentTime.Hour == 9 && currentTime.Minute == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", currentTime, a.helloWords[rnd.Next(0, 4)]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            while (currentTime.Hour < 23 || currentlyInProcessing.Count > 0)
            {
                if (rnd.Next(1,100)>90 && currentTime.Hour < 23)
                {
                    orders.Enqueue(
                    new Order(TimeSpan.FromMinutes(rnd.Next(15,31)), TimeSpan.FromMinutes(rnd.Next(5,11)),
                    currentTime, ordernum)
                    );
                    ordernum++;
                }
                Thread.Sleep(1);
                currentTime = currentTime.AddMinutes(1);
                Console.WriteLine("Current Time: {0}", currentTime);

                var nextInProcessing = new List<(Order order, IEnumerator<int> process)>();

                if (orders.Count > 0)
                {
                    var order = orders.Peek();
                    if (order.OrderRecievedTime <= currentTime)
                    {
                        currentlyInProcessing.Add((order, order.GetSequence().GetEnumerator()));
                        orders.Dequeue();
                    }
                }

                foreach (var item in currentlyInProcessing)
                {
                    item.order.CurrentTime = currentTime;
                    if (item.process.MoveNext())
                    {
                        count++;
                        nextInProcessing.Add(item);
                        foreach (Pizzamaker i in groupPizzamakers) i.CheckOrder(item.order);
                        foreach (Deliverer g in groupDeliverers) g.CheckOrder(item.order);
                    }

                    else
                    {
                        item.process.Dispose();
                    }
                }
                if (currentTime.Hour==13 && currentTime.Minute==0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("{0} {1}", currentTime, a.halfWords[rnd.Next(0, 2)]);
                    Console.ForegroundColor = ConsoleColor.White;
                    currentTime = currentTime.AddMinutes(30);
                    FraseSaid = currentTime;
                }
                if (currentTime.Hour == 22 && currentTime.Minute == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("{0} {1}", currentTime, a.nearlyEndWords[rnd.Next(0, 2)]);
                    Console.ForegroundColor = ConsoleColor.White;
                    FraseSaid = currentTime;
                }
                currentlyInProcessing = nextInProcessing;
                if (nextInProcessing.Count > 10 && currentTime>=FraseSaid.AddMinutes(30))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("{0} {1}", currentTime, a.manyOrdersWords[rnd.Next(0, 2)]);
                    Console.ForegroundColor = ConsoleColor.White;
                    FraseSaid = currentTime;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("{0} {1}", currentTime, a.finishWords[rnd.Next(0,2)]);
            Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine($"Finished {count} synchronizations in {sw.Elapsed.TotalSeconds} sec.");

            Console.ReadKey();
        }
    }
}