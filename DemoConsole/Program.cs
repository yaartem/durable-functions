using System;


namespace DemoConsole
{
    class Program
    {
        static void Main(string [] args)
        {
            var currentTime = new DateTime(2019, 3, 27, 8, 59, 0);
            var n1= new Pizzeria();

            using (var iter = n1.Work())    
            {
                while (iter.MoveNext())
                {
                    currentTime = currentTime.AddMinutes(1);
                    n1.CurrentTime = currentTime;
                }
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}