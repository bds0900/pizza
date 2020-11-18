using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Size
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public float SizePrice { get; set; }


        public List<Pizza> Pizzas { get; set; }
    }
}
