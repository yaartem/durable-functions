using System;
using System.Collections.Generic;
using PizzaCooking.Domain;
using Utilities.Deterministic;

namespace DemoConsole
{
    public static class TestPizzeria
    {
        
        public static (List<Order.Menu.Meal>, List<Order.Menu.Pizza>, List<Order.Menu.Drinks>)
            GetSampleFood(DeterministicRandom random)
        {
            var pizzas = new List<Order.Menu.Pizza>();
            var drinks = new List<Order.Menu.Drinks>();
            var meals = new List<Order.Menu.Meal>();
            pizzas.Add((Order.Menu.Pizza) random.Next(0, 8));
            while (random.Next(0,3) == 0)
            {
                switch (random.Next(0, 3))
                {
                    case 0: pizzas.Add((Order.Menu.Pizza)random.Next(0,8));
                        break;
                    case 1: meals.Add((Order.Menu.Meal)random.Next(0, 4));
                        break;
                    case 2: drinks.Add((Order.Menu.Drinks)random.Next(0, 6));
                        break;
                }
            }

            return (meals, pizzas, drinks);
        }

        public static Order CreateSampleOrder(TimeSpan timeToDeliver, DateTime orderRecievedTime, int orderNumber, DeterministicRandom random)
        {
            var (meals, pizzas, drinks) = GetSampleFood(random);
            var order = new Order(timeToDeliver, orderRecievedTime, orderNumber, meals, pizzas, drinks);
            return order;
        }
    }
}