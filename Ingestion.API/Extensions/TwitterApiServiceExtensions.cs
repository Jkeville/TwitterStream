using System.Text.Json;
using System.Net.Http;
using Ingestion.API.Exceptions;
using TwitterStream.Models.Models;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace Ingestion.API.Extensions
{
    public class TwitterApiServiceExtensions
    {
      public bool isCancelled { get; set; }

        public static async Task<HttpResponseMessage> GetTwitterApiResponseAsync(HttpClient httpClient, HttpRequestMessage request)
        {
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response == null)
            {
                throw new TwitterApiException("Failed to get response from Twitter API.");
            }

            if (!((int)response.StatusCode).ToString().StartsWith("2"))
            {
                var content = await response.Content.ReadAsStringAsync();

                JObject jsonErrorContent = null;
                var errorMessage = string.Empty;

                try
                {
                    jsonErrorContent = JObject.Parse(content);
                }
                catch (Exception)
                {
                    errorMessage += content;
                }

                if (jsonErrorContent != null)
                {
                    JToken errorJson = jsonErrorContent["errors"];

                    if (errorJson != null && errorJson.Type == JTokenType.Array)
                    {
                        if (errorJson[0]["message"] == null || errorJson[0]["message"].Type == JTokenType.Null)
                        {
                            errorJson = errorJson["errors"];
                        }

                        if (errorJson != null && errorJson.Type == JTokenType.Array)
                        {
                            foreach (var error in errorJson)
                            {
                                if (error["message"] != null && error["message"].Type != JTokenType.Null)
                                {
                                    errorMessage += (!string.IsNullOrWhiteSpace(errorMessage) ? "\r\n\n" : "") + error["message"];
                                }
                            }
                        }

                    }
                    else if (jsonErrorContent["detail"] != null && jsonErrorContent["detail"].Type != JTokenType.Null)
                    {
                        errorMessage += (!string.IsNullOrWhiteSpace(errorMessage) ? "\r\n\n" : "") + jsonErrorContent["detail"];
                    }
                }

                int? xRateLimitReset = null;
                if (response.Headers != null && response.Headers.Contains("x-rate-limit-reset") && int.TryParse(response.Headers.GetValues("x-rate-limit-reset").FirstOrDefault(), out int xRate))
                {
                    xRateLimitReset = xRate;
                }

                throw new TwitterApiException(string.Format("{0} response has been returned from API.", (int)response.StatusCode, !string.IsNullOrWhiteSpace(errorMessage) ? "Error message: " + errorMessage + "\r\n" : ""), xRateLimitReset);
            }

            return response;
        }

        public static async IAsyncEnumerable<Tweet> GetTweets(HttpClient httpClient, HttpRequestMessage request,
        [EnumeratorCancellation] System.Threading.CancellationToken cancellationToken = default)
        {
           
            while (true)
            {
                var response= await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
               var t=await JsonSerializer.DeserializeAsync<Tweet>(await response.Content.ReadAsStreamAsync());


                yield return t;
            }
        }
        /*
        public static async Task<Tweet> GetAllTweets(HttpClient  httpClient, HttpRequestMessage request,CancellationToken cancellationToken)
        {


            async IAsyncEnumerable<string> clientStreamData()
            {
                for (var i = 0; i < 5; i++)
                {
                    var data =  await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    yield return data;
                }
                //After the for loop has completed and the local function exits the stream completion will be sent.
            }

            await connection.SendAsync("UploadStream", clientStreamData());






            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
           

            var results = await JsonSerializer.DeserializeAsync<IAsyncEnumerable<Tweet>>(await response.Content.ReadAsStreamAsync());

                

                await foreach (Tweet tw in results)
                     yield return tw;
            }
        */
        }
    }

