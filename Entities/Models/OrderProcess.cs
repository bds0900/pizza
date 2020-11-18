using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrderProcess
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public int ProcessId { get; set; }
        public Process Process { get; set; }
    }
}
