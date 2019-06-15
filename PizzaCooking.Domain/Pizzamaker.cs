using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Deterministic;
namespace PizzaCooking.Domain
{
    public class Pizzamaker
    {
        readonly DeterministicRandom rnd;
        public int PizzamakerNum { set; get; }
        public string Name { get; }
        public int OrdersDone { set; get; }
        private bool Busy { get; set; }
        private bool ForgotToTake { set; get; }

        public Pizzamaker(int pizzamakerNum, string name,DeterministicRandom rnd)
        {
            this.rnd = rnd;
            Name = name;
            PizzamakerNum = pizzamakerNum;
            Busy = false;
            OrdersDone=0;
        }

        public void CheckOrder(Order order)
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
                            Console.WriteLine("Pizzamaker number {0} Took Order number{1} ", PizzamakerNum, order.OrderNumber);
                            order.TakenToCook = true;
                            Busy = true;
                            order.OrderTakenForCookingTime = order.CurrentTime;
                        }
                    }
                }
            }
            else
            {
                if (order.CurrentTime != order.OrderTakenForCookingTime + order.TimeToCook) return;
                Busy = false;
                OrdersDone++;
            }
        }
    }
}
