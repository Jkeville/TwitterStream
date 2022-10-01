using System;
using TweetStream.Models.Models;

namespace Blazor.UI.Client.Services
{
    public class TweetHttpRepository:ITweetHttpRepository
    {
        private readonly HttpClient _client ;

        public async Task<List<Product>> GetProducts()
        {
            List<Product> prods = new List<Product>();

            return prods;

        }
        public async Task CallChartEndpoint()
        {
            var result = await _client.GetAsync("chart");
            if (!result.IsSuccessStatusCode)
                Console.WriteLine("Something went wrong with the response");
        }

        public TweetHttpRepository(HttpClient httpClient)
        {
            _client = httpClient;

        }
    }
}
