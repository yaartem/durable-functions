using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DemoConsole
{
    internal static class CoroutinePerformanceDemo
    {
        public static void DemonstrateCoroutinePerformance()
        {
            var n = 0;
            var sw = Stopwatch.StartNew();

            using (var ie = StateMachine())
            {
                while (ie.MoveNext())
                {
                    n++;
                }
            }
            
            sw.Stop();
            Console.WriteLine($"Completed {n} iterations in {sw.Elapsed}");
            var nsPerOperation = 1_000_000_000 * sw.Elapsed.TotalSeconds / n;
            Console.WriteLine($"{nsPerOperation}ns per operation");
        }

        private static IEnumerator<int> StateMachine()
        {
            for (var j = 0; j < 1_000_000; j++)
            {
                for (var i = 0; i < 10; i++)
                {
                    yield return 0;
                }
                
                for (var i = 0; i < 10; i++)
                {
                    yield return 0;
                }
            }
        }
    }
}