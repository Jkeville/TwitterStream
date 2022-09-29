using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Diagnostics;
using TweetStream.Consume;
using TweetStream.Models.Models;
using TwitterStream.Contracts;
using TwitterStream.Models.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Redis.MessageQueue.Services
{
    public class MessageQueueProcessService : IMessageQueueProcess
    {
        public delegate MessageQueueProcessService Factory(string jobName, CancellationToken cancellationToken = new CancellationToken());

        private IConnectionMultiplexer _connectionMultiplexer;//=> _lazyConnection.Value;

        private readonly string _jobQueue;
        private readonly string _processingQueue;
        private readonly string _subChannel;
        private readonly string _jobName;
        private readonly CancellationToken _cancellationToken;

        private Task _managementTask;
        private IConfiguration _configuration;
        private string _tweetCountKey;
        private string _sessionCountKey;
        private string _sessionStartKey;
        private string _hashTagQueueKey;
        private string jobName;
        private readonly ILoggerManager _logger;
        private bool _receiving;
        private readonly IOptions<TwitterQueueConfiguration> _twitterQueuesConfiguration;

        public event EventHandler<JobReceivedEventArgs> OnJobReceived;

        public MessageQueueProcessService(IConnectionMultiplexer multiplexer, IOptions<TwitterQueueConfiguration> _queueConfig, ILoggerManager logger, CancellationToken cancellationToken = new CancellationToken())
        {
            _twitterQueuesConfiguration= _queueConfig;
            _tweetCountKey = _twitterQueuesConfiguration.Value.TweetCountKey;
            _hashTagQueueKey = _twitterQueuesConfiguration.Value.HashTagQueueKey;
            _sessionCountKey = _twitterQueuesConfiguration.Value.SessionTweetCount;
            _sessionStartKey = _twitterQueuesConfiguration.Value.SessionStartKey;


            _connectionMultiplexer = multiplexer;
            //ConnectionMultiplexer = multiplexer;
            _jobQueue = $"{jobName}:jobs";
            _processingQueue = $"{jobName}:process";
            _subChannel = $"{jobName}:channel";
            _jobName = jobName;
            _cancellationToken = cancellationToken;
            _logger = logger;
        }





        public async Task<double> GetSessionThroughput()
        {
            double tweetsPerSecond = 0;
            try
            {


                var db = _connectionMultiplexer.GetDatabase();
                var startString = await db.StringGetAsync(_sessionStartKey);
                var parsedDate = DateTime.Parse(startString);
                var seconds = (DateTime.Now - parsedDate).TotalSeconds;
                var tCount = await db.StringGetAsync(_sessionCountKey);

                tweetsPerSecond = Convert.ToDouble(Convert.ToDouble(tCount) / seconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to get GetSessionThroughput {0}", ex.Message));
                throw;
            }
            return tweetsPerSecond;
        }

        public async Task SetSessionStart()
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();

                await db.StringSetAsync(_sessionCountKey, 0);

                await db.StringSetAsync(_sessionStartKey, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to get tweets sample stream: {0}", ex.Message));
                throw;
            }

        }

        public async Task<bool> UpdateTweetCount(CancellationToken cancellationToken)
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();

                await db.StringIncrementAsync(_tweetCountKey);
                await db.StringIncrementAsync(_sessionCountKey);

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to get GetSessionThroughput {0}", ex.Message));
                return false; ;
            }
            return true;
        }
        public async void UpdateHashTags(Hashtag[] hashtags, CancellationToken cancellationToken)
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();



                for (int i = 0; i < hashtags.Length; i++)
                {
                    await db.SortedSetIncrementAsync(_hashTagQueueKey, hashtags[i].tag, 1);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to UpdateHashTags {0}", ex.Message));

            }


        }

        public async Task<int> GetTweetCount()
        {
            int tweetTotal = 0;

            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                var tCount = await db.StringGetAsync(_tweetCountKey);
                tweetTotal = Convert.ToInt32(tCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to GetTweetCount {0}", ex.Message));

            }

            return tweetTotal;
        }

        public async Task<String[]> GetTopHashTags()
        {

            try
            {
                var db = _connectionMultiplexer.GetDatabase();
                var hashtags = await db.SortedSetRangeByScoreAsync(_hashTagQueueKey, 0, double.PositiveInfinity, Exclude.None, Order.Descending, 0, 10);

                return hashtags.ToStringArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Failed to GetTopHashTags {0}", ex.Message));

            }

            return new string[0];

        }



    }
}
