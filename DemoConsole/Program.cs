using System;
using Utilities.Deterministic;


namespace DemoConsole
{
    class Program
    {
        static void Main(string [] args)
        {
            var currentTime = new DateTime(2019, 3, 27, 8, 59, 0);
            var n1= new Pizzeria();
            int ordernum = 0;
            DeterministicRandom rnd = new DeterministicRandom(123);
            using (var iter = n1.Work())    
            {
                while (iter.MoveNext())
                {
                    if (rnd.Next(1, 101) > 60 && currentTime.Hour < 23) 
                    {
                        n1.TakeOrder(new PizzaCooking.Domain.Order(TimeSpan.FromMinutes(rnd.Next(15, 31)), currentTime, ordernum, rnd));
                        ordernum++;
                        
                    }
                    currentTime = currentTime.AddMinutes(1);
                    n1.CurrentTime = currentTime;
                }
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}