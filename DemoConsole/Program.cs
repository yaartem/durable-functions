using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using PizzaCooking.Domain;

namespace DemoConsole
{
    class Program
    {
        static void Main()
        {
            Random rnd = new Random();

            var sw = Stopwatch.StartNew();

            var startTime = new DateTime(2019,3,27, 9,0,0);

            var orders = new Queue<Order>();
            for (var i = 0; i < 300; i++)
            {
                if(startTime.AddMinutes(3*i).Hour > 9 && startTime.AddMinutes(3 * i).Hour < 23)
                {
                    orders.Enqueue(
                    new Order(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(300),
                    startTime.AddMinutes(3 * i), i, "Пицца Сырный цыпленок и Додстер")
                    );
                }
                
            }
            
            var groupDeliverers = Enumerable.Range(1, 50).Select(i => new Deliverer(i)).ToList();

            var currentTime = startTime;

            var currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();

            var count = 0;

            Alice a = new Alice();
            
            if (currentTime.Hour == 9 && currentTime.Minute == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", currentTime, a.helloWords[rnd.Next(0, 4)]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            while (currentlyInProcessing.Count > 0 || orders.Count > 0)
            {
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
                        foreach (Deliverer i in groupDeliverers) i.CheckOrder(item.order);
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
                    }
                currentlyInProcessing = nextInProcessing;
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("{0} {1}", currentTime, a.finishWords[rnd.Next(0,2)]);
            Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine($"Finished {count} synchronizations in {sw.Elapsed.TotalSeconds} sec.");

            Console.ReadKey();
        }
    }
}