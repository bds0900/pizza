using Client.Models;
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
    public partial class SideDetailComponent
    {
        [Inject] 
        StateContainer StateContainer { get; set; }
        [Parameter]
        public Side Side { get; set; }
        private int Qty { get; set; } = 1;
        private float TotalPrice { get; set; }
        private float TaxPrice { get; set; }
        private float SubtotalPrice { get; set; }

        private async Task AddItem(Side side)
        {
            var items = StateContainer.SideItems;
            SubtotalPrice = (float)Math.Round(side.SidePrice * Qty, 2);
            TaxPrice = (float)Math.Round(side.SidePrice * 0.13, 2);
            TotalPrice = (float)Math.Round(SubtotalPrice + TaxPrice, 2);
            SideItem item = new SideItem
            {
                ItemId = new Guid(),
                Qty = Qty,
                SideId = side.SideId,
                Subtotal = SubtotalPrice,
                Tax = TaxPrice,
                Total = TotalPrice
            };
            items.Add(item);

            StateContainer.SetSideItems(items);

        }

        private void AddQty(int num)
        {
            Qty += num;
            if (Qty < 1) Qty = 1;
        }
    }
}
