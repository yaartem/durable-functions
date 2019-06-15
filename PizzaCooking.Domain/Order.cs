using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Deterministic;

namespace PizzaCooking.Domain
{
    public class Order
    {
        public TimeSpan TimeToDeliver{get;}
        public TimeSpan TimeToCook => TimeSpan.FromMinutes(_pizzas.Count*5);
        public DateTime OrderRecievedTime { get; }
        public DateTime OrderTakenForCookingTime { set; get; }
        public int OrderNumber { get; }
        public DateTime CurrentTime { get; set; }
        public string State { get; set; }
        public DateTime TimeTaken {get; set;}
        
        private readonly List<Menu.Pizza> _pizzas;

        private readonly List<Menu.Meal> _meals;

        private readonly List<Menu.Drinks> _drinks;
        
        public bool AnyDrinks => _drinks.Count > 0;
        
        public int PizzaCount => _pizzas.Count;

        private string _content;

        public string Content => _content ?? (_content =
            string.Join(",",
                _pizzas.Select(p => p.ToString())
                    .Concat(_meals.Select(m => m.ToString()))
                    .Concat(_drinks.Select(d => d.ToString()))
            ));
        
        public struct Menu
        {
            public enum Pizza
            {
                Додо, Пепперони, Мясная, Мексиканская,
                Маргарита, Гавайская, Сырная, Итальянская
            }

            public enum Meal
            {
                Додстер, Салат, Крылья, Кукуруза
            }

            public enum Drinks
            {
                Кола, Спрайт, Фанта, Фьюзти, Сок, Вода
            }
        }

        public bool TakenToDeliver=false;
        public bool TakenToCook = false;

        public IEnumerable<int> GetSequence()
        {
            while (OrderRecievedTime != OrderTakenForCookingTime && !TakenToCook)
            {
                State = "Is Waiting To Be Cooked";
                ShowState();
                yield return 0;
            }
            while (CurrentTime < OrderTakenForCookingTime + TimeToCook)
            {
                State = "Is Cooking";
                ShowState();
                yield return 0;
            }

            while (CurrentTime >= OrderTakenForCookingTime + TimeToCook && !TakenToDeliver)
            {
                State = "Is Ready To Be Taken";
                ShowState();
                yield return 0;
            }
            while (CurrentTime < TimeTaken+TimeToDeliver)
            {
                State = "Is Delivering";
                ShowState();
                yield return 0;
            }
        }

        protected Order(TimeSpan timeToDeliver, DateTime orderRecievedTime, int orderNumber,
            List<Menu.Meal> meals, List<Menu.Pizza> pizzas, List<Menu.Drinks> drinks)
        {
            TimeToDeliver = timeToDeliver;
            OrderRecievedTime = orderRecievedTime;
            OrderNumber = orderNumber;
            _meals = meals;
            _pizzas = pizzas;
            _drinks = drinks;
        }

        public void ShowState()
        {
            Console.WriteLine("Order number {0} {1} ", OrderNumber, State);
        }

        public static (List<Menu.Meal>, List<Menu.Pizza>, List<Menu.Drinks>)
            GetSampleFood(DeterministicRandom random)
        {
            var pizzas = new List<Menu.Pizza>();
            var drinks = new List<Menu.Drinks>();
            var meals = new List<Menu.Meal>();
            pizzas.Add((Menu.Pizza) random.Next(0, 8));
            while (random.Next(0,3) == 0)
            {
                switch (random.Next(0, 3))
                {
                    case 0: pizzas.Add((Menu.Pizza)random.Next(0,8));
                        break;
                    case 1: meals.Add((Menu.Meal)random.Next(0, 4));
                        break;
                    case 2: drinks.Add((Menu.Drinks)random.Next(0, 6));
                        break;
                }
            }

            return (meals, pizzas, drinks);
        }

        public static Order CreateSample(TimeSpan timeToDeliver, DateTime orderRecievedTime, int orderNumber, DeterministicRandom random)
        {
            var (meals, pizzas, drinks) = GetSampleFood(random);
            var order = new Order(timeToDeliver, orderRecievedTime, orderNumber, meals, pizzas, drinks);
            return order;
        }
    }
}