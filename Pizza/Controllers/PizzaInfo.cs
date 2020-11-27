using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza.Controllers
{
    public class PizzaInfo
    {
        public int Qty { get; set; }
        public int TypeId { get; set; }
        public int SizeId { get; set; }
        public int[] Toppings { get; set; }
        public float Subtotal { get; set; }

    }
    public class SideInfo
    {
        public int Qty { get; set; }
        public int SideId { get; set; }
        public float Subtotal { get; set; }
    }

}
