using System;
using System.Linq;
using durable_functions.Framework;
using PizzaCooking.Domain;
using Utilities.Deterministic;

namespace DemoConsole
{
    public static class PizzeriaBusinessDemo
    {
        public static void DemonstratePizzeriaBusiness()
        {
            var logger = new SampleLogger();
            var random = new DeterministicRandom(123);
            var sampleDate = new DateTime(2019, 3, 27, 9, 0, 0);
            var sampleOrders = TestPizzeria.GenerateSampleOrders(sampleDate, random, logger);
            
            var logsBegin = 0;
            const int logsLen = 5;
            var maxlog=logsLen;
            var key = ConsoleKey.UpArrow;
            while (key != ConsoleKey.Q)
            {
                Console.Clear();
                if (key == ConsoleKey.DownArrow) logsBegin += 1;
                if (key == ConsoleKey.UpArrow) logsBegin -= 1;
                if (logsBegin < 0)
                {
                    logsBegin = 0;
                }

                if (logsBegin + logsLen - 1 >maxlog)
                {
                    logsBegin = maxlog - logsLen + 1;
                }
                var prevColor = Console.BackgroundColor;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Показываются шаги с {logsBegin} по {logsBegin+logsLen - 1}. Стрелка вверх-один шаг назад, стрелка вниз-один шаг вперед");
                Console.BackgroundColor = prevColor;

                var currentTime = sampleDate;
                var pizzeria = TestPizzeria.CreatePizzeria(logger);
                pizzeria.CurrentTime = currentTime;

                Console.WriteLine();

                var stepCount = 0;
                var nextOrderIndex = 0;
                
                using (var iter = pizzeria.Work())
                {
                    bool ShowLogs()
                    {
                        return stepCount >= logsBegin && stepCount < logsBegin + logsLen;
                    }

                    logger.SetLogginEnabled(ShowLogs());
                    
                    while (iter.MoveNext())
                    {
                        while (
                            nextOrderIndex < sampleOrders.Count
                            && sampleOrders[nextOrderIndex].OrderRecievedTime <= currentTime)
                        {
                            pizzeria.TakeOrder(CreateOrder(sampleOrders[nextOrderIndex], logger));
                            nextOrderIndex++;
                        }
                        pizzeria.CurrentTime = currentTime;
                        
                        logger.LogColored($"Step: {stepCount} Current Time: {currentTime}", ConsoleColor.Green);
                        
                        stepCount++;
                        currentTime = currentTime.AddMinutes(1);
                        
                        logger.SetLogginEnabled(ShowLogs());
                    }
                    
                }

                maxlog = stepCount;

                Console.WriteLine("Press any key to continue");
                key = Console.ReadKey().Key;
            }
        }

        private static Order CreateOrder(OrderEvent e, ILogger logger)
        {
            return new Order(
                e.TimeToDeliver,
                e.OrderRecievedTime,
                e.OrderNumber,
                e.Meals.ToList(),
                e.Pizzas.ToList(),
                e.Drinks.ToList(), 
                logger);
        }
    }
}