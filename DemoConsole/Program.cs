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

            var currentTime = new DateTime(2019, 3, 27, 8, 59, 0);

            var orders = new Queue<Order>();
            var ordernum = 0;

            Names name;
            var groupDeliverers = new List<Deliverer>();
            for (int i = 1; i <= 25; i++)
            {
                name = (Names)rnd.Next(0, 32);
                groupDeliverers.Add(new Deliverer(i, name.ToString()));
            }

            var groupPizzamakers = new List<Pizzamaker>();
            for (int i = 1; i <= 10; i++)
            {
                name = (Names)rnd.Next(0, 32);
                groupPizzamakers.Add(new Pizzamaker(i, name.ToString()));
            }
            name = (Names)rnd.Next(0, 32);
            Manager boss = new Manager(name.ToString());

            var currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();

            var count = 0;

            Alice a = new Alice(boss);
            DateTime FraseSaid = currentTime;
            List<(Order order, IEnumerator<int> process)> nextInProcessing;


            var alisaProcess = a.GetSequence(boss, currentlyInProcessing, groupPizzamakers).GetEnumerator();

            var aliceProcessFinished = false;
            while (currentTime.Hour < 23 || currentlyInProcessing.Count > 0)
            {
                if (rnd.Next(1, 101) > 60 && currentTime.Hour < 23)
                {
                    orders.Enqueue(
                    new Order(TimeSpan.FromMinutes(rnd.Next(15, 31)), currentTime, ordernum)
                    );
                    ordernum++;
                }

                nextInProcessing = new List<(Order order, IEnumerator<int> process)>();

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
                        count++;
                        nextInProcessing.Add(item);
                        foreach (Pizzamaker i in groupPizzamakers) i.CheckOrder(item.order); //убрать
                        foreach (Deliverer g in groupDeliverers) g.CheckOrder(item.order); //убрать
                    }
                    else
                    {
                        item.process.Dispose();
                    }
                }
                //currentlyInProcessing = nextInProcessing;
                currentlyInProcessing.Clear();
                currentlyInProcessing.AddRange(nextInProcessing);
            }
            alisaProcess.MoveNext();
            Console.WriteLine($"Finished {count} synchronizations in {sw.Elapsed.TotalSeconds} sec.");

            Console.ReadKey();
        }
    }
}