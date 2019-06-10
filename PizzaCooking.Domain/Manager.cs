using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaCooking.Domain
{
    public class Manager
    {
        public string Name { get; }

        public Manager(string name)
        {
            Name = name;
        }
    }
}
