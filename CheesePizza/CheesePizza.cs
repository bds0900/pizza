using System;
using BasicPizza;

namespace Cheese
{
    public class CheesePizza:Pizza
    {
        public CheesePizza(String name) : base(name) { }
        public override void BasicTopping()
        {
            //base.BasicTopping();
            Console.WriteLine("Cheese Pizza says: This is cheese base toppoing");
        }
    }
}
