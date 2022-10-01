
using TweetStream.Models.Models;

namespace Blazor.UI.Client.Services
{
   
        public interface ITweetHttpRepository
        {
            public Task<List<Product>> GetProducts();
           public Task CallChartEndpoint();
        }
    
}
