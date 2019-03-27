using System;

namespace PizzaCooking.Domain
{
    public class Deliverer
    {
        public Deliverer(int delivererNum)
        {
            DelivererNum = delivererNum;
            Busy = false;
        }
        public int DelivererNum { get; }
        private bool Busy { get; set; }
        public TimeSpan TimeToDeliver { get; set; }
        public DateTime TookOrderTime { get; set; }
        public void CheckOrder(Order order)
        {
            if (!Busy)
            {
                if (order.State == "Is Ready")
                {
                    if (!order.Taken)
                    {
                        //Console.WriteLine("Deliverer number {0} Took Order number{1} ", DelivererNum, order.OrderNumber);
                        order.Taken = true;
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