
using TwitterStream.Contracts;
using TwitterStream.Models.Models;

namespace TweetStream.Ingestion.Services
{
    public class TweetReportService : ITweetReportService
    {
        private readonly ILoggerManager _logger;
        private readonly IMessageQueueProcess _broker;

        public TweetReportService(ILoggerManager logger, IMessageQueueProcess broker)
        {
            _logger = logger;
            _broker = broker;
        }

        public async Task<IEnumerable<string>?> GetTopTenHashtags()
        {
            IEnumerable<string> result = null;

            try
            {
             return await _broker.GetTopHashTags();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in GetTopTenHashtags(): " + ex.Message);
            }

            return result;
        }

        public async Task<int> GetTotalTweetReceived()
        {
            int result = 0;

            try
            {
               return await _broker.GetTweetCount(); 
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in GetTotalTweetReceived(): " + ex.Message);
            }

            return result;
        }


        public async Task<double> GetSessionThroughput()
        {
            double result = 0;

            try
            {
                return await _broker.GetSessionThroughput();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in GetSessionThroughput(): " + ex.Message);
            }

            return result;
        }


    }
}
