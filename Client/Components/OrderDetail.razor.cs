using Client.Models;
using Client.Services;
using Client.Store;
using Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class OrderDetail
    {
        [Parameter]
        public Guid OrderId { get; set; }
        private OrderInfo OrderInfo{get;set;}

        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            OrderInfo = await pizzaService.GetOrderAsync(OrderId);
            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
