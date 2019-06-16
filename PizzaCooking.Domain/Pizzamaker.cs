using System;
using System.Collections.Generic;
using System.Text;
using durable_functions.Framework;
using Utilities.Deterministic;
namespace PizzaCooking.Domain
{
    public class Pizzamaker
    {
        readonly DeterministicRandom rnd;
        private readonly ILogger _logger;
        public int PizzamakerNum { set; get; }
        public string Name { get; }
        public int OrdersDone { set; get; }
        private bool Busy { get; set; }
        private bool ForgotToTake { set; get; }

        public Pizzamaker(int pizzamakerNum, string name,DeterministicRandom rnd, ILogger logger)
        {
            this.rnd = rnd;
            _logger = logger;
            Name = name;
            PizzamakerNum = pizzamakerNum;
            Busy = false;
            OrdersDone=0;
        }

        public bool CheckOrder(Order order)
        {
            if (!Busy)
            {
                if (OrdersDone > 10)
                {
                    if (OrdersDone > 20) ForgotToTake = rnd.Next(0, 25) != 0;
                    else
                        ForgotToTake = rnd.Next(0, 4) != 0;
                }
                else ForgotToTake = rnd.Next(0, 2) == 0;
                
                if (!ForgotToTake)
                {
                    if (order.State == "Is Waiting To Be Cooked")
                    {
                        if (!order.TakenToCook)
                        {
                            _logger.Log($"Pizzamaker number {PizzamakerNum} Took Order number{order.OrderNumber} ");
                            order.TakenToCook = true;
                            Busy = true;
                            order.OrderTakenForCookingTime = order.CurrentTime;
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (order.CurrentTime != order.OrderTakenForCookingTime + order.TimeToCook) return false;
                Busy = false;
                OrdersDone++;
                _logger.Log($"Pizzamaker number {PizzamakerNum} is free.");
                return true;
            }

            return false;
        }
    }
}
