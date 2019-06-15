using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using System.Transactions;
using PizzaCooking.Domain;
using Utilities.Deterministic;

namespace DemoConsole
{
    class Pizzeria
    {
        DeterministicRandom rnd = new DeterministicRandom(123);
        DateTime currentTime = new DateTime(2019, 3, 27, 8, 59, 0);
        Queue<Order> orders = new Queue<Order>();
        int ordernum = 0;
        List<Deliverer> groupDeliverers = new List<Deliverer>();
        List<Pizzamaker> groupPizzamakers = new List<Pizzamaker>();
        Names name;
        Manager boss;
        List<(Order order,IEnumerator<int> process)> currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();
        Alice a;
        List<(Order order, IEnumerator<int> process)> nextInProcessing = new List<(Order order, IEnumerator<int> process)>();
        private IEnumerator<int> alisaProcess;
        private bool aliceProcessFinished;

        public Pizzeria()
        {
            for (int i = 1; i <= 25; i++)
            {
                name = (Names)rnd.Next(0, 32);
                groupDeliverers.Add(new Deliverer(i, name.ToString()));
            }

            for (int i = 1; i <= 10; i++)
            {
                name = (Names)rnd.Next(0, 32);
                groupPizzamakers.Add(new Pizzamaker(i, name.ToString(), rnd));
            }
            name = (Names)rnd.Next(0, 32);
            boss = new Manager(name.ToString());

            a = new Alice(boss, currentlyInProcessing, groupPizzamakers, rnd);

            alisaProcess = a.GetSequence().GetEnumerator();

            aliceProcessFinished = false;
        }

        public void Work()
        {
            while (currentTime.Hour < 23 || currentlyInProcessing.Count > 0)
            {
                if (rnd.Next(1, 101) > 60 && currentTime.Hour < 23)
                {
                    orders.Enqueue(
                    new Order(TimeSpan.FromMinutes(rnd.Next(15, 31)), currentTime, ordernum, rnd)
                    );
                    ordernum++;
                }

                nextInProcessing.Clear();

                Thread.Sleep(0);
                currentTime = currentTime.AddMinutes(1);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Current Time: {0}", currentTime);
                Console.ForegroundColor = ConsoleColor.White;
                a.CurrentTime = currentTime;

                if (!aliceProcessFinished)
                {
                    aliceProcessFinished = !alisaProcess.MoveNext();
                }


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
                        nextInProcessing.Add(item);
                        foreach (Pizzamaker i in groupPizzamakers) i.CheckOrder(item.order); //убрать
                        foreach (Deliverer g in groupDeliverers) g.CheckOrder(item.order); //убрать
                    }
                    else
                    {
                        item.process.Dispose();
                    }
                }
                currentlyInProcessing.Clear();
                currentlyInProcessing.AddRange(nextInProcessing);
            }
            alisaProcess.MoveNext();
        }
        
    }
}
