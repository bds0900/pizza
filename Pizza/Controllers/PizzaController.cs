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
using Entities;

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
        [HttpGet("topping")]
        public IEnumerable<Topping> Topping()
        {
            return _context.Toppings.ToList();
        }
        [HttpGet("type")]
        public IEnumerable<Entities.Type> Type()
        {
            return _context.Types.ToList();
        }
        [HttpGet("size")]
        public IEnumerable<Size> Size()
        {
            return _context.Sizes.ToList();
        }
        [HttpGet("sides")]
        public IEnumerable<Side> Sides()
        {
            return _context.Sides.ToList();
        }
        // GET: api/Pizza
        [HttpGet]
        //[Authorize]
        public ActionResult Get()
        {
            return new JsonResult(new
            {
                Size = _context.Sizes.Select(o => new { o.SizeId, o.SizeName, o.SizePrice }),
                Type = _context.Types.Select(o => new { o.TypeId, o.TypeName, o.Image, o.Description }),
                Topping = _context.Toppings.Select(o => new { o.ToppingId, o.ToppingName, o.ToppingPrice })
            });
        }
        [HttpGet("track/{orderId}")]
        public IEnumerable<Entities.Process> Track(Guid orderId)
        {
            var xx = (from d in _context.OrderProcesses
                      join t in _context.Processes on d.ProcessId equals t.ProcessId
                      where d.OrderId == orderId
                      orderby t.ProcessNum
                      select t).ToList();
            return xx;
        }

        // GET: api/Pizza/Customers/5/Orders
        [HttpGet("Customers/{id}/Orders", Name = "GetOrderIds")]
        //[Authorize]
        public IEnumerable<Order> GetOrderIds(Guid id)
        {
            return _context.Orders.Where(o => o.Customer.CustomerId == id);

        }


        // GET: api/Pizza/Orders/5
        [HttpGet("Orders/{orderId}", Name = "GetOrderInfo")]
        public JsonResult GetOrderInfo(Guid orderId)
        {

            var pizzas =
                        _context.Orders
                        .Join(_context.Pizzas, o => o.OrderId, p => p.OrderId,
                            (o, p) => new { o.Created, p.OrderId, p.PizzaId, p.SizeId, p.TypeId, p.Qty })
                        .Join(_context.Types, p => p.TypeId, pt => pt.TypeId,
                            (p, pt) => new { p.Created, p.OrderId, p.PizzaId, p.SizeId, p.Qty, pt.TypeName, })
                        .Join(_context.Sizes, p => p.SizeId, t => t.SizeId,
                            (p, t) => new
                            {
                                p.OrderId,
                                p.PizzaId,
                                p.Created,
                                p.Qty,
                                type = p.TypeName,
                                size = t.SizeName,
                                price = t.SizePrice,
                                toppings = (from topping in _context.Toppings
                                            where (from pt in _context.PizzaToppings
                                                   where pt.PizzaId == p.PizzaId
                                                   select pt.ToppingId).Contains(topping.ToppingId)
                                            select new { topping.ToppingName, topping.ToppingPrice }).ToArray(),


                            })
                        .AsEnumerable()
                        .Where(o => o.OrderId == orderId)
                        .GroupBy(g => g.PizzaId);

            var sides = _context.Orders
                        .Join(_context.SideOrders, o => o.OrderId, so => so.OrderId,
                        (o, so) => new
                        {
                            o.Created,
                            so.OrderId,
                            so.Qty,
                            sides = (from side in _context.Sides
                                     where (from so in _context.SideOrders
                                            where so.OrderId == orderId
                                            select so.SideId).Contains(side.SideId)
                                     select new { side.SideName, side.SidePrice }).ToArray()
                        })
                        .AsEnumerable()
                        .Where(o => o.OrderId == orderId);
            /*.GroupBy(g => g.OrderId);*/
            var order = _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
            var pizzaitem = _context.Pizzas.Where(o => o.OrderId == orderId)
                .Select(o => new
                {
                    o.PizzaId,
                    o.SizeId,
                    o.TypeId,
                    o.Qty,
                    ToppingId =  o.PizzaToppings.Select(t => t.ToppingId)
                    
                }).ToList();
            var sideitem = _context.SideOrders.Where(o => o.OrderId == orderId).Select(s => new { s.SideId, s.Qty }).ToList();
            return new JsonResult(new
            {
                Order=order,
                Pizzas = pizzaitem,
                Sides = sideitem
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
                var sub = (float)Math.Round(value.Pizzas.Select(o => o.Subtotal).Sum() + value.Sides.Select(o => o.Subtotal).Sum(), 2);
                var tax = (float)Math.Round(sub * 0.13, 2);
                var total = (float)Math.Round(sub + tax, 2);
                newOrder.Subtotal = sub;
                newOrder.Tax = tax;
                newOrder.Total = total;

                foreach (var side in value.Sides)
                {
                    SideOrder newSideOrder = new SideOrder { Order = newOrder, SideId = side.SideId, Qty = side.Qty };
                    await _context.SideOrders.AddAsync(newSideOrder);
                }


                foreach (var pizza in value.Pizzas)
                {
                    //피자 생성
                    Entities.Pizza newPizza = new Entities.Pizza { Order = newOrder, TypeId = pizza.TypeId, SizeId = pizza.SizeId, Qty = pizza.Qty };
                    await _context.Pizzas.AddAsync(newPizza);
                    //피자에 토핑연결
                    foreach (var topping in pizza.Toppings)
                    {
                        PizzaTopping pizzaTopping = new PizzaTopping { Pizza = newPizza, ToppingId = topping };
                        await _context.PizzaToppings.AddAsync(pizzaTopping);
                    }
                }

                //진행상황
                OrderProcess newOrderProcess = new OrderProcess { Order = newOrder, ProcessId = _context.Processes.Where(p => p.ProcessNum == 1).FirstOrDefault().ProcessId };
                var ret = await _context.OrderProcess.AddAsync(newOrderProcess);
                await _context.SaveChangesAsync();

                Task.Run(() => PizzaMakingTask(newOrder.OrderId));

                await transaction.CommitAsync();
            }
            catch (DbUpdateException updateEx)
            {
                return StatusCode(500, "Internal server error");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            newOrder.Pizzas = _context.Pizzas.Where(o => o.OrderId == newOrder.OrderId).ToList();
            newOrder.SideOrders = _context.SideOrders.Where(o => o.OrderId == newOrder.OrderId).ToList();
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
