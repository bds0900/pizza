using System;
using System.Collections.Generic;
using System.Text;
using BasicPizza;
namespace PizzaMachine
{
    public class PizzaMaker
    {
        private Pizza pizza;
        public void MakePizza()
        {
            PreHeatOven();
            RollOutDough();
            BasicTopping();
            TopThePizza();

            BakeThePizza();
            SliceThePizza();
            PackThePizza();
        }

        public void SetPizza(Pizza pizza)
        {
            this.pizza = pizza;
        }

        // These operations already have implementations.
        protected void PreHeatOven()
        {
            Console.WriteLine("PizzaTemplate says: Start pre-heating oven");
        }

        protected void RollOutDough()
        {
            Console.WriteLine("PizzaTemplate says: Rolling out the dough");
        }

        protected void BasicTopping()
        {
            pizza.BasicTopping();
        }

        protected void BakeThePizza()
        {
            Console.WriteLine("PizzaTemplate says: Baking the pizza");
        }
        protected void SliceThePizza()
        {
            Console.WriteLine("PizzaTemplate says: Slicing the pizza");
        }
        protected void PackThePizza()
        {
            Console.WriteLine("PizzaTemplate says: Pack into box");
        }
        protected virtual void TopThePizza()
        {
            if (pizza.GetToppings() != null)
            {

                foreach (var topping in pizza.GetToppings())
                {
                    Console.WriteLine("Add topping :{0}", topping);
                }
            }
        }



    }
}
