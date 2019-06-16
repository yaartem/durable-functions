using System;
using durable_functions.Framework;

namespace PizzaCooking.Domain
{
    public class Deliverer
    {
        private readonly ILogger _logger;

        public Deliverer(int delivererNum, string name, ILogger logger)
        {
            _logger = logger;
            Name = name;
            DelivererNum = delivererNum;
            Busy = false;
        }
        public string Name { get; }
        public int DelivererNum { get; }
        private bool Busy { set; get; }
        public TimeSpan TimeToDeliver { get; set; }
        public DateTime TookOrderTime { get; set; }

        public bool CheckOrder(Order order)
        {
            if (!Busy)
            {
                if (order.State == "Is Ready To Be Taken")
                {
                    if (!order.TakenToDeliver)
                    {
                        _logger.Log($"Deliverer number {DelivererNum} Took Order number{order.OrderNumber}");

                        order.TakenToDeliver = true;
                        Busy = true;
                        order.TimeTaken = order.CurrentTime;
                        TookOrderTime = order.CurrentTime;
                        TimeToDeliver = order.TimeToDeliver;
                        return true;
                    }
                }
            }
            else
            {
                if (order.CurrentTime == TookOrderTime + TimeToDeliver + TimeToDeliver)
                {
                    Busy = false;
                    _logger.Log($"Deliverer number {DelivererNum} becomes free.");
                    return true;
                }
            }

            return false;
        }
    }
}