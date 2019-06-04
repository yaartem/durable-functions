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
        }
        
    }
}
