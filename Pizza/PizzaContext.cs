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
using Entities;
namespace Pizza
{
    public class PizzaDbContext : DbContext
    {
        public DbSet<Entities.Pizza> Pizzas { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Entities.Type> Types { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Side> Sides { get; set; }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProcess> OrderProcess { get; set; }
        public DbSet<PizzaTopping> PizzaToppings { get; set; }
        public DbSet<OrderProcess> OrderProcesses { get; set; }
        public DbSet<SideOrder> SideOrders { get; set; }

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
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")=="Developement")
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            }
            else
            {
                optionsBuilder.UseNpgsql(ConnectionUri.Convert(Environment.GetEnvironmentVariable("DATABASE_URL")));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Entities.Pizza>()
                .HasKey(b => b.PizzaId);

            modelBuilder.Entity<Size>()
                .HasKey(b => b.SizeId);

            modelBuilder.Entity<Entities.Type>()
                .HasKey(b => b.TypeId);

            modelBuilder.Entity<Side>()
                .HasKey(b => b.SideId);


            modelBuilder.Entity<Entities.Pizza>()
                .Property(b => b.PizzaId)
                //.ValueGeneratedOnAdd();
                .HasDefaultValueSql("uuid_generate_v1()");

            modelBuilder.Entity<Customer>()
                .Property(b => b.CustomerId)
                //.ValueGeneratedOnAdd();
                .HasDefaultValueSql("uuid_generate_v1()");

            modelBuilder.Entity<Size>()
                .Property(b => b.SizeId)
                .ValueGeneratedOnAdd();
                //.HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Entities.Type>()
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

            modelBuilder.Entity<Side>()
                .Property(b => b.SideId)
                .ValueGeneratedOnAdd();

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
            modelBuilder.Entity<Entities.Pizza>()
                .HasOne(p => p.Size)
                .WithMany(p => p.Pizzas)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(p => p.SizeId);

            modelBuilder.Entity<Entities.Pizza>()
                .HasOne(p => p.Type)
                .WithMany(p => p.Pizzas)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(p => p.TypeId);

            modelBuilder.Entity<Entities.Pizza>()
                .HasOne(p => p.Order)
                .WithMany(p => p.Pizzas)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(p => p.OrderId);



            modelBuilder.Entity<Order>()
                .Property(b => b.Created)
                .HasDefaultValueSql("now()");

            // 두개의 one to many로 나눈다
            modelBuilder.Entity<Order>()
                .Property(b => b.OrderId)
                //.ValueGeneratedOnAdd();
                .HasDefaultValueSql("uuid_generate_v1()");

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



            modelBuilder.Entity<SideOrder>()
                .HasKey(so=>new { so.OrderId,so.SideId});

            modelBuilder.Entity<SideOrder>()
                .HasOne(so => so.Order)
                .WithMany(o => o.SideOrders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(so => so.OrderId);

            modelBuilder.Entity<SideOrder>()
                .HasOne(so => so.Side)
                .WithMany(o => o.SideOrders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(so => so.SideId);

        }

    }


     


}

