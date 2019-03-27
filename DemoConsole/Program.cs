using System;
using System.Collections.Generic;
using System.Threading;
using PizzaCooking.Domain;

namespace DemoConsole
{ 
    class Program
    {
        static void Main()
        { 
            var startTime = new DateTime(2019,3,27);

            List<Order> orders = new List<Order>
            {
                new Order(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(300), startTime, 1)
            };

            Deliverer[] groupDeliverers = new Deliverer[2];
            groupDeliverers[0] = new Deliverer(111);
            groupDeliverers[1] = new Deliverer(222);

            var currentTime = startTime;

            var count = 1;

            for(int g=0;g<=orders.Count-1;g++)
            {
                var ie = orders[g].GetSequence().GetEnumerator();
                while (ie.MoveNext())
                {
                    foreach (Deliverer i in groupDeliverers) i.CheckOrder(orders[g]);
                    currentTime = currentTime.AddMinutes(1);
                    orders[g].CurrentTime = currentTime;
                    Console.WriteLine("Current Time: {0}", currentTime);
                    Thread.Sleep(200);
                }
                count++;
                orders.Add(new Order(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(300), currentTime, count));
                ie.Dispose();
            }
            Console.ReadKey();
        }
    }
}