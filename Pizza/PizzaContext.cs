using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace Pizza
{
    public class PizzaDbContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Topping> Toppings { get; set; }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProcess> OrderProcess { get; set; }
        public DbSet<PizzaTopping> PizzaToppings { get; set; }
        public DbSet<OrderProcess> OrderProcesses { get; set; }

        public PizzaDbContext(DbContextOptions<PizzaDbContext>options):base(options)
        {
            
        }
        public PizzaDbContext() : base()
            
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()

                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Pizza>()
                .HasKey(b => b.PizzaId);

            modelBuilder.Entity<Size>()
                .HasKey(b => b.SizeId);

            modelBuilder.Entity<Type>()
                .HasKey(b => b.TypeId);
            

            modelBuilder.Entity<Pizza>()
                .Property(b => b.PizzaId)
                //.ValueGeneratedOnAdd();
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Customer>()
                .Property(b => b.CustomerId)
                //.ValueGeneratedOnAdd();
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Size>()
                .Property(b => b.SizeId)
                .ValueGeneratedOnAdd();
                //.HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Type>()
                .Property(b => b.TypeId)
                .ValueGeneratedOnAdd();
                //.HasDefaultValueSql("NEWID()");


            modelBuilder.Entity<Process>()
                .Property(b => b.ProcessId)
                .ValueGeneratedOnAdd();
                //.HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Topping>()
                .Property(b => b.ToppingId)
                .ValueGeneratedOnAdd();
                //.HasDefaultValueSql("NEWID()");

            /////////////////////////////
            /*modelBuilder.Entity<Size>()
                .HasMany(p => p.Pizzas)
                .WithOne(b=>b.Size)
                .HasForeignKey(p => p.PizzaId);

            modelBuilder.Entity<Type>()
                .HasMany(p => p.Pizzas)
                .WithOne(b=>b.Type)
                .HasForeignKey(p => p.PizzaId);*/
            ////////////////////////////////////////
            modelBuilder.Entity<Pizza>()
                .HasOne(p => p.Size)
                .WithMany(p => p.Pizzas)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(p => p.SizeId);

            modelBuilder.Entity<Pizza>()
                .HasOne(p => p.Type)
                .WithMany(p => p.Pizzas)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(p => p.TypeId);

            modelBuilder.Entity<Pizza>()
                .HasOne(p => p.Order)
                .WithMany(p => p.Pizzas)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(p => p.OrderId);



            modelBuilder.Entity<Order>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");

            // 두개의 one to many로 나눈다
            modelBuilder.Entity<Order>()
                .Property(b => b.OrderId)
                //.ValueGeneratedOnAdd();
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Order>()
                .HasKey(b => b.OrderId);

            //order에는 one customer가있고 many orders가 있다, 그리고 order table의 fk는 customerid
            modelBuilder.Entity<Order>()
                .HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(order => order.CustomerId);

            //order에는 one pizza가있고 many orders가 있다, 그리고 order table의 fk는 pizzaid
           /* modelBuilder.Entity<Order>()
                .HasOne(order => order.Pizza)
                .WithMany(pizza => pizza.Orders)
                .HasForeignKey(order => order.PizzaId);*/






            modelBuilder.Entity<OrderProcess>()
                .HasKey(pp => new { pp.OrderId, pp.ProcessId });

            modelBuilder.Entity<OrderProcess>()
                .HasOne(op => op.Order)
                .WithMany(pizza => pizza.OrderProcesses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(pp => pp.OrderId);

            modelBuilder.Entity<OrderProcess>()
                .HasOne(pp => pp.Process)
                .WithMany(process => process.OrderProcesses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(pp => pp.ProcessId);





            modelBuilder.Entity<PizzaTopping>()
                .HasKey(pt => new { pt.PizzaId, pt.ToppingId });

            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pt => pt.Pizza)
                .WithMany(pizza => pizza.PizzaToppings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(pp => pp.PizzaId);

            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pt => pt.Topping)
                .WithMany(topping => topping.PizzaToppings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(pt => pt.ToppingId);
        }

    }


    public class Pizza
    {
        public Guid PizzaId { get; set; }

        public int TypeId { get; set; }
        public Type Type { get; set; }

        public int SizeId { get; set; }
        public Size Size { get; set; }

        public List<PizzaTopping> PizzaToppings { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }


    //options
    public class Type
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }



        public List<Pizza> Pizzas { get; set; }
    }


    public class Size
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public float SizePrice { get; set; }


        public List<Pizza> Pizzas { get; set; }
    }




    public class Customer
    {
        public Guid CustomerId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        public List<Order> Orders { get; set; }


    }
    
    public class Order
    {
        public Guid OrderId { get; set; }

        public DateTime Created { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<Pizza> Pizzas { get; set; }
        public List<OrderProcess> OrderProcesses { get; set; }
    }


    public class Process
    {
        public int ProcessId { get; set; }
        public string Status { get; set; }
        public List<OrderProcess> OrderProcesses { get; set; }
    }

    public class OrderProcess
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public int ProcessId { get; set; }
        public Process Process { get; set; }
    }



    public  class Topping
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; }
        public float ToppingPrice { get; set; }

        public List<PizzaTopping> PizzaToppings { get; set; }
    }



    public class PizzaTopping
    {
        public Guid PizzaId { get; set; }
        public Pizza Pizza { get; set; }


        public int ToppingId { get; set; }
        public Topping Topping { get; set; }
    }
 


}

