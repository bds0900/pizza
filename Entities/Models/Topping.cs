using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Topping
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; }
        public float ToppingPrice { get; set; }

        public List<PizzaTopping> PizzaToppings { get; set; }
    }
}
