using System;
using System.Collections.Generic;
using durable_functions.Framework;
using PizzaCooking.Domain;
using Utilities.Deterministic;

namespace DemoConsole
{
    public static class TestPizzeria
    {
        private static (List<Order.Menu.Meal>, List<Order.Menu.Pizza>, List<Order.Menu.Drinks>)
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

        private static OrderEvent CreateSampleOrder(TimeSpan timeToDeliver, DateTime orderRecievedTime, int orderNumber,
            DeterministicRandom random)
        {
            var (meals, pizzas, drinks) = GetSampleFood(random);
            var order = new OrderEvent(timeToDeliver, orderRecievedTime, orderNumber, meals, pizzas, drinks);
            return order;
        }

        public static List<OrderEvent> GenerateSampleOrders(DateTime date, DeterministicRandom random, ILogger logger)
        {
            var orderList = new List<OrderEvent>();

            var startTime = date.Date.AddHours(9);

            var endTime = date.Date.AddHours(23);

            var currentTime = startTime;

            var ordernum = 0;
            while (currentTime < endTime)
            {
                if (random.Next(1, 101) > 60 && currentTime.Hour < 23)
                {
                    orderList.Add(CreateSampleOrder(
                        TimeSpan.FromMinutes(random.Next(15, 31)), currentTime, ordernum, random));
                    ordernum++;
                }

                currentTime = currentTime.AddMinutes(1);
            }

            return orderList;
        }
    }

    public class OrderEvent
    {
        public TimeSpan TimeToDeliver { get; }
        public DateTime OrderRecievedTime { get; }
        public int OrderNumber { get; }
        public IEnumerable<Order.Menu.Meal> Meals { get; }
        public IEnumerable<Order.Menu.Pizza> Pizzas { get; }
        public IEnumerable<Order.Menu.Drinks> Drinks { get; }

        public OrderEvent(TimeSpan timeToDeliver, DateTime orderRecievedTime, int orderNumber, List<Order.Menu.Meal> meals, List<Order.Menu.Pizza> pizzas, List<Order.Menu.Drinks> drinks)
        {
            TimeToDeliver = timeToDeliver;
            OrderRecievedTime = orderRecievedTime;
            OrderNumber = orderNumber;
            Meals = meals;
            Pizzas = pizzas;
            Drinks = drinks;
        }
    }
}