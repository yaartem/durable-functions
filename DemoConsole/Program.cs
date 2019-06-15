using System;
using Utilities.Deterministic;


namespace DemoConsole
{
    class Program
    {
        static void Main(string [] args)
        {
            var currentTime = new DateTime(2019, 3, 27, 8, 59, 0);
            var pizzeria= new Pizzeria();
            pizzeria.CurrentTime = currentTime;
            int ordernum = 0;
            DeterministicRandom rnd = new DeterministicRandom(123);
            using (var iter = pizzeria.Work())
            {
                while (iter.MoveNext())
                {
                    if (rnd.Next(1, 101) > 60 && currentTime.Hour < 23) // external
                    {
                        pizzeria.TakeOrder( TestPizzeria.CreateSampleOrder(
                            TimeSpan.FromMinutes(rnd.Next(15, 31)), currentTime, ordernum, rnd));
                        ordernum++;
                    }
                    currentTime = currentTime.AddMinutes(1);
                    pizzeria.CurrentTime = currentTime;
                }
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}