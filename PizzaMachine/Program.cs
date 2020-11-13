using System;
using BasicPizza;
using Cheese;

namespace PizzaMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            PizzaMaker pizzamaker = new PizzaMaker();
            Pizza pizza = new Pizza();
            pizzamaker.AddTopping(new string[] { "cheese","beef"});
            pizzamaker.SetPizza(pizza);
            pizzamaker.MakePizza();
            
        }
    }
}
