using System;

namespace PizzaCooking.Domain
{
    public class Deliverer
    {
        public Deliverer(int delivererNum, string name)
        {
            Name = name;
            DelivererNum = delivererNum;
            Busy = false;
        }
        public string Name { get; }
        public int DelivererNum { get; }
        private bool Busy { set; get; }
        public TimeSpan TimeToDeliver { get; set; }
        public DateTime TookOrderTime { get; set; }

        public void CheckOrder(Order order)
        {
            if (!Busy)
            {
                if (order.State == "Is Ready To Be Taken")
                {
                    if (!order.TakenToDeliver)
                    {
                        Console.WriteLine("Deliverer number {0} Took Order number{1} ", DelivererNum,
                            order.OrderNumber);
                        order.TakenToDeliver = true;
                        Busy = true;
                        order.TimeTaken = order.CurrentTime;
                        TookOrderTime = order.CurrentTime;
                        TimeToDeliver = order.TimeToDeliver;
                    }
                }
            }
            else
            {
                if (order.CurrentTime == TookOrderTime + TimeToDeliver + TimeToDeliver)
                    Busy = false;
            } 
        }
    }
}