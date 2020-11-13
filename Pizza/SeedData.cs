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

            context.Types.AddRange(new Type[]
            {
                new Type{TypeName="Normal Pizza"},
                new Type{TypeName="Cheese Pizza"},
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
