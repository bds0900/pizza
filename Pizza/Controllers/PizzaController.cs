using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Pizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {

        PizzaDbContext _context;
        PizzaMaker _pizzaMaker;
        public void PizzaMakingTask(Guid orderId)
        {
            _pizzaMaker.Run(orderId);
            Trace.WriteLine("Done");
        }
        public PizzaController(PizzaDbContext context)
        {
            _context = context;
            _pizzaMaker = new PizzaMaker();
        }
        // GET: api/Pizza
        [HttpGet]
        //[Authorize]
        public ActionResult Get()
        {
            return new JsonResult(new
            {
                Size = _context.Sizes.Select(o=>new { o.SizeId,o.SizeName,o.SizePrice}),
                Type = _context.Types.Select(o=>new { o.TypeId,o.TypeName}),
                Topping = _context.Toppings.Select(o => new{ o.ToppingId,o.ToppingName,o.ToppingPrice})
            });
        }

        // GET: api/Pizza/Customers/5/Orders
        [HttpGet("Customers/{id}/Orders", Name = "GetOrderIds")]
        [Authorize]
        public JsonResult GetOrderIds(Guid id)
        {
            var pizzaquery = _context.Orders.Where(o => o.Customer.CustomerId == id).Select(o => o.OrderId);
            return new JsonResult(new
            {
                CustomerId = id,
                OrderIds = pizzaquery
            });
        }

        // GET: api/Pizza/Orders/5
        [HttpGet("Orders/{id}", Name = "GetOrderInfo")]
        public JsonResult GetOrderInfo(Guid id)
        {
            /*var ret1 = from p in _context.Pizzas
                       where p.OrderId == id
                       select new
                       {
                           p.PizzaId,
                           tt = (from topping in _context.Toppings
                                 where (from pt in _context.PizzaToppings
                                        where pt.PizzaId == p.PizzaId
                                        select pt.ToppingId).Contains(topping.ToppingId)
                                 select new { topping.ToppingName, topping.ToppingPrice }).ToArray()
                       };*/

            var pizzaquery = 
                
                        _context.Orders
                        .Join(_context.Pizzas,o=>o.OrderId,p=>p.OrderId,
                            (o, p) => new{
                                o.Created,
                                p.OrderId,
                                p.PizzaId,
                                p.SizeId,
                                p.TypeId})
                        .Join(_context.Types,p => p.TypeId,pt => pt.TypeId,
                            (p, pt) => new{
                                p.Created,
                                p.OrderId,
                                p.PizzaId,
                                p.SizeId,
                                pt.TypeName})
                        .Join(_context.Sizes,p => p.SizeId,t => t.SizeId,
                            (p, t) => new{
                                p.OrderId,
                                p.PizzaId,
                                p.Created,
                                type = p.TypeName,
                                size = t.SizeName,
                                price = t.SizePrice,
                                /*top= (from pt in _context.PizzaToppings
                                      where pt.PizzaId == p.PizzaId
                                      select pt.ToppingId).ToList()*/
                                toppings = (from topping in _context.Toppings
                                            where (from pt in _context.PizzaToppings
                                                   where pt.PizzaId == p.PizzaId
                                                   select pt.ToppingId).Contains(topping.ToppingId)
                                            select new { topping.ToppingName, topping.ToppingPrice }).ToArray(),})
                        .AsEnumerable()
                        .Where(o => o.OrderId == id)
                        .GroupBy(g => g.PizzaId);
            return new JsonResult(new
            {
                OrderId = id,
                Pizzas = pizzaquery
            });
        }

        // POST: api/Pizza
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InputModel value)
        {
            var customer = _context.Customers.Find(value.Customer.CustomerId);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            Order newOrder = null;
            try
            {
               
                if (customer == null)
                {
                    newOrder = new Order { Customer = value.Customer };
                }
                else
                {
                    newOrder = new Order { Customer = customer };
                }

                foreach (var pizza in value.Pizza)
                {
                    //피자 생성
                    Pizza newPizza = new Pizza { Order = newOrder, TypeId = pizza.TypeId, SizeId = pizza.SizeId };
                    //피자에 토핑연결
                    foreach (var topping in pizza.Toppings)
                    {
                        PizzaTopping pizzaTopping = new PizzaTopping { Pizza = newPizza, ToppingId = topping };
                        await _context.PizzaToppings.AddAsync(pizzaTopping);
                    }
                }

                //진행상황
                OrderProcess newOrderProcess = new OrderProcess { Order = newOrder, ProcessId = 1 };
                var ret = await _context.OrderProcess.AddAsync(newOrderProcess);
                await _context.SaveChangesAsync();

                Task.Run(() => PizzaMakingTask(newOrder.OrderId));

                await transaction.CommitAsync();
            }
            catch(DbUpdateException updateEx)
            {
                return StatusCode(500, "Internal server error");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

            return new JsonResult(newOrder);
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


        public string[] SearchToppingsByPizzaId(Guid pizzaId)
        {
            /*subqueries in the Where clause
             * 
             * Select toppingName
                From Toppings
                Where Toppings.ToppingId In (Select ToppingId
                From PizzaToppings
                Where pizzaId = 1)
                go
             */
            var ret = (from topping in _context.Toppings
                       where (from pt in _context.PizzaToppings
                              where pt.PizzaId == pizzaId
                              select pt.ToppingId).Contains(topping.ToppingId)
                       select topping.ToppingName).ToArray();
            return ret;
        }
        public Dictionary<Guid, string[]> PizzaToppings(Guid orderId)
        {
            Dictionary<Guid, string[]> toppings = new Dictionary<Guid, string[]>();
            var pizzaIds = (from p in _context.Pizzas
                            where p.OrderId == orderId
                            select p.PizzaId).ToList();
            foreach (var id in pizzaIds)
            {
                toppings.Add(id, SearchToppingsByPizzaId(id));
            }
            return toppings;
        }
    }
}
