using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;
using System.Transactions;
using durable_functions.Framework;
using PizzaCooking.Domain;
using Utilities.Deterministic;

namespace DemoConsole
{
    class Pizzeria
    {
        private readonly ILogger _logger;

        /// <summary>
        /// _rnd это генератор случайных чисел для имитации неоднозначного поведения.
        /// В данном исследовании генератор случайных чисел заменяет неоднозначность
        /// поведения реальных людей, например - сотрудников пиццерии.
        /// Также, случайным образом для теста задаются параметры, внутренние для пиццерии.
        /// </summary>
        readonly DeterministicRandom _rnd;
        public DateTime CurrentTime { get; set; }
        Queue<Order> orders = new Queue<Order>();
        
        List<Deliverer> groupDeliverers = new List<Deliverer>();
        List<Pizzamaker> groupPizzamakers = new List<Pizzamaker>();
        Names name;
        Manager boss;
        List<(Order order,IEnumerator<int> process)> currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();
        Alice a;
        List<(Order order, IEnumerator<int> process)> nextInProcessing = new List<(Order order, IEnumerator<int> process)>();
        private IEnumerator<int> alisaProcess;
        private bool aliceProcessFinished;

        /// <summary>
        /// TODO: вынести тестовую логику в класс TestPizzeria. Вся конфигурация получается извне через параметры конструктора.
        /// </summary>
        public Pizzeria(ILogger logger)
        {
            _logger = logger;
            _rnd = new DeterministicRandom(123);
            for (int i = 1; i <= 25; i++)
            {
                name = (Names)_rnd.Next(0, 32);
                groupDeliverers.Add(new Deliverer(i, name.ToString(), logger));
            }

            for (int i = 1; i <= 10; i++)
            {
                name = (Names)_rnd.Next(0, 32);
                groupPizzamakers.Add(new Pizzamaker(i, name.ToString(), _rnd, logger));
            }
            name = (Names)_rnd.Next(0, 32);
            boss = new Manager(name.ToString());

            a = new Alice(boss, currentlyInProcessing, groupPizzamakers, _rnd, logger);

            alisaProcess = a.GetSequence().GetEnumerator();

            aliceProcessFinished = false;
        }
        public void TakeOrder(Order o)
        {

            orders.Enqueue(o); 
                     
            
        }
        public IEnumerator<int> Work()
        {
            while (CurrentTime.Hour < 23 || currentlyInProcessing.Count > 0)
            {
                yield return 0;

                nextInProcessing.Clear();
 
                _logger.LogColored($"Current Time: {CurrentTime}", ConsoleColor.Green);
                a.CurrentTime = CurrentTime;

                if (!aliceProcessFinished)
                {
                    aliceProcessFinished = !alisaProcess.MoveNext();
                }


                if (orders.Count > 0)
                {
                    var order = orders.Peek();
                    if (order.OrderRecievedTime <= CurrentTime)
                    {
                        currentlyInProcessing.Add((order, order.GetSequence().GetEnumerator()));
                        orders.Dequeue();
                    }
                }

                foreach (var item in currentlyInProcessing)
                {
                    item.order.CurrentTime = CurrentTime;
                    if (item.process.MoveNext())
                    {
                        nextInProcessing.Add(item);
                        foreach (Pizzamaker i in groupPizzamakers) i.CheckOrder(item.order);
                        foreach (Deliverer g in groupDeliverers) g.CheckOrder(item.order);
                    }
                    else
                    {
                        item.process.Dispose();
                    }
                }
                currentlyInProcessing.Clear();
                currentlyInProcessing.AddRange(nextInProcessing);
            }
            if (!aliceProcessFinished)
            {
                aliceProcessFinished = !alisaProcess.MoveNext();
            }
        }

        void Log(string what)
        {
            _logger.Log(what);
        }
    }
}
