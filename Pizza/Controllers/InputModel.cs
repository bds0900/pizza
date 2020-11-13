using Pizza.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizza
{
    public class InputModel
    {
        public Customer Customer { set; get; }
        public List<PizzaInfo> Pizza { set; get; }
    }
}
