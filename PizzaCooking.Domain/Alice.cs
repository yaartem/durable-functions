using System;
using System.Collections.Generic;

namespace PizzaCooking.Domain
{
    public class Alice
    {
        Random rnd = new Random(); // отключить
        public List<string> helloWords = new List<string>();
        public List<string> halfWords = new List<string>();
        public List<string> finishWords = new List<string>();
        public List<string> manyOrdersWords = new List<string>();
        public List<string> nearlyEndWords = new List<string>();

        public DateTime CurrentTime { get; set; }
        public DateTime FraseSaid { get; set; }

        /*

                while (не наступило время 9:00) {

                    // Все, что обычно
                    WhatISayAllTHeTime();

                    yield return 0; //
                }
                то, что один раз после 9

                while (не наступило время 10)
                {
                    WhatISayAllTHeTime();

                    yield return 0; //
                }

                // Наказать опоздавших
                BlameLaties();


                while (не наступило время 22)
                {
                    WhatISayAllTHeTime();

                    yield return 0; //
                }

                while (позже 22, и до упора)
                {
                    WhatISayAllTHeTime();

                    WhatISayJustBeforeShiftTermination();

                    yield return 0; //
                }

         */

        public IEnumerable<int> GetSequence(Manager boss, List<(Order order,
            IEnumerator<int> process)> currentlyInProcessing, List<Pizzamaker> groupPizzamakers)
        {
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(9))
            {
                yield return 0;
            }


                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", CurrentTime, helloWords[rnd.Next(0, 4)]);
                Console.ForegroundColor = ConsoleColor.White;
                yield return 0;
          


            
            
