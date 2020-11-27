using Client.Services;
using Client.Store;
using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages
{
    public class OrderIdModel
    {
        [Required]
        [RegularExpression(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", ErrorMessage = "Not a valid order ID")]
        public string OrderId { get; set; }
    }
    public partial class Track
    {
        private OrderIdModel model = new OrderIdModel();
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }


        public List<Process> Processing { get; set; }
        [Inject]
        StateContainer StateContainer { get; set; }
        [Inject]
        PizzaService pizzaService { get; set; }
        public async Task TrackOrderId()
        {
            Guid id = new Guid(model.OrderId);
/*            HttpClient _client = new HttpClient();
            var response = await _client.GetAsync($"https://localhost:44355/api/pizza/track/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var products = JsonSerializer.Deserialize<List<Process>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });*/
            var processes = await pizzaService.GetProcessAsync(id);
            Processing = processes;
            StateHasChanged();
        }



    }
}
