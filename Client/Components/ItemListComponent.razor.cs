using Client.Models;
using Client.Store;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class ItemListComponent
    {
        [Inject] 
        StateContainer StateContainer { get; set; }
        List<Item> Items { get; set; }
        protected override async Task OnInitializedAsync()
        {

            StateContainer.OnChange += StateHasChanged;
        }

        private async Task RemoveItem(Guid itemid)
        {
            var items = StateContainer.Items;
            var item = items.Find(id => id.ItemId == itemid);
            items.Remove(item);
            StateContainer.SetItems(items);
        }
        private async Task RemoveSideItem(Guid itemid)
        {
            var items = StateContainer.SideItems;
            var item = items.Find(id => id.ItemId == itemid);
            items.Remove(item);
            StateContainer.SetSideItems(items);
        }

        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
}
