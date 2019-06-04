﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace PizzaCooking.Domain
{
    public class Order
    {
        Random rnd=new Random();
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

        public Order(TimeSpan timeToDeliver, TimeSpan timeToCook, DateTime orderRecievedTime, int orderNumber)
        {
            TimeToDeliver = timeToDeliver;
            TimeToCook = timeToCook;
            OrderRecievedTime = orderRecievedTime;
            OrderNumber = orderNumber;
            FillwithFood();
        }

        public TimeSpan TimeToDeliver{get;}
        public TimeSpan TimeToCook { get; }
        public DateTime OrderRecievedTime { get; }
        public DateTime OrderTakenForCookingTime { set; get; }
        public int OrderNumber { get; }
        public DateTime CurrentTime { get; set; }
        public string State { get; set; }
        public DateTime TimeTaken {get; set;}
        public string Content { set; get; }
        public int Pizzas { set; get; }

        public void ShowState()
        {
            Console.WriteLine("Order number {0} {1} ", OrderNumber, State);
        }

        public void FillwithFood()
        {
            Content = ((Menu.Pizza)rnd.Next(0, 8)).ToString();
            while (rnd.Next(0,3) == 0)
            {
                switch (rnd.Next(1, 8))
                {
                    case 1: Content = Content + ", " + ((Menu.Pizza)rnd.Next(0,8)).ToString();
                        Pizzas++;
                        break;
                    case 2: Content = Content + ", "  + ((Menu.Meal)rnd.Next(0, 4)).ToString();
                        break;
                    case 3: Content = Content + ", " + ((Menu.Drinks)rnd.Next(0, 6)).ToString();
                        break;
                    default: break;
                }
            }
        }
    }
}