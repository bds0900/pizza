using Client.Models;
using Client.Services;
using Client.Store;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Pages
{
    public partial class PizzaDetail
    {
        [Inject] 
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        [Parameter]
        public string PizzaName { get; set; }
        public PizzaInfo PizzaInfo { get; set; }
        public int Qty { get; set; }
        private float TotalPrice { get; set; }
        private float TaxPrice { get; set; }
        private float SubtotalPrice { get; set; }

        private List<int> SelectedTopping { get; set; } = new List<int>();

        private void handleClick(Entities.Topping item)
        {
            if (SelectedTopping.Contains(item.ToppingId))
            {
                SelectedTopping.Remove(item.ToppingId);
            }
            else
            {
                SelectedTopping.Add(item.ToppingId);
            }
            UpdatePrice();
        }

        private Entities.Size SelectedSize { get; set; }
        private void SelectionChanged(Entities.Size size)
        {
            SelectedSize = size;
            UpdatePrice();
        }


        private void UpdatePrice()
        {
            SubtotalPrice = (float)Math.Round(SelectedSize.SizePrice * Qty + SelectedTopping.Aggregate(
                0f, (total, next) => total + PizzaInfo.Topping.Where(d => d.ToppingId == next).FirstOrDefault().ToppingPrice
            ) * Qty, 2);
            TaxPrice = (float)Math.Round(SubtotalPrice * 0.13, 2);
            TotalPrice = (float)Math.Round(SubtotalPrice + TaxPrice, 2);
        }
        private void AddQty(int num)
        {
            Qty += num;
            if (Qty < 1) Qty = 1;

            UpdatePrice();
        }

        private void ChangQty(int num)
        {
            Qty = num;
            if (Qty < 1) Qty = 1;

            UpdatePrice();
        }



        private async Task AddItem()
        {
            UpdatePrice();
            var items = StateContainer.Items;
            PizzaItem item = new PizzaItem
            {
                ItemId = new Guid(),
                Qty = Qty,
                SizeId = SelectedSize.SizeId,
                TypeId = PizzaInfo.Type.Where(o => o.TypeName == PizzaName).FirstOrDefault().TypeId,
                ToppingId = SelectedTopping.ToArray(),
                Subtotal = SubtotalPrice,
                Tax = TaxPrice,
                Total = TotalPrice
            };
            items.Add(item);

            StateContainer.SetItems(items);

        }



        protected async override Task OnInitializedAsync()
        {
        /*var result = await ProtectedSessionStore.GetAsync<PizzaInfo>("PizzaInfo");
            PizzaInfo = result.Success ? result.Value : null;
            SelectedSize = PizzaInfo.Size.First(); */
            Qty = 1;
            var pizzaInfo = await pizzaService.GetPizzaInfoAsync();
            StateContainer.SetPizzaInfo(pizzaInfo);
            PizzaInfo = StateContainer.PizzaInfo;
            SelectedSize = PizzaInfo.Size.First();
            UpdatePrice();
            
            StateContainer.OnChange += StateHasChanged;
        }


        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
