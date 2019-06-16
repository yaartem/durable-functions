using System;
using System.Collections.Generic;
using durable_functions.Framework;
using Utilities.Deterministic;

namespace PizzaCooking.Domain
{
    public class Alice
    {
        private List<Pizzamaker> GroupPizzamakers { get; }
        public List<(Order order, IEnumerator<int> process)> CurrentlyInProcessing { get; }
        DeterministicRandom _rnd;
        private readonly ILogger _logger;
        private static List<string> HelloWords { get; }
        private static List<string> HalfWords { get; }
        private static List<string> FinishWords { get; }
        private List<string> ManyOrdersWords { get; }
        private static List<string> NearlyEndWords { get; }

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
                        Say($"Молодец-{man.Name}, {man.OrdersDone} выполнено заказов");
                    }
                    else
                    {
                        Say($"Ничего так-{man.Name}, {man.OrdersDone} выполнено заказов");
                    }
                }
            }
            yield return 0;
            while (CurrentlyInProcessing.Count > 0)
            {
                yield return 0;
            }
            Say("Все доставлено и сделано! Ждем следующего дня");
            yield return 0;
        }

        private void Say(string message)
        {
            _logger.LogColored(message, ConsoleColor.DarkRed);
        }

        public Alice(Manager boss, List<(Order order, IEnumerator<int> process)> currentlyInProcessing,
            List<Pizzamaker> groupPizzamakers,DeterministicRandom rnd, ILogger logger)
        {
            _rnd = rnd;
            _logger = logger;
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
                switch (_rnd.Next(1, 6))
                {
                    case 1:
                        Say($"{groupPizzamakers[BestPizzamaker(groupPizzamakers)].Name} выполнил больше всех заказов! Можно и перерыв на кофе");
                        FraseSaid = CurrentTime;
                        break;
                    case 2:
                        Say($"Новый король пиццы-{groupPizzamakers[BestPizzamaker(groupPizzamakers)].Name}, больше всего заказов за сегодня!");
                        FraseSaid = CurrentTime;
                        break;
                    default:
                        break;
                }
            } //Хвальба лучших

            if (currentlyInProcess.Count > 10 && CurrentTime >= FraseSaid.AddMinutes(15))
            {
                Say($"{CurrentTime} {ManyOrdersWords[_rnd.Next(0, 3)]}");
                FraseSaid = CurrentTime;
            } //Много заказов

            foreach (var item in currentlyInProcess)
            {
                if (item.order.State == "Is Waiting To Be Cooked" && !item.order.TakenToCook)
                {
                    if (item.order.PizzaCount >= 3 || item.order.AnyDrinks)
                    {
                        if (item.order.PizzaCount >= 3)
                        {
                            switch (_rnd.Next(1, 4))
                            {
                                case 1:
                                    Say($"Кто так много ест? Заказ№{item.order.OrderNumber}! Там больше трех пицц");
                                    break;
                                case 2:
                                    Say($"Какой-то праздник намечается? Тогда откуда больше 3 пицц в заказе№{item.order.OrderNumber}?");
                                    break;
                                default:
                                    Say($"Заказ№{item.order.OrderNumber} <<{item.order.Content}>>-многовато пицц для одного заказа");
                                    break;
                            }
                        }
                        else
                        {
                            switch (_rnd.Next(1, 4))
                            {
                                case 1:
                                    Say("Напитки в заказе! Наверное, в пустыню едет...");
                                    break;
                                case 2:
                                   Say($"Заказ№{item.order.OrderNumber} с напитками, не забудьте");
                                    break;
                                default:
                                    Say($"Стоит отметить, что заказ № {item.order.OrderNumber} <<{item.order.Content}>> имеет напитки");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (_rnd.Next(1, 4))
                        {
                            case 1:
                                Say($"Господа, заказ <<{item.order.Content}>> №{item.order.OrderNumber} не готов! Пора бы им заняться");
                                break;
                            case 2:
                                Say($"<<{item.order.Content}>>, заказ №{item.order.OrderNumber} и пиццамейкер! Надо бы сделать");
                                break;
                            case 3:
                                Say($"<<{item.order.Content}>>, заказ №{item.order.OrderNumber}. За работу, дорогие!");
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
                            Say($"Травка зеленеет, солнышко блестит, заказ№{item.order.OrderNumber} к курьерам летит!");
                            break;
                        case 2:
                            Say($"<<{item.order.Content}>> выполнен! Доставщики, ваш выход");
                            break;
                        default:
                            break;
                    }
                };
            }
        }
    }
}
