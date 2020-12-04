using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Pizza
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment appEnv)
        {
            Configuration = configuration;
            _currentEnvironment = appEnv;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _currentEnvironment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                     );
            string connectionString="";
            if (_currentEnvironment.IsDevelopment())
            {
                connectionString = Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                connectionString = ConnectionUri.Convert(Environment.GetEnvironmentVariable("DATABASE_URL"));
            }
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = Configuration.GetValue<string>("JwtBearer:Authority");
                    config.Audience = Configuration.GetValue<string>("JwtBearer:Audience"); 
                });
            services.AddDbContext<PizzaDbContext>(config =>
            {
                //config.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(Startup).Assembly.FullName));
                config.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(Startup).Assembly.FullName));
                
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //https://stackoverflow.com/questions/56562956/connection-refused-on-api-request-between-containers-with-docker-compose
            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        );

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
