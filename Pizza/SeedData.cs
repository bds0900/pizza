using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();

            services.AddLogging();


            services.AddDbContext<PizzaDbContext>(options =>options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)));


            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {

                    // ApplicationDbContext
                    using(var PizzaDbContext = scope.ServiceProvider.GetService<PizzaDbContext>())
                    {
                        //PizzaDbContext.Database.EnsureCreated();
                        PizzaDbContext.Database.Migrate();

                        EnsureSeedData(PizzaDbContext);
                    }
                    
                    
                }
            }
        }
        private static void EnsureSeedData(PizzaDbContext context)
        {
            
            context.Sizes.AddRange(new Size[] {
                new Size{SizeName="Small",SizePrice=4.99f},
                new Size{SizeName="Medium",SizePrice=5.99f},
                new Size{SizeName="Large",SizePrice=6.99f},
            });

            context.Types.AddRange(new Entities.Type[]
            {
                new Entities.Type{
                    TypeName="Custom",
                    Description="choose your favorite topping ",
                    Image="https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?ixlib=rb-1.2.1&auto=format&fit=crop&w=500&q=60"
                },
                new Entities.Type{
                    TypeName="Cheese",
                    Description="For cheese lover",
                    Image="https://thumbs.dreamstime.com/z/woman-taking-slice-hot-cheese-pizza-margherita-table-closeup-138142081.jpg"
                },
                new Entities.Type{
                    TypeName="Pepperoni",
                    Description="my favorite",
                    Image="https://thumbs.dreamstime.com/z/sliced-pepperoni-pizza-slice-being-lifted-board-43268133.jpg"
                },
            });

            context.Customers.AddRange(new Customer[]{
                new Customer { FirstName="Doosan",LastName="Beak"}
            });
            context.Toppings.AddRange(new Topping[]{
                new Topping{ToppingName="Cheese",ToppingPrice=2},
                new Topping{ToppingName="Jalapeno Pepper",ToppingPrice=1},
                new Topping{ToppingName="Chicken",ToppingPrice=1},
            });
            context.Processes.AddRange(new Process[]{
                new Process{Status="Preparing"},
                new Process{Status="Making"},
                new Process{Status="Ready to deliver"},
                new Process{Status="Shipped"},
                new Process{Status="Received"},
            });
            context.SaveChanges(); 
            /*
            if (!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in Configuration.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }*/

        }
    }
}
