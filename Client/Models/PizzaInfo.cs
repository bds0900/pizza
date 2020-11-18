using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class PizzaInfo
    {
        public List<Size> Size { get; set; }
        public List<Entities.Type> Type { get; set; }
        public List<Topping> Topping { get; set; }
    }
}
