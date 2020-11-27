using Client.Models;
using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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

        public List<PizzaItem> Items { get; set; } = new List<PizzaItem>();
        public List<SideItem> SideItems { get; set; } = new List<SideItem>();
        public PizzaInfo PizzaInfo { get; set; } = null;
        public List<Side> Sides { get; set; } = null;
        public Customer Customer { get; set; } = new Customer { CustomerId = Guid.NewGuid() };
        public List<Order> Orders { get; set; } = new List<Order>();
        public void SetCustomer(Customer customer)
        {
            Customer = customer;
            NotifyStateChanged();
        }
        public void SetItems(List<PizzaItem> items)
        {
            Items = items;
            NotifyStateChanged();
        }
        public void SetSideItems(List<SideItem> items)
        {
            SideItems = items;
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
