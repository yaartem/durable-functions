using System;
using System.Collections.Generic;

namespace Dodo.Bus
{
    public class Logger : ILogger
    {
        Queue<string> logEvents = new Queue<string>();

        public void Info(string info)
        {
            // logEvents.Enqueue($"{DateTime.Now.ToString()} {info}");
            Console.WriteLine($"{DateTime.Now.ToString()} {info}");
        }

        public void Flush()
        {
            var count = 0L;
            while (logEvents.TryDequeue(out var line))
            {
                //if (count++ % 1000 == 0)
                Console.WriteLine(line);
            }
        }
    }
}
