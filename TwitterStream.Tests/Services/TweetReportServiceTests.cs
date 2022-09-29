using Moq;
using Shouldly;
using TwitterStream.Contracts;
using TwitterStream.Entities.Models;
using Ingestion.API.Services;
using Microsoft.Extensions.Configuration;
using Ingestion.API.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace TwitterStream.Tests.Services
{
    public class TweetReportServiceTests
    {
        [Fact]
        public void GetTopTenHashTags_GivenNoTweetReturnFromRepo_ShouldReturnNull()
        {
            //Arrange
            var loggerMock = new Mock<ILoggerManager>();
            var brokerMock = new Mock<IMessageQueueProcess>();
            String[] hashTags = null;

            brokerMock.Setup(t => t.GetTopHashTags()).ReturnsAsync(hashTags);
            var tweeterReportService = new TweetReportService(loggerMock.Object, brokerMock.Object);

            //Act
            var result = tweeterReportService.GetTopTenHashtags().Result;

            //Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GetTopTenHashTags_GivenTweetNotIncludeAnyHashtags_ShouldReturnNull()
        {
            //Arrange
            var loggerMock = new Mock<ILoggerManager>();
            var brokerMock = new Mock<IMessageQueueProcess>();
            var hashTags = new Hashtag[] { };

            IEnumerable<Tweet> tweets = new List<Tweet>() { 
                new Tweet() 
                { 
                    data = new Data() 
                    { 
                        author_id = "1",
                        entities = new Entities.Models.Entities(){ hashtags = hashTags }
                    } 
                } 
            };

            //tweetRepoMock.Setup(t => t.GetTopHashTags()).ReturnsAsync(tweets);
            var tweeterReportService = new TweetReportService(loggerMock.Object, brokerMock.Object);

            //Act
            var result = tweeterReportService.GetTopTenHashtags().Result;

            //Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GetTopTenHashTags_GivenValidTweetIncludeHashtags_ShouldReturnSomeHashtags()
        {
            //Arrange
            var loggerMock = new Mock<ILoggerManager>();
            var brokerMock = new Mock<IMessageQueueProcess>();
            var configMock = new Mock<IOptions<TwitterApiConfiguration>>();

            var hashTag1 = new Hashtag() { start = 1, end = 2, tag = "tag1" };
            var hashTag2 = new Hashtag() { start = 1, end = 2, tag = "tag2" };
            var hashTags = new Hashtag[] { hashTag1, hashTag2 };


            var tweet = new Tweet()
            {
                data = new Data()
                {
                    author_id = "1",
                    entities = new Entities.Models.Entities() { hashtags = hashTags }
                }
            };

            var twitterAuthService = new TwitterApiAuthService(loggerMock.Object,configMock.Object);
            var tweeterService = new TwitterApiTweetService(loggerMock.Object, twitterAuthService, brokerMock.Object);

            //brokerMock.Setup(t => t.GetTopHashTags()).ReturnsAsync(tweets);
           

            //Act
             tweeterService.ProcessTweetJson(JsonConvert.SerializeObject( tweet), new CancellationToken());
            var tweeterReportService = new TweetReportService(loggerMock.Object, brokerMock.Object);

            var result = tweeterReportService.GetTopTenHashtags().Result;
            //Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GetTotalTweetReceived_GivenNoTweetReturnFromRepo_ShouldReturnZero()
        {
            //Arrange
            var loggerMock = new Mock<ILoggerManager>();
          
            IEnumerable<Tweet> tweets = null;
            var brokerMock = new Mock<IMessageQueueProcess>();
            
            var tweeterReportService = new TweetReportService(loggerMock.Object, brokerMock.Object);

            //Act
            var result = tweeterReportService.GetTotalTweetReceived().Result;

            //Assert
            result.ShouldBe(0);
        }

        [Fact]
        public void GetTotalTweetReceived_GivenValidTweetsReturnFromRepo_ShouldReturnNumOfTweetCount()
        {
            //Arrange
            var loggerMock = new Mock<ILoggerManager>();
            var brokerMock = new Mock<IMessageQueueProcess>();
            var configMock = new Mock<IOptions<TwitterApiConfiguration>>();

            var hashTag1 = new Hashtag() { start = 1, end = 2, tag = "tag1" };
            var hashTag2 = new Hashtag() { start = 1, end = 2, tag = "tag2" };
            var hashTags = new Hashtag[] { hashTag1, hashTag2 };

            var tweet =
                new Tweet()
                {
                    data = new Data()
                    {
                        author_id = "1",
                        entities = new Entities.Models.Entities() { hashtags = hashTags }
                    }
                };


            var twitterAuthService = new TwitterApiAuthService(loggerMock.Object, configMock.Object);
            var tweeterService = new TwitterApiTweetService(loggerMock.Object, twitterAuthService, brokerMock.Object);

            //brokerMock.Setup(t => t.GetTopHashTags()).ReturnsAsync(tweets);


            //Act
            tweeterService.ProcessTweetJson(JsonConvert.SerializeObject(tweet), new CancellationToken());
            var tweeterReportService = new TweetReportService(loggerMock.Object, brokerMock.Object);
           

            //Act
            var result = tweeterReportService.GetTotalTweetReceived().Result;

            //Assert
            result.ShouldBe(1);
        }
    }
}
