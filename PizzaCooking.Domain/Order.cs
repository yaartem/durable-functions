using System;
using System.Collections.Generic;
using System.Linq;
using durable_functions.Framework;

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
        private readonly ILogger _logger;

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

        public Order(TimeSpan timeToDeliver, DateTime orderRecievedTime, int orderNumber,
            List<Menu.Meal> meals, List<Menu.Pizza> pizzas, List<Menu.Drinks> drinks, ILogger logger)
        {
            TimeToDeliver = timeToDeliver;
            OrderRecievedTime = orderRecievedTime;
            OrderNumber = orderNumber;
            _meals = meals;
            _pizzas = pizzas;
            _drinks = drinks;
            _logger = logger;
        }

        public void ShowState()
        {
            _logger.Log($"Order number {OrderNumber} {State} ");
        }

    }
}