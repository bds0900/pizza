using System;

namespace BasicPizza
{
    public abstract class Pizza
    {
        private String name;
        private string[] toppings;

        public Pizza(string name)
        {
            this.name = name;
        }
        public abstract void BasicTopping();

        public void AddToppings(string[] toppings)
        {
            this.toppings = toppings;
        }
        public string[] GetToppings()
        {
            return toppings;
        }
    }


    public class DefaultPizza : Pizza
    {
        public DefaultPizza(string name) : base(name) { }
        public override void BasicTopping()
        {
            Console.WriteLine("Basic Pizza says: This is basic topping");
        }
    }

    public static class Toppings
    {
        public static String Ham { get { return "Ham"; } }
        public static String Broccoli { get { return "Broccoli"; } }
        public static String Tomato { get { return "Tomato"; } }
    }
}
