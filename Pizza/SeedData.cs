using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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


            services.AddDbContext<PizzaDbContext>(options =>
                    //options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName))
                    options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName))
            );


            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {

                    // ApplicationDbContext
                    using (var PizzaDbContext = scope.ServiceProvider.GetService<PizzaDbContext>())
                    {
                        //PizzaDbContext.Database.EnsureCreated();
                        Log.Information("Context is not founded in database");
                        Log.Information("strt migration");

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
                new Size{SizeName="X-Large",SizePrice=9.99f},
            });

            context.Types.AddRange(new Entities.Type[]
            {
                new Entities.Type{
                    TypeName="Custom",
                    Description="Customized your own pizza with toppings!",
                    Image="https://cdn.pixabay.com/photo/2018/12/15/21/50/pizza-3877683_1280.jpg"
                },
                new Entities.Type{
                    TypeName="Cheese",
                    Description="It's simple, but delicious! Cheee Pizza is made with rich whole-milk Mozzarella Cheese with fresh tomato sauce made from fresh tomatoes on our special crust.",
                    Image="https://cdn.pixabay.com/photo/2020/06/17/21/46/pizza-5311269_1280.jpg"
                },
                new Entities.Type{
                    TypeName="Pepperoni",
                    Description="Traditional American styled pizza with tomato paste, pepperoni, and mozzarella cheese makes best pizza with saltiness.",
                    Image="https://cdn.pixabay.com/photo/2020/08/19/14/42/pizza-5501057_1280.jpg"
                },
            });

            context.Customers.AddRange(new Customer[]{
                new Customer { FirstName="Doosan",LastName="Beak",Email="bds@gmail.com",PhoneNumber="222-333-4444"}
            });
            context.Toppings.AddRange(new Topping[]{
                new Topping{ToppingName="Cheese",ToppingPrice=2},
                new Topping{ToppingName="Jalapeno Pepper",ToppingPrice=1},
                new Topping{ToppingName="Chicken",ToppingPrice=1},
                new Topping{ToppingName="Beef",ToppingPrice=1},
                new Topping{ToppingName="Ham",ToppingPrice=1},
                new Topping{ToppingName="Onions",ToppingPrice=1},
                new Topping{ToppingName="Feta Cheese",ToppingPrice=1},
                new Topping{ToppingName="Cheddar Cheese",ToppingPrice=2},
            });
            context.Sides.AddRange(new Side[]
            {
                new Side{
                    SideName="Potato Wedges",
                    SidePrice=3.99f,
                    Image="https://cdn.pixabay.com/photo/2015/07/13/15/29/potato-wedges-843311_1280.jpg",
                    Description=""
                },
                new Side{
                    SideName="Garlic Bread",
                    SidePrice=2.59f,
                    Image="https://cdn.pixabay.com/photo/2017/01/10/11/00/olive-oil-1968846_960_720.jpg",
                    Description=""
                },
                new Side{
                    SideName="Wings",
                    SidePrice=5.99f,
                    Image="https://cdn.pixabay.com/photo/2017/09/03/01/17/wings-2709068_1280.jpg",
                    Description=""
                },
                new Side{
                    SideName="Caesar Salad",
                    SidePrice=3.99f,
                    Image="https://cdn.pixabay.com/photo/2017/03/19/14/59/italian-salad-2156720_960_720.jpg",
                    Description=""
                },
            });
            context.Processes.AddRange(new Process[]{
                new Process{ Status="Preparing",ProcessNum=1},
                new Process{ Status="Making",ProcessNum=2},
                new Process{ Status="Ready to deliver",ProcessNum=3},
                new Process{ Status="Shipped",ProcessNum=4},
                new Process{ Status="Received",ProcessNum=5},
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
