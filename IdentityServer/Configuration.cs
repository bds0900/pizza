using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource
                { 
                    Name="rc.scope",
                    UserClaims =
                    {
                        "rc.granma"
                    }
                }

            };
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> { 
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo",new string[]{ "rc.api.granma"}), 

            };
        public static IEnumerable<Client> GetClients() =>
            new List<Client> { 
                new Client
                {
                    ClientId="client_id",
                    ClientSecrets={new Secret("client_secret".ToSha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials ,
                    AllowedScopes={"ApiOne"}
                },
                new Client
                {
                    ClientId="client_id_mvc",
                    ClientSecrets={new Secret("client_secret_mvc".ToSha256()) },
                    AllowedGrantTypes=GrantTypes.Code,
                    AllowedScopes=
                    {
                        "ApiOne",//api resource
                        "ApiTwo",//api resource
                        IdentityServerConstants.StandardScopes.OpenId,//identity resource
                        IdentityServerConstants.StandardScopes.Profile,//identity resource
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "rc.scope",//identity resource
                        
                    },
                    RedirectUris={"https://blazor-test-client.herokuapp.com/signin-oidc" },
                    RequireConsent=false,//consent page가 안뜨게 한다. 이러이러한 권한을 수락하시겠습니까? 하고 물어보는 페이지

                    AlwaysIncludeUserClaimsInIdToken=true,
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("ApiOne"),
                new ApiScope("ApiTwo"),
            };
    }
}
