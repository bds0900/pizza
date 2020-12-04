using Client.Models;
using Client.Services;
using Client.Store;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Client.Pages
{
    public partial class Menu
    {
        private PizzaInfo PizzaInfo { get; set; }
        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            PizzaInfo = await pizzaService.GetPizzaInfoAsync();
            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
