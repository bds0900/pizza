using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
//using BasicPizza;
namespace Pizza
{
    enum DoorStatus
    {
        LOCKED,
        UNLOCKED
    }
    public class PizzaMaker
    {
        private Entities.Pizza pizza;
        private List<Topping> toppings;
        private PizzaDbContext _context;
        private DoorStatus doorStatus;
        public PizzaMaker(PizzaDbContext context)
        {
            _context = context;
        }
        public PizzaMaker()
        {
            _context = new PizzaDbContext();
        }
        public void Run(Guid orderId)
        {
            _context.OrderProcesses.AddAsync(new OrderProcess { OrderId = orderId, ProcessId = 2 });
            _context.SaveChangesAsync();

            //making
            Trace.WriteLine("Making");
            MakePizza();

            _context.OrderProcesses.AddAsync(new OrderProcess { OrderId = orderId, ProcessId = 3 });
            _context.SaveChangesAsync();
            //ready to deliver
            Trace.WriteLine("Ready to deliver");
            Thread.Sleep(6000);

            _context.OrderProcesses.AddAsync(new OrderProcess { OrderId = orderId, ProcessId = 4 });
            _context.SaveChangesAsync();
            //shipped
            Trace.WriteLine("Shipped");

        }
        public void MakePizza()
        {
            RollOutDough();
            BasicTopping();
            TopThePizza();

            BakeThePizza();
            SliceThePizza();
            PackThePizza();

        }

        public void SetPizza(Entities.Pizza pizza)
        {
            this.pizza = pizza;
        }
        public void AddTopping(string[] toppings)
        {

        }


        protected void RollOutDough()
        {
            Thread.Sleep(6000);
            Console.WriteLine("PizzaTemplate says: Rolling out the dough");
        }

        protected void BasicTopping()
        {
            //Add basic Toppings
            Thread.Sleep(1000);
            Console.WriteLine("PizzaTemplate says: Add Tomato");
            Console.WriteLine("PizzaTemplate says: Add Cheese");
        }

        protected void BakeThePizza()
        {
            Thread.Sleep(12000);
            Console.WriteLine("PizzaTemplate says: Baking the pizza");
        }
        protected void SliceThePizza()
        {
            Thread.Sleep(1000);
            Console.WriteLine("PizzaTemplate says: Slicing the pizza");
        }
        protected void PackThePizza()
        {
            Thread.Sleep(1000);
            Console.WriteLine("PizzaTemplate says: Pack into box");
        }
        protected virtual void TopThePizza()
        {
            Thread.Sleep(10000);
            //additional toppings
            Console.WriteLine("Add topping ");
        }




    }
}
