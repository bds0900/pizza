using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public DateTime Created { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public float Subtotal { get; set; }
        public float Tax { get; set; }
        public float Total { get; set; }

        public List<Pizza> Pizzas { get; set; }
        public List<SideOrder> SideOrders { get; set; }
        public List<OrderProcess> OrderProcesses { get; set; }

    }
}
