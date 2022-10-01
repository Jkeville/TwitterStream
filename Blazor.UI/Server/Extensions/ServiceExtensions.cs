
using Microsoft.Extensions.Configuration;
using Redis.MessageQueue.Services;
using StackExchange.Redis;
using TweetStream.Ingestion.Services;

using TwitterStream.Contracts;
using TwitterStream.LoggerService;
using TweetStream.Models.Models;

namespace Blazor.UI.Server.Extensions
{
    public static class ServiceExtensions
    {
        //configure CORS
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        //configure IIS
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
                //use default values
            });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureTwitterApiServices(this IServiceCollection services, IConfigurationSection twitterApiConfigurationSection)
        {
            services.Configure<TwitterApiConfiguration>(twitterApiConfigurationSection);
            services.AddSingleton<ITwitterApiAuthService, TwitterApiAuthService>();
            services.AddSingleton<ITwitterApiTweetService, TwitterApiTweetService>();
            services.AddSingleton<ITweetReportService, TweetReportService>();
        }
        public static void ConfigureTwitterQueues(this IServiceCollection services, IConfigurationSection twitterQueuesConfigurationSection)
        {
            services.Configure<TwitterQueueConfiguration>(twitterQueuesConfigurationSection);
        }

            public static void ConfigureBrokerServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageQueueProcess, MessageQueueProcessService>();
           
        }

        //configure Redis
        public static void ConfigureRedisDB(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton<IConnectionMultiplexer>(opt =>
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"))
            );

      

      
        public static void ConfigurePublishing(this IServiceCollection services, IConfiguration configuration) =>
           services.AddSingleton<IConnectionMultiplexer>(sp =>
                 ConnectionMultiplexer.Connect(new ConfigurationOptions
                 {
                     EndPoints = { configuration.GetConnectionString("RedisConnection")
                 },
                  AbortOnConnectFail = false,
               }));


       
    }
}
