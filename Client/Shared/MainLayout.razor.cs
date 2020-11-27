using Client.Services;
using Client.Store;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using IdentityModel;

namespace Client.Shared
{
    public partial class MainLayout
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            var authstate=await authenticationStateTask;
            if(authstate.User.Identity.IsAuthenticated)
            {
                var claims = authstate.User.Claims;
                StateContainer.Customer = new Customer
                {
                    CustomerId = new Guid(claims.Where(t => t.Type == JwtClaimTypes.Subject).Select(t => t.Value).FirstOrDefault()),
                    FirstName = claims.Where(t => t.Type == JwtClaimTypes.GivenName).Select(t => t.Value).FirstOrDefault(),
                    LastName = claims.Where(t => t.Type == JwtClaimTypes.FamilyName).Select(t => t.Value).FirstOrDefault(),
                    Email = claims.Where(t => t.Type == JwtClaimTypes.Email).Select(t => t.Value).FirstOrDefault(),
                    PhoneNumber = claims.Where(t => t.Type == JwtClaimTypes.PhoneNumber).Select(t => t.Value).FirstOrDefault()
                };
            }
            var pizzaInfo = await pizzaService.GetPizzaInfoAsync();
            StateContainer.SetPizzaInfo(pizzaInfo);
            var sideInfo = await pizzaService.GetSideAsync();
            StateContainer.SetSide(sideInfo);

        }
    }
}
