using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Entities;
using Client.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Client.Services
{
    
    public class PizzaService
    {
        private readonly HttpClient http;
        private readonly TokenProvider tokenProvider;
        public PizzaService(IHttpClientFactory clientFactory, TokenProvider tokenProvider, IConfiguration config)
        {
            http = clientFactory.CreateClient();
            this.tokenProvider = tokenProvider;
            //http.BaseAddress = new Uri(config.GetValue<string>("BaseUri"));
            http.BaseAddress = new Uri(config["BaseUri"]);
        }

        public async Task<List<Process>> GetProcessAsync(Guid id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                                $"/api/pizza/track/{id}");
                var response = await http.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<List<Process>>();
            }
            catch(HttpRequestException ex)
            {

            }
            return null;

           
        }

        public async Task<List<Side>> GetSideAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                                "/api/pizza/sides");
                var response = await http.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<List<Side>>();
            }
            catch(HttpRequestException ex)
            {

            }
            return null;
            
        }

        public async Task<PizzaInfo> GetPizzaInfoAsync()
        {
                var request = new HttpRequestMessage(HttpMethod.Get,
                                "/api/pizza");
                var response = await http.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<PizzaInfo>();
            try
            {
            }
            catch(HttpRequestException ex)
            {

            }
            return null;
            
        }

        public async Task<List<Order>> GetOrdersAsync(Guid customerId)
        {

            try
            {
                var requst = new HttpRequestMessage(HttpMethod.Get,
                                $"/api/pizza/Customers/{customerId}/Orders");
                var response = await http.SendAsync(requst);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<List<Order>>();
            }
            catch(HttpRequestException ex)
            {

            }
            return null;
            
        }
        public async Task<OrderInfo> GetOrderAsync(Guid orderId)
        {

            try
            {
                var requst = new HttpRequestMessage(HttpMethod.Get,
                    $"/api/pizza/Orders/{orderId}");
                var response = await http.SendAsync(requst);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<OrderInfo>();
            }
            catch(HttpRequestException ex)
            {

            }
            return null;

        }



        public async Task<Order> PostCheckOutAsync(object item)
        {
            http.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                  //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
            http.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/pizza")
            {
                Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json")
            };

            try
            {
                //Failing on this Line.
                var response = await http.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return  await response.Content.ReadAsAsync<Order>();
                    
            }
            catch (HttpRequestException ex)
            {
                //
            }

            return null;
        }

        public string GetIdToken()
        {
            return tokenProvider.IdentityToken;
        }
        public string GetAccessToken()
        {
            return tokenProvider.AccessToken;
        }
    }
}
