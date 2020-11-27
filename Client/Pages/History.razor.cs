using Client.Services;
using Client.Store;
using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Pages
{
    public partial class History
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        private List<Order> Orders { get; set; }

        private DateTime? collapse { get; set; }

        private void HandleChange(DateTime created, MouseEventArgs e)
        {
            collapse = collapse==null ? created : null;
        }
        protected async override Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;
            var id_token = pizzaService.GetIdToken();
            var customerid = user.Claims.Where(s => s.Type == "sub").Select(s => s.Value).FirstOrDefault();
            Orders = await pizzaService.GetOrdersAsync(new Guid(customerid));
            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