                Console.ForegroundColor = ConsoleColor.DarkRed;
                BlameLaties(groupPizzamakers);
                Console.ForegroundColor = ConsoleColor.White;
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(13))
            {
                yield return 0;
            }

           
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", CurrentTime, halfWords[rnd.Next(0, 2)]);
                Console.ForegroundColor = ConsoleColor.White;
                CurrentTime = CurrentTime.AddMinutes(30);
                FraseSaid = CurrentTime;
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(22))
            {
                yield return 0;
            }
            
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", CurrentTime, nearlyEndWords[rnd.Next(0, 2)]);
                Console.ForegroundColor = ConsoleColor.White;
                FraseSaid = CurrentTime;
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(23))
            {
                yield return 0;
            }
           
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0}", finishWords[rnd.Next(0, 2)]);
                Console.WriteLine("Есть люди как люди, а есть {0}! {1}!Слишком много заказов!",
                    groupPizzamakers[BestPizzamaker(groupPizzamakers)].Name, groupPizzamakers[BestPizzamaker(groupPizzamakers)].OrdersDone);
                Console.WriteLine();
                foreach (var man in groupPizzamakers)
                {
                    if (man.OrdersDone > 20)
                    {
                        Console.WriteLine("Большой молодец-{0}, {1} выполнено заказов", man.Name, man.OrdersDone);
                    }
                    else
                    {
                        if (man.OrdersDone > 10)
                        {
                            Console.WriteLine("Молодец-{0}, {1} выполнено заказов", man.Name, man.OrdersDone);
                        }
                        else
                        {
                            Console.WriteLine("Ничего так-{0}, {1} выполнено заказов", man.Name, man.OrdersDone);
                            Console.WriteLine("Шучу, умница ты, {0}", man.Name);
                        }
                    }
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                yield return 0;
            
            while (currentlyInProcessing.Count > 10 && CurrentTime >= FraseSaid.AddMinutes(15)) //many orders
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", CurrentTime, manyOrdersWords[rnd.Next(0, 3)]);
                Console.ForegroundColor = ConsoleColor.White;
                FraseSaid = CurrentTime;
                yield return 0;
            }
            while (CurrentTime >= FraseSaid.AddMinutes(60) && CurrentTime.Hour < 23 && currentlyInProcessing.Count != 0) //best
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                switch (rnd.Next(1, 3))
                {
                    case 1:
                        Console.WriteLine("{0} выполнил больше всех заказов! Можно и перерыв на кофе",
                            groupPizzamakers[BestPizzamaker(groupPizzamakers)].Name);
                        FraseSaid = CurrentTime;
                        break;
                    case 2:
                        Console.WriteLine("Новый король пиццы-{0}, больше всего заказов за сегодня!",
                            groupPizzamakers[BestPizzamaker(groupPizzamakers)].Name);
                        FraseSaid = CurrentTime;
                        break;
                    default:
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
                yield return 0;
            }
            foreach (var item in currentlyInProcessing)
            {
                while (item.order.State == "Is Waiting To Be Cooked")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (item.order.Pizzas >= 3 || item.order.AnyDrinks)
                    {
                        if (item.order.Pizzas >= 3)
                        {
                            switch (rnd.Next(1, 4))
                            {
                                case 1:
                                    Console.WriteLine("Кто так много ест? Заказ№{0}! Там больше трех пицц", item.order.OrderNumber);
                                    break;
                                case 2:
                                    Console.WriteLine("Какой-то праздник намечается? Тогда откуда больше 3 пицц в заказе№{0}?", item.order.OrderNumber);
                                    break;
                                default:
                                    Console.WriteLine("Заказ№{1} <<{0}>>-многовато пицц для одного заказа", item.order.Content, item.order.OrderNumber);
                                    break;
                            }
                        }
                        else
                        {
                            switch (rnd.Next(1, 4))
                            {
                                case 1:
                                    Console.WriteLine("Напитки в заказе! Наверное, в пустыню едет...");
                                    break;
                                case 2:
                                    Console.WriteLine("Заказ№{0} с напитками, не забудьте", item.order.OrderNumber);
                                    break;
                                default:
                                    Console.WriteLine("Стоит отметить, что заказ№{1} <<{0}>> имеет напитки", item.order.Content, item.order.OrderNumber);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (!item.order.TakenToCook)
                        {
                            switch (rnd.Next(1, 4))
                            {
                                case 1:
                                    Console.WriteLine("Господа, заказ <<{0}>> №{1} не готов! Пора бы им заняться", item.order.Content, item.order.OrderNumber);
                                    break;
                                case 2:
                                    Console.WriteLine("<<{0}>>, заказ №{1} и пиццамейкер! Надо бы сделать", item.order.Content, item.order.OrderNumber);
                                    break;
                                case 3:
                                    Console.WriteLine("<<{0}>>, заказ №{1}. За работу, дорогие!", item.order.Content, item.order.OrderNumber);
                                    break;
                                default:
                                    break;
                            }
                        }

                    }

                    Console.ForegroundColor = ConsoleColor.White;
                
                }

                while (item.order.State == "Is Ready To Be Taken")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (!item.order.TakenToDeliver)
                    {
                        switch (rnd.Next(1, 3))
                        {
                            case 1:
                                Console.WriteLine("Травка зеленеет, солнышко блестит, заказ№{0} к курьерам летит!", item.order.OrderNumber);
                                break;
                            case 2:
                                Console.WriteLine("<<{0}>> выполнен! Доставщики, ваш выход", item.order.Content);
                                break;
                            default:
                                break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    yield return 0;
                }
            }
        }

        public Alice(Manager boss)
        {
            helloWords.Add("Здравствуйте, дорогие, какой чудесный день, чтобы побить рекорды!");
            helloWords.Add("Привет, ребята, новый день-новые заказы!");
            helloWords.Add("Всем привет, готовы работать? Лично я всегда готова!");
            helloWords.Add("9:00, работа началась! Всем хорошего дня!");
            halfWords.Add("13:00, пора подкрепиться, ребята");
            halfWords.Add("Победа, победа, вместо обеда! Пойдеи кушать, друзья");
            finishWords.Add("Конец Смены! Хорошая работа, ребята!");
            finishWords.Add("Вот и пришел конец нашей работе, можно по домам!");
            manyOrdersWords.Add("Поторопитесь, в очереди много заказов!");
            manyOrdersWords.Add("Стало многовато заказов! Пора поднапрячься, любимые");
            manyOrdersWords.Add("Ребята, помогите! " + boss.Name + " пытается закрыть смену. Заказов многовато стало");
            nearlyEndWords.Add("Не спать! До конца смены всего час!");
            nearlyEndWords.Add("Час до конца! Последние заказы! Крепитесь!");
        }

        public void BlameLaties(List<Pizzamaker> guys)
        {
            string laties = " ";
            foreach (var i in guys)
            {
                if (i.IsLate())
                {
                    if (laties == " ")
                    {
                        laties += i.Name;
                    }
                    else
                    {
                        laties = laties + ", " + i.Name;
                    }
                }
            }
            if (laties != " ")
            {
                Console.WriteLine("Друзья мои,{0}, опаздываем! В следующий раз так не обижайте меня", laties);
            }

        }

        public int BestPizzamaker(List<Pizzamaker> guys)
        {
            int max = 0;
            for (int i = 1; i < guys.Count; i++)
            {
                if (guys[i].OrdersDone > guys[max].OrdersDone)
                    max = i;
            }
            return max;
        }

    }
}
