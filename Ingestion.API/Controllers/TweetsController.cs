using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ingestion.API.Extensions;
using Ingestion.API.Services;
using TwitterStream.Contracts;
using TwitterStream.Models.Models;

namespace Ingestion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ITwitterApiTweetService _twitterApiTweetService;

        public TweetsController(ILoggerManager logger, ITwitterApiTweetService twitterApiTweetService)
        {
            _logger = logger;
            _twitterApiTweetService = twitterApiTweetService;
        }
       
        [HttpGet("startstream")]
        public async Task<IActionResult> StartTweetSampledStream(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _twitterApiTweetService.GetTweetsSampleStreamResponseAsync();
                await _twitterApiTweetService.GetTweetsSampleStreamAsync(response, cancellationToken);
                //await _twitterApiTweetService.GetTweets(response, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

 
    }
}
