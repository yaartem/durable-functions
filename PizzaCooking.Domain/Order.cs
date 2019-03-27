using System;
using System.Collections.Generic;
using System.IO;

namespace PizzaCooking.Domain
{
    public class Order
    {
        static StreamWriter sw = new StreamWriter("Text.txt");
        public bool Taken=false;
        public IEnumerable<int> GetSequence()
        {
            while (CurrentTime<OrderRecievedTime+TimeToCook)
            {
                State = "Is Cooking";
                ShowState();
                yield return 1;
            }

            while (CurrentTime >= OrderRecievedTime + TimeToCook && !Taken)
            {
                State = "Is Ready";
                ShowState();
                yield return 2;
            }

            while (CurrentTime < TimeTaken+TimeToDeliver)
            {
                State = "Is Delivering";
                ShowState();
                yield return 3;
            }
        }

        public Order(TimeSpan timeToDeliver, TimeSpan timeToCook, DateTime orderRecievedTime, int orderNumber)
        {
            TimeToDeliver = timeToDeliver;
            TimeToCook = timeToCook;
            OrderRecievedTime = orderRecievedTime;
            OrderNumber = orderNumber;
        }

        public TimeSpan TimeToDeliver{get;}
        public TimeSpan TimeToCook { get; }
        public DateTime OrderRecievedTime { get; }
        public int OrderNumber { get; }
        public DateTime CurrentTime { get; set; }
        public string State { get; set; }
        public DateTime TimeTaken {get; set;}

        public void ShowState()
        {
            sw.WriteLine("{0} {1} {2}", OrderNumber, State, CurrentTime);
            Console.Write("Order number {0} {1} ", OrderNumber, State);
        }
    }
}