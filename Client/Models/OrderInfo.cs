using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class OrderInfo
    {
        public Order Order { get; set; }
        public List<PizzaItem> Pizzas { get; set; }
        public List<SideItem> Sides { get; set; }
    }
}
