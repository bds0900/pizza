using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public int TypeId { get; set; }
        public int SizeId { get; set; }
        public int[] ToppingId { get; set; }
        public float Subtotal { get; set; }
        public float Tax { get; set; }
        public float Total { get; set; }
    }
}
