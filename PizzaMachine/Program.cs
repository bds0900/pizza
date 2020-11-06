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
            Pizza pizza = new CheesePizza("Cheese");
            pizza.AddToppings(new string[]{ "Red Onions", "Hot Sausage", "Salami" });
            pizzamaker.SetPizza(pizza);
            pizzamaker.MakePizza();

            Pizza pizza1 = new DefaultPizza("Default");
            pizzamaker.SetPizza(pizza1);
            pizzamaker.MakePizza();





        }
    }
}
