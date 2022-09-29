//using Newtonsoft.Json;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Ingestion.API.Exceptions;
using Ingestion.API.Extensions;
using TwitterStream.Contracts;
using TwitterStream.Models.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Ingestion.API.Configuration;
using Microsoft.Extensions.Options;

namespace Ingestion.API.Services
{
    public class TwitterApiTweetService : ITwitterApiTweetService
    {
        private readonly ILoggerManager _logger;
        private readonly ITwitterApiAuthService _twitterApiAuthService;
        private readonly IMessageQueueProcess _broker;
        const string QUERY = "tweet.fields=author_id,created_at,entities";
       

        public TwitterApiTweetService(ILoggerManager logger, ITwitterApiAuthService twitterApiAuthService, IMessageQueueProcess broker)
        {
            _logger = logger;
            _twitterApiAuthService = twitterApiAuthService;
            _broker = broker;
        }
        public async Task GetTweetsSampleStreamAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetTweetsSampleStreamAsync");
            await _broker.SetSessionStart();

            try
            {
                //start the stream
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    while (!reader.EndOfStream)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var line = await reader.ReadLineAsync();

                        if (!string.IsNullOrWhiteSpace(line))
                        {

                            await ProcessTweetJson(line, cancellationToken);
                                
                             
                        }
                    }
                }
            }
            catch (TwitterApiException ex)
            {
                _logger.LogError(string.Format("Failed to get tweets sample stream | Twitter API error: {0}", ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to get tweets sample stream: {0}", ex.Message));
                throw;
            }
            finally
            {
                //dispose of the response if the stream ends
                if (response != null)
                {
                    response.Dispose();
                }
            }
        }


        public async Task ProcessTweetJson(string tweetJSON, CancellationToken cancellationToken)
        {
            try { 

            await _broker.UpdateTweetCount(cancellationToken);
            var tweet = JsonSerializer.Deserialize<Tweet>(tweetJSON);
            //await _broker.AddTweet(tweet.data.id, line);

            if (tweet.data != null && tweet.data.entities != null && tweet.data.entities.hashtags != null)
            {
                Hashtag[] hashtags = tweet.data.entities.hashtags;
                 _broker.UpdateHashTags(hashtags, cancellationToken);
            }
        }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed ProcessTweetJson: {0}", ex.Message));
                throw;
            }
}

        public async Task<HttpResponseMessage> GetTweetsSampleStreamResponseAsync()
        {
            var url = string.Format("https://api.twitter.com/2/tweets/sample/stream?{0}", QUERY);

            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetTweetsSampleStreamResponseAsync");
            parameters.Add("Uri", url);
            //parameters.Add("MaxRules", maxResults);

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //calling a GET request
                    using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        request.Headers.Add("Authorization", string.Format("Bearer {0}", await _twitterApiAuthService.GetOAuth2TokenAsync()));

                        return await TwitterApiServiceExtensions.GetTwitterApiResponseAsync(httpClient, request);
                    }
                }
            }
            catch (TwitterApiException ex)
            {
                _logger.LogError(String.Format("Failed to get tweet sampled stream response | Twitter API error: {0}, Parameters: {1}", ex.Message, parameters));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("Failed to get tweet sampled stream response. {0}", ex.Message));
                throw;
            }
        }

 
    }
}
