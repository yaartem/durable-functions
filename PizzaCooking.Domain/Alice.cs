using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PizzaCooking.Domain;

namespace PizzaCooking.Domain
{
    public class Alice
    {
        public List<string> helloWords = new List<string>();
        public List<string> halfWords = new List<string>();
        public List<string> finishWords = new List<string>();
        public List<string> manyOrdersWords = new List<string>();
        public List<string> nearlyEndWords = new List<string>();

        public Alice()
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
            nearlyEndWords.Add("Не спать! До конца смены всего час!");
            nearlyEndWords.Add("Час до конца! Последние заказы! Крепитесь!");
        }

        public void BlameLaties(List<Pizzamaker> guys)
        {
            foreach (var i in guys)
            {
                if (i.IsLate())
                {
                    Console.Write("Друг мой, {0}, опаздываем! В следующий раз так не обижай меня", i.Name);
                }
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


    }
}
