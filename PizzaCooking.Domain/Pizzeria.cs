using System;
using System.Collections.Generic;
using durable_functions.Framework;
using PizzaCooking.Domain;
using Utilities.Deterministic;

namespace DemoConsole
{
    public class Pizzeria
    {
        private readonly ILogger _logger;
        readonly DeterministicRandom _rnd;
        public DateTime CurrentTime { get; set; }
        Queue<Order> orders = new Queue<Order>();
        List<Deliverer> groupDeliverers = new List<Deliverer>();
        List<Pizzamaker> groupPizzamakers = new List<Pizzamaker>();
        List<(Order order,IEnumerator<int> process)> currentlyInProcessing = new List<(Order order, IEnumerator<int> process)>();
        Alice a;
        List<(Order order, IEnumerator<int> process)> nextInProcessing = new List<(Order order, IEnumerator<int> process)>();
        private IEnumerator<int> alisaProcess;
        private bool aliceProcessFinished;

        public Pizzeria(ILogger logger, List<Deliverer> groupDeliverers, Manager boss, List<Pizzamaker> groupPizzamakers)
        {
            _logger = logger;
            _rnd = new DeterministicRandom(123);
            a = new Alice(boss, currentlyInProcessing, groupPizzamakers, _rnd, logger);
            this.groupDeliverers = groupDeliverers;
            this.groupPizzamakers = groupPizzamakers;
            alisaProcess = a.GetSequence().GetEnumerator();
            aliceProcessFinished = false;
        }
        
        public void TakeOrder(Order o)
        {
            orders.Enqueue(o); 
        }
        public IEnumerator<int> Work()
        {
            var endOfDay = CurrentTime.Date.AddHours(23);
            while (CurrentTime < endOfDay || currentlyInProcessing.Count > 0)
            {
                yield return 0;

                nextInProcessing.Clear();
                
                a.CurrentTime = CurrentTime;

                if (!aliceProcessFinished)
                {
                    aliceProcessFinished = !alisaProcess.MoveNext();
                }

                while (orders.Count > 0)
                {
                    var order = orders.Peek();
                    currentlyInProcessing.Add((order, order.GetSequence().GetEnumerator()));
                    orders.Dequeue();
                }

                foreach (var item in currentlyInProcessing)
                {
                    item.order.CurrentTime = CurrentTime;
                    if (item.process.MoveNext())
                    {
                        nextInProcessing.Add(item);
                        foreach (var pizzaMaker in groupPizzamakers)
                        {
                            pizzaMaker.CheckOrder(item.order);
                        }

                        foreach (var deliverer in groupDeliverers)
                        {
                            deliverer.CheckOrder(item.order);
                        }
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
    }
}
