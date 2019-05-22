using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PizzaCooking.Domain;

namespace DemoConsole
{ 
    class Program
    {
        static void Main()
        {
            var sw = Stopwatch.StartNew();

            var startTime = new DateTime(2019,3,27);

            var orders = new Queue<Order>();
            for (var i = 0; i < 10000; i++)
            {
                orders.Enqueue(
                    new Order(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(300),
                        startTime.AddMinutes(3 * i), i));
            }

            var groupDeliverers = Enumerable.Range(1, 20).Select(i => new Deliverer(i)).ToList();

            var currentTime = startTime;

            var currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();

            var count = 0;

            while (currentlyInProcessing.Count > 0 || orders.Count > 0)
            {
                currentTime = currentTime.AddMinutes(1);
                //Console.WriteLine("Current Time: {0}", currentTime);

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

                currentlyInProcessing = nextInProcessing;
            }

            Console.WriteLine($"Finished {count} synchronizations in {sw.Elapsed.TotalSeconds} sec.");
            
            Console.ReadKey();
        }
    }
}