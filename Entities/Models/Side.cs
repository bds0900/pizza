using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Side
    {
        public int SideId { get; set; }
        public string SideName { get; set; }
        public float SidePrice { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public List<SideOrder> SideOrders { get; set; }
    }
}
