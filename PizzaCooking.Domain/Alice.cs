using System;
using System.Collections.Generic;

namespace PizzaCooking.Domain
{
    public class Alice
    {
        private List<Pizzamaker> GroupPizzamakers { get; }
        public List<(Order order, IEnumerator<int> process)> CurrentlyInProcessing { get; }
        Random _rnd = new Random(); // отключить
        private static List<string> HelloWords { get; }
        private static List<string> HalfWords { get; }
        private static List<string> FinishWords { get; }
        private List<string> ManyOrdersWords { get; }
        private static  List<string> NearlyEndWords { get; }

        public DateTime CurrentTime { get; set; }
        public DateTime FraseSaid { get; set; }
        
        public IEnumerable<int> GetSequence()
        {
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(9))
            {
                yield return 0;
            }
                Say($"{CurrentTime} {HelloWords[_rnd.Next(0, 4)]}");
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(10))
            {
                PhrasesAll(CurrentlyInProcessing,GroupPizzamakers);
                yield return 0;
            }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                BlameLaties(GroupPizzamakers);
                Console.ForegroundColor = ConsoleColor.White;
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(13))
            {
                PhrasesAll(CurrentlyInProcessing, GroupPizzamakers);
                yield return 0;
            }
                
                Say(string.Format("{0} {1}", CurrentTime, HalfWords[_rnd.Next(0, 2)]));
                
                CurrentTime = CurrentTime.AddMinutes(30);
                FraseSaid = CurrentTime;
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(22))
            {
                PhrasesAll(CurrentlyInProcessing, GroupPizzamakers);
                yield return 0;
            }
                
                Say(string.Format("{0} {1}", CurrentTime, NearlyEndWords[_rnd.Next(0, 2)]));
                FraseSaid = CurrentTime;
                yield return 0;
            while (CurrentTime.TimeOfDay < TimeSpan.FromHours(23))
            {
                PhrasesAll(CurrentlyInProcessing, GroupPizzamakers);
                yield return 0;
            }
                
                Say(FinishWords[_rnd.Next(0, 2)]);
                Say(string.Format("Есть люди как люди, а есть {0}! {1}!Слишком много заказов!",
                    GroupPizzamakers[BestPizzamaker(GroupPizzamakers)].Name, GroupPizzamakers[BestPizzamaker(GroupPizzamakers)].OrdersDone));
                
                foreach (var man in GroupPizzamakers)
                {
                    if (man.OrdersDone > 20)
                    {
                        Say($"Большой молодец-{man.Name}, {man.OrdersDone} выполнено заказов");
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
                while (CurrentlyInProcessing.Count>0)
                {
                    yield return 0;
                }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Все доставлено и сделано! Ждем следующего дня");
                Console.ForegroundColor = ConsoleColor.White;
                yield return 0;
        }

        private void Say(string what)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine(what);
            Console.WriteLine();
            Console.ForegroundColor = prevColor;
        }

        public Alice(Manager boss, List<(Order order, IEnumerator<int> process)> currentlyInProcessing,
            List<Pizzamaker> groupPizzamakers)
        {
            GroupPizzamakers = groupPizzamakers;
            CurrentlyInProcessing = currentlyInProcessing;

            ManyOrdersWords = new List<string>
            {
                "Поторопитесь, в очереди много заказов!",
                "Стало многовато заказов! Пора поднапрячься, любимые",
                "Ребята, помогите! " + boss.Name + " пытается закрыть смену. Заказов многовато стало"
            };
        }

        static Alice()
        {
            HelloWords = new List<string>
            {
                "Здравствуйте, дорогие, какой чудесный день, чтобы побить рекорды!",
                "Привет, ребята, новый день-новые заказы!",
                "Всем привет, готовы работать? Лично я всегда готова!",
                "9:00, работа началась! Всем хорошего дня!"
            };

            HalfWords = new List<string>
            {
                "13:00, пора подкрепиться, ребята",
                "Победа, победа, вместо обеда! Пойдем кушать, друзья"
            };

            FinishWords = new List<string>
            {
                "Конец Смены! Хорошая работа, ребята!",
                "Вот и пришел конец нашей работе, можно по домам!"
            };

            NearlyEndWords = new List<string>
            {
                "Не спать! До конца смены всего час!",
                "Час до конца! Последние заказы! Крепитесь!"
            };
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

        public void PhrasesAll(List<(Order order, IEnumerator<int> process)> currentlyInProcess, List<Pizzamaker> groupPizzamakers)
        {
            if (CurrentTime >= FraseSaid.AddMinutes(60))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                switch (_rnd.Next(1, 6))
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
            } //Хвальба лучших

            if (currentlyInProcess.Count > 10 && CurrentTime >= FraseSaid.AddMinutes(15))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("{0} {1}", CurrentTime, ManyOrdersWords[_rnd.Next(0, 3)]);
                Console.ForegroundColor = ConsoleColor.White;
                FraseSaid = CurrentTime;
            } //Много заказов

            foreach (var item in currentlyInProcess)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if (item.order.State == "Is Waiting To Be Cooked" && !item.order.TakenToCook)
                {
                    if (item.order.Pizzas >= 3 || item.order.AnyDrinks)
                    {
                        if (item.order.Pizzas >= 3)
                        {
                            switch (_rnd.Next(1, 4))
                            {
                                case 1:
                                    Console.WriteLine("Кто так много ест? Заказ№{0}! Там больше трех пицц",
                                        item.order.OrderNumber);
                                    break;
                                case 2:
                                    Console.WriteLine(
                                        "Какой-то праздник намечается? Тогда откуда больше 3 пицц в заказе№{0}?",
                                        item.order.OrderNumber);
                                    break;
                                default:
                                    Console.WriteLine("Заказ№{1} <<{0}>>-многовато пицц для одного заказа",
                                        item.order.Content, item.order.OrderNumber);
                                    break;
                            }
                        }
                        else
                        {
                            switch (_rnd.Next(1, 4))
                            {
                                case 1:
                                    Console.WriteLine("Напитки в заказе! Наверное, в пустыню едет...");
                                    break;
                                case 2:
                                    Console.WriteLine("Заказ№{0} с напитками, не забудьте", item.order.OrderNumber);
                                    break;
                                default:
                                    Console.WriteLine("Стоит отметить, что заказ№{1} <<{0}>> имеет напитки",
                                        item.order.Content, item.order.OrderNumber);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (_rnd.Next(1, 4))
                        {
                            case 1:
                                Console.WriteLine("Господа, заказ <<{0}>> №{1} не готов! Пора бы им заняться",
                                    item.order.Content, item.order.OrderNumber);
                                break;
                            case 2:
                                Console.WriteLine("<<{0}>>, заказ №{1} и пиццамейкер! Надо бы сделать",
                                    item.order.Content, item.order.OrderNumber);
                                break;
                            case 3:
                                Console.WriteLine("<<{0}>>, заказ №{1}. За работу, дорогие!", item.order.Content,
                                    item.order.OrderNumber);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (item.order.State == "Is Ready To Be Taken" && !item.order.TakenToDeliver)
                {
                    switch (_rnd.Next(1, 3))
                    {
                        case 1:
                            Console.WriteLine("Травка зеленеет, солнышко блестит, заказ№{0} к курьерам летит!",
                                item.order.OrderNumber);
                            break;
                        case 2:
                            Console.WriteLine("<<{0}>> выполнен! Доставщики, ваш выход", item.order.Content);
                            break;
                        default:
                            break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        

    }
}
