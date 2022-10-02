using Blazor.UI.Shared;
using Microsoft.AspNetCore.Mvc;
using TweetStream.Models.Models;
using TwitterStream.Contracts;

namespace Blazor.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly ITwitterApiTweetService _tweetService;
        private readonly ITweetReportService _tweetReports;

        private readonly ILogger<TweetController> _logger;

        public TweetController(ILogger<TweetController> logger, ITwitterApiTweetService twitterService)
        {
            _logger = logger;
            _tweetService = twitterService;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get2()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        public async Task<StartResult> Get()
        {
            StartResult startResult = new StartResult() { IsSuccess =false};
            try
            {
                var response = await _tweetService.GetTweetsSampleStreamResponseAsync();
                _tweetService.GetTweetsSampleStreamAsync(response, new CancellationToken());
                startResult.IsSuccess = true;


            }
            catch (Exception ex)
            {
                startResult.IsSuccess = false;
                startResult.Message = ex.Message;
                _logger.LogError(ex.Message);
               
            }

            return startResult; 
        }
    }
}