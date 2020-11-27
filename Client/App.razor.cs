using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Client
{
    public partial class App
    {
        [Parameter]
        public InitialApplicationState InitialState { get; set; }

        protected  override Task OnInitializedAsync()
        {

            TokenProvider.AccessToken = InitialState.AccessToken;
            TokenProvider.RefreshToken = InitialState.RefreshToken;
            TokenProvider.IdentityToken = InitialState.IdentityToken;
            
            return base.OnInitializedAsync();
        }


        
    }
}
