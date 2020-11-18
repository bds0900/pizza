using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Process
    {
        public int ProcessId { get; set; }
        public string Status { get; set; }
        public List<OrderProcess> OrderProcesses { get; set; }
    }
}
