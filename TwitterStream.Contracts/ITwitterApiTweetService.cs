using System.Runtime.CompilerServices;
using TwitterStream.Models.Models;

namespace TwitterStream.Contracts
{
    public interface ITwitterApiTweetService
    {
        public Task<HttpResponseMessage> GetTweetsSampleStreamResponseAsync();
        public Task GetTweetsSampleStreamAsync(HttpResponseMessage response, CancellationToken cancellationToken);
        public Task ProcessTweetJson(string tweetJSON, CancellationToken cancellationToken);
    }
}