using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Client.Services;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4;

namespace Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationCore();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //services.AddSingleton<WeatherForecastService>();
            services.AddHttpClient();
            services.AddScoped<PizzaService>();
            services.AddScoped<Store.StateContainer>();
            services.AddScoped<TokenProvider>();

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = "oidc";
                /*config.DefaultScheme =CookieAuthenticationDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme =OpenIdConnectDefaults.AuthenticationScheme;*/
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://identity-test-server.herokuapp.com";                    
                    config.ClientId = "client_id_mvc";

                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;

                    config.ResponseType = "code";
                    
                    //config.Scope.Clear();
                    config.Scope.Add("ApiOne");
                    config.Scope.Add("openid");
                    config.Scope.Add(IdentityServerConstants.StandardScopes.Phone);
                    config.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                    config.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                    config.Scope.Add("rc.scope");

                    var jwtHandler = new JwtSecurityTokenHandler
                    {
                        MapInboundClaims = false
                    };
                    config.SecurityTokenValidator = jwtHandler;



                    config.GetClaimsFromUserInfoEndpoint = true;

                    /*
                    config.ClaimActions.DeleteClaim("amr");
                    config.ClaimActions.MapUniqueJsonKey("RawCoding.granma", "rc.granma");

                    */

                });
            /*services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SaveTokens = true;

                options.Scope.Add("offline_access");
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
