using System;

namespace DemoConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CoroutinePerformanceDemo.DemonstrateCoroutinePerformance();
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            
            PizzeriaBusinessDemo.DemonstratePizzeriaBusiness();
        }
    }
}