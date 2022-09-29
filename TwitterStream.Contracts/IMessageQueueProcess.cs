using StackExchange.Redis;
using TwitterStream.Models.Models;

namespace TwitterStream.Contracts
{
    public interface IMessageQueueProcess
    {

      
         public void UpdateHashTags(Hashtag[] hashtags,  CancellationToken cancellationToken);

        public Task<bool> UpdateTweetCount( CancellationToken cancellationToken);

        public Task<String[]> GetTopHashTags();

        public Task<int> GetTweetCount();

        public Task<double> GetSessionThroughput();

        public Task SetSessionStart();

    }
}
