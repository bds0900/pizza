using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Diagnostics;
using PizzaMachine;
using Cheese;
using Microsoft.AspNetCore.Authorization;

namespace Pizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        DAL dal = new DAL();
        public void SimpleTask()
        {
            Thread.Sleep(10000);
            PizzaMaker pizzamaker = new PizzaMaker();
            BasicPizza.Pizza pizza = new CheesePizza("Cheese");
            pizza.AddToppings(new string[] { "Red Onions", "Hot Sausage", "Salami" });
            pizzamaker.SetPizza(pizza);
            pizzamaker.MakePizza();
            Trace.WriteLine("Done");
        }
        // GET: api/Pizza
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Pizza>> Get()
        {
            
            string[] size = new string[] { "small", "medium", "large" };
            float[] price = new float[] { 10.99f, 12.99f, 14.99f };
            List<Pizza> pizza = new List<Pizza>();
            DataTable dt=dal.GetPizzaList();
            var rows = dt.Rows;
            foreach(DataRow row in rows)
            {
                pizza.Add(new Pizza { Id = row[0].ToString(), Name = row[1].ToString(),Image= row[2].ToString(), Description = row[3].ToString() });
            }
            //await SimpleTask();
            var ttt = Task.Run(SimpleTask);
            return pizza.ToArray();
        }

        // GET: api/Pizza/5
        [HttpGet("{id}", Name = "Get")]
        public Pizza Get(int id)
        {
            var row=dal.GetPizza(1);
            return new Pizza { Id = row[0].ToString(), Name = row[1].ToString(), Image = row[2].ToString(), Description = row[3].ToString() };
        }

        // POST: api/Pizza
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Pizza/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
