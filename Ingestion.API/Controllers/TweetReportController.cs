using Microsoft.AspNetCore.Mvc;
using TwitterStream.Contracts;

namespace Ingestion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetReportController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ITweetReportService _tweetReportService;

        public TweetReportController(ILoggerManager logger, ITweetReportService tweetReportService)
        {
            this._logger = logger;
            this._tweetReportService = tweetReportService;
        }

        [HttpGet("total_tweets")]
        public async Task<IActionResult> GetTotalNumberOfTweetsReceived()
        {
            return Ok(await _tweetReportService.GetTotalTweetReceived());
        }

        [HttpGet("tweets_per_second")]
        public async Task<IActionResult> GetSessionThroughput()
        {
            return Ok(await _tweetReportService.GetSessionThroughput());
        }


        [HttpGet("top10_hashtags")]
        public async Task<IActionResult> GetTopTenHashtags()
        {
            var topTenHashtags = await _tweetReportService.GetTopTenHashtags();
            
            if(topTenHashtags is null || !topTenHashtags.Any())
                return NotFound("No hash tag found.");

            return Ok(string.Join(',', topTenHashtags));
        }
    }
}
