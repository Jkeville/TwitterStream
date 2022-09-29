using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text;
using Ingestion.API.Configuration;
using Ingestion.API.Exceptions;
using Ingestion.API.Extensions;
using TwitterStream.Contracts;

namespace Ingestion.API.Services
{
    public class TwitterApiAuthService : ITwitterApiAuthService
    {
        private readonly ILoggerManager _logger;
        private readonly IOptions<TwitterApiConfiguration> _twitterApiConfiguration;

        public TwitterApiAuthService(ILoggerManager logger, IOptions<TwitterApiConfiguration> twitterApiConfiguration)
        {
            _logger = logger;
            _twitterApiConfiguration = twitterApiConfiguration;
        }

        public async Task<string> GetOAuth2TokenAsync()
        {
            var url = "https://api.twitter.com/oauth2/token";

            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetOAuth2TokenAsync");
            parameters.Add("Uri", url);

            var content = string.Empty;

            try
            {
                string token = String.Empty;
                using (var httpClient = new HttpClient())
                {
                    //calling POST request
                    using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                    {
                        //basic authorization with clientId and clientSecret
                        request.Headers.Add("Authorization", string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}",
                            _twitterApiConfiguration.Value.ClientId, _twitterApiConfiguration.Value.ClientSecret)))));

                        //pass in a form value of grant_type
                        request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });

                        //retrieve access token from response
                        using (var response =  await TwitterApiServiceExtensions.GetTwitterApiResponseAsync(httpClient, request))
                        {
                            var jsonContent = JObject.Parse(await response.Content.ReadAsStringAsync());
                            token = jsonContent["access_token"].ToString();
                        }
                    }
                }

                return token;
            }
            catch(TwitterApiException apiExecption)
            {
                _logger.LogError(apiExecption.ToString());
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
