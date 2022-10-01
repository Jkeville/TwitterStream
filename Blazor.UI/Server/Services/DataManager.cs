using Blazor.UI.Server.Controllers;
using TweetStream.Models.Models;
using TwitterStream.Contracts;

namespace Blazor.UI.Server.Services
{
    public  class DataManager:IDataManager
    {


        private readonly ITweetReportService _tweetReports;

        private readonly ILogger<TweetController> _logger;
        public async  Task<TopTenTotalsModel> GetData()
        {
            TopTenTotalsModel totals = new TopTenTotalsModel();
            try
            {
                totals.TopTenHashTags = await _tweetReports.GetTopTenHashtags();
                totals.TotalTweets = await _tweetReports.GetTotalTweetReceived();
                totals.TweetsPerSecond = await _tweetReports.GetSessionThroughput();


            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);

            }

            return totals;
        }

        public DataManager(ILogger<TweetController> logger, ITweetReportService reportService)
        {
            _logger = logger;
            _tweetReports = reportService;

        }

    }


}
