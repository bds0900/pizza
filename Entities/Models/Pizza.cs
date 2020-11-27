using System;
using System.Collections.Generic;

namespace Entities
{
    public class Pizza
    {
        public Guid PizzaId { get; set; }

        public int TypeId { get; set; }
        public Type Type { get; set; }

        public int SizeId { get; set; }
        public Size Size { get; set; }
        public int Qty { get; set; }

        public List<PizzaTopping> PizzaToppings { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
