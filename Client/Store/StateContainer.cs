using Client.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Store
{
    public class StateContainer
    {
        public string Property { get; set; } = "Initial value from StateContainer";

        public event Action OnChange;
        public void SetProperty(string value)
        {
            Property = value;
            NotifyStateChanged();
        }

        public float SubTotal { get; set; } = 0;
        public float Tax { get; set; } = 0;
        public float Total { get; set; } = 0;
        public List<Item> Items { get; set; } = new List<Item>();
        public PizzaInfo PizzaInfo { get; set; } = null;
        public List<Side> Sides { get; set; } = null;
        public void SetSubTotal(float subtotal)
        {
            SubTotal = subtotal;
            NotifyStateChanged();
        }
        public void SetTax(float tax)
        {
            Tax = tax;
            NotifyStateChanged();
        }
        public void SetTotal(float total)
        {
            Total = total;
            NotifyStateChanged();
        }
        public void SetItems(List<Item> items)
        {
            Items = items;
            NotifyStateChanged();
        }
        public void SetPizzaInfo(PizzaInfo pizzainfo)
        {
            PizzaInfo = pizzainfo;
            NotifyStateChanged();
        }
        public void SetSide(List<Side> side)
        {
            Sides = side;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
