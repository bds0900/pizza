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

namespace Client.Services
{
    
    public class PizzaService
    {
        private readonly HttpClient http;
        private readonly TokenProvider tokenProvider;
        public PizzaService(IHttpClientFactory clientFactory, TokenProvider tokenProvider)
        {
            http = clientFactory.CreateClient();
            this.tokenProvider = tokenProvider;
        }

        public async Task<List<Process>> GetProcessAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://localhost:44355/api/pizza/track/{id}");
            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Process>>();
        }

        public async Task<List<Side>> GetSideAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://localhost:44355/api/pizza/sides");
            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Side>>();
        }

        public async Task<PizzaInfo> GetPizzaInfoAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://localhost:44355/api/pizza/");
            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<PizzaInfo>();
        }

        public async Task<List<Order>> GetOrdersAsync(Guid customerId)
        {

            var requst = new HttpRequestMessage(HttpMethod.Get,
                $"https://localhost:44355/api/pizza/Customers/{customerId}/Orders");
            var response = await http.SendAsync(requst);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Order>>();
        }
        public async Task<OrderInfo> GetOrderAsync(Guid orderId)
        {

            var requst = new HttpRequestMessage(HttpMethod.Get,
                $"https://localhost:44355/api/pizza/Orders/{orderId}");
            var response = await http.SendAsync(requst);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsAsync<OrderInfo>();
        }



        public async Task<Order> PostCheckOutAsync(object item)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                                                                                  //client.DefaultRequestHeaders.Add("Cookie", AuthCookie);
            client.DefaultRequestHeaders.Add("X-Requested-With", "X");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44355/api/pizza")
            {
                Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json")
            };

            try
            {
                //Failing on this Line.
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return  await response.Content.ReadAsAsync<Order>();
                    
            }
            catch (HttpRequestException ex)
            {
                //
            }

            return null;
        }
        public async void Sync(Guid tmepId, Guid userId)
        {
            var requst = new HttpRequestMessage(HttpMethod.Post,
                            $"https://localhost:44355/api/pizza/Sync/{tmepId}/{userId}");
            var response = await http.SendAsync(requst);
            response.EnsureSuccessStatusCode();
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
