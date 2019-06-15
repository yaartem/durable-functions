using System;
using durable_functions.Framework;
using Utilities.Deterministic;

namespace DemoConsole
{
    internal class Program
    {
        /// <summary>
        ///     TODO:
        ///     1. Вывод параметров, с какого шага по какой логируем
        ///     2. На заданный номер
        ///     3. Вверх-вниз
        ///     q - выход
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            var logsBegin = 0;
            var logsLen = 10;
            var key = ConsoleKey.A;
            while (key != ConsoleKey.Q)
            {
                Console.Clear();
                if (key == ConsoleKey.Z) logsBegin += logsLen;
                var logger = new SampleLogger();
                var iLogger = (ILogger) logger;

                var currentTime = new DateTime(2019, 3, 27, 8, 59, 0);
                var pizzeria = new Pizzeria(logger);
                pizzeria.CurrentTime = currentTime;
                var ordernum = 0;
                var rnd = new DeterministicRandom(123);

                Console.WriteLine();

                var stepCount = 0;

                using (var iter = pizzeria.Work())
                {
                    bool ShowLogs()
                    {
                        return stepCount >= logsBegin && stepCount < logsBegin + logsLen;
                    }

                    logger.SetLogginEnabled(ShowLogs());
                    while (iter.MoveNext())
                    {
                        logger.SetLogginEnabled(ShowLogs());
                        if (rnd.Next(1, 101) > 60 && currentTime.Hour < 23)
                        {
                            pizzeria.TakeOrder(TestPizzeria.CreateSampleOrder(
                                TimeSpan.FromMinutes(rnd.Next(15, 31)), currentTime, ordernum, rnd, iLogger));
                            ordernum++;
                        }

                        currentTime = currentTime.AddMinutes(1);
                        pizzeria.CurrentTime = currentTime;
                        stepCount++;
                    }
                }

                key = Console.ReadKey().Key;
            }

            Console.WriteLine("Press any key to continue");
            //Console.ReadKey();
        }
    }

    public class SampleLogger : ILogger
    {
        private bool _logsEnabled;

        public void Log(string what)
        {
            if (!_logsEnabled) return;
            Console.WriteLine(what);
        }

        public void SetLogginEnabled(bool enable)
        {
            _logsEnabled = enable;
        }
    }
}