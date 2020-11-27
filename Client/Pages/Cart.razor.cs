using Client.Services;
using Client.Store;
using Entities;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages
{
    public partial class Cart
    {
        private bool checkout { get; set; } = false;
        private void ProceedCheckOut ()
        {
            if (StateContainer.Items.Count > 0 || StateContainer.SideItems.Count > 0)
            {
                checkout = true;
            }
        }
        
        [Inject]
        StateContainer StateContainer { get; set; }
        protected async override Task OnInitializedAsync()
        {

            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private Order Result { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        public async Task CheckOut()
        {

            if (StateContainer.Items.Count > 0 || StateContainer.SideItems.Count > 0)
            {
                var send = new
                {
                    Customer = StateContainer.Customer,
                    Pizzas = StateContainer.Items.Select(o => new { o.Qty, o.TypeId, o.SizeId, Toppings = o.ToppingId, o.Subtotal }),
                    Sides = StateContainer.SideItems.Select(o => new { o.Qty, o.SideId, o.Subtotal })
                };
                Result = await pizzaService.PostCheckOutAsync(send);
                StateContainer.Orders.Add(Result);

                StateContainer.Items.Clear();
                StateContainer.SideItems.Clear();
            }
            else
            {

            }

        }


    }
}
