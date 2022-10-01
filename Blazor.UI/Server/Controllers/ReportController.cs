using Microsoft.AspNetCore.Mvc;
using TweetStream.Models.Models;
using TwitterStream.Contracts;

namespace Blazor.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
      
          
            private readonly ITweetReportService _tweetReports;

        private readonly ILogger<TweetController> _logger;
        public ReportController(ILogger<TweetController> logger,  ITweetReportService reportService)
        {
            _logger=logger;
            _tweetReports=reportService;

        }
        [HttpGet]
     
        public async Task<TopTenTotalsModel> Get()
        {
            TopTenTotalsModel totals= new TopTenTotalsModel();
            try
            {
                totals.TopTenHashTags= await _tweetReports.GetTopTenHashtags();
                totals.TotalTweets= await _tweetReports.GetTotalTweetReceived();
                totals.TweetsPerSecond = await _tweetReports.GetSessionThroughput();
               

            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex.Message);
                
            }

            return totals;
        }
    }
}
