using Client.Services;
using Client.Store;
using Entities;
using IdentityModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class CheckOutComponent
    {

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private Order Result { get; set; }
        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }

        private string FirstName { get; set; } = "";
        private string LastName { get; set; } = "";
        private string Email { get; set; }
        private string PhoneNumber { get; set; }
        public async Task CheckOut()
        {
            if (StateContainer.Items.Count > 0 || StateContainer.SideItems.Count > 0)
            {
                /*var authState = await authenticationStateTask;
                if (authState.User.Identity.IsAuthenticated)
                {
                    var claims = authState.User.Claims;
                    customer = new Customer
                    {
                        CustomerId = new Guid(claims.Where(t => t.Type == JwtClaimTypes.Subject).Select(t => t.Value).FirstOrDefault()),
                        FirstName = claims.Where(t => t.Type == JwtClaimTypes.GivenName).Select(t => t.Value).FirstOrDefault(),
                        LastName = claims.Where(t => t.Type == JwtClaimTypes.FamilyName).Select(t => t.Value).FirstOrDefault(),
                        Email = claims.Where(t => t.Type == JwtClaimTypes.Email).Select(t => t.Value).FirstOrDefault(),
                        PhoneNumber = claims.Where(t => t.Type == JwtClaimTypes.PhoneNumber).Select(t => t.Value).FirstOrDefault()
                    };
                }
                else
                {

                }*/


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
        protected async override Task OnInitializedAsync()
        {

            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
