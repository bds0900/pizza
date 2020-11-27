using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SideOrder
    {
        public int SideId { get; set; }
        public Side Side { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Qty { get; set; }
    }
}
