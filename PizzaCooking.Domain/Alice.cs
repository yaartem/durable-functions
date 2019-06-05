using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PizzaCooking.Domain;

namespace PizzaCooking.Domain
{
    public class Alice
    {
        Random rnd = new Random();
        public List<string> helloWords = new List<string>();
        public List<string> halfWords = new List<string>();
        public List<string> finishWords = new List<string>();
        public List<string> manyOrdersWords = new List<string>();
        public List<string> nearlyEndWords = new List<string>();

        public void IntializeAlice(Manager boss)
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
            string laties=" ";
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

        public string BestPizzamaker(List<Pizzamaker> guys)
        {
            int max=0;
            for (int i = 1; i <= guys.Count; i++)
            {
                if (guys[i].OrdersDone > guys[i - 1].OrdersDone)
                    max = i;
            }
            return guys[max].Name;
        }

        public void AllFrases(Manager boss, ref DateTime currentTime, List<(Order order,
            IEnumerator<int> process)> currentlyInProcessing, List<Pizzamaker> groupPizzamakers)
        {
            var FraseSaid = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 9, 0, 0);
            IntializeAlice(boss);
            if (currentTime.Hour == 9 && currentTime.Minute == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", currentTime, helloWords[rnd.Next(0, 4)]);

                Console.ForegroundColor = ConsoleColor.White;
            } // Поздороваться утром
            if (currentTime.Hour == 13 && currentTime.Minute == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", currentTime, halfWords[rnd.Next(0, 2)]);
                Console.ForegroundColor = ConsoleColor.White;
                currentTime = currentTime.AddMinutes(30);
                FraseSaid = currentTime;
            } //Обед
            if (currentTime.Hour == 22 && currentTime.Minute == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", currentTime, nearlyEndWords[rnd.Next(0, 2)]);
                Console.ForegroundColor = ConsoleColor.White;
                FraseSaid = currentTime;
            } //за час до конца
            if (currentlyInProcessing.Count > 10 && currentTime >= FraseSaid.AddMinutes(30))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", currentTime, manyOrdersWords[rnd.Next(0, 3)]);
                Console.ForegroundColor = ConsoleColor.White;
                FraseSaid = currentTime;
            } // много заказов
            if (currentTime.Hour == 10 && currentTime.Minute==0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                BlameLaties(groupPizzamakers);
                Console.ForegroundColor = ConsoleColor.White;
                
            } //отчитывание опоздавших

            
        }

        public void Notification((Order order, IEnumerator<int> process) item)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (item.order.State == "Is Waiting To Be Cooked")
            {
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
                                Console.WriteLine("Напитки в заказе! Наверное, в пустыню едет…");
                                break;
                            case 2:
                                Console.WriteLine("Заказ№{0} с напитками, не забудьте", item.order.OrderNumber);
                                break;
                            default:
                                Console.WriteLine("стоит отметить, что заказ№{1} <<{0}>> имеет напитки", item.order.Content, item.order.OrderNumber);
                                break;
                        }
                    }
                }
                else
                {    
                    switch (rnd.Next(1, 10))
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
    }
}
