using Client.Store;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Shared
{
    public partial class NavMenu
    {
        [Inject] 
        StateContainer StateContainer { get; set; }
        private bool collapseNavMenu = true;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }


        private bool collapseMyMenu = true;

        private string MyMenuCssClass => collapseMyMenu ? "collapse" : null;

        private void ToggleMyMenu()
        {
            collapseMyMenu = !collapseMyMenu;
        }







        protected async override Task OnInitializedAsync()
        {
            StateContainer.OnChange += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }

        private async Task Authentication()
        {
            HttpClient _client = new HttpClient();
            var discoveryDocument = await _client.GetDiscoveryDocumentAsync("https://localhost:44310/"); ;


        /*var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }*/
            
    }
    }
}
