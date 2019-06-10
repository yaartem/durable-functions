using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaCooking.Domain
{
    public class Pizzamaker
    {
        Random rnd = new Random();
        public int PizzamakerNum { set; get; }
        public string Name { get; }
        public int OrdersDone { set; get; }
        private bool Busy { get; set; }
        private bool ForgotToTake { set; get; }

        public Pizzamaker(int pizzamakerNum, string name)
        {
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
                    if(OrdersDone > 20) ForgotToTake = rnd.Next(0, 100) == 0 ? false : true;
                    else
                        ForgotToTake = rnd.Next(0, 10) == 0 ? false : true;
                }  
                else ForgotToTake = rnd.Next(0, 2) == 0 ? true : false;

                if(!ForgotToTake)
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
                if (order.CurrentTime == order.OrderTakenForCookingTime + order.TimeToCook)
                {
                    Busy = false;
                    OrdersDone++;
                }
            }
        }

        public bool IsLate()
        {
            if (rnd.Next(1, 101) > 80)
            {
                return true;
            }
            else return false;
        }
    }
}
