using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Services;
using Client.Store;
using Microsoft.AspNetCore.Components;
namespace Client.Pages
{
    public partial class Side
    {
        public List<Entities.Side> Sides { get; set; }

        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Sides = await pizzaService.GetSideAsync();

            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
