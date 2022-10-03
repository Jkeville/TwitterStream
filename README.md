# TwitterStream

## Introduction

TweetStream.Ingestion communicates with Twitter API to consume a random sample of approximately 1% of the full tweet stream and keep track of the following:

- Total number of tweets received
- Top 10 Hashtags
- Tweets processed per second

## Setting up project

Requirements
- .NET 6.0 SDK
- Visual Studio / VS Code 
- Docker Desktop

1. Running **Docker Desktop** to make sure docker daemon is running
2. Run Redis Container:
3.  Set **Blazor.UI.Server** as Startup Project
![image](https://user-images.githubusercontent.com/50490528/193567235-544d4412-a7d1-4da6-b809-0352d7e4d90f.png)

## Authentication
For Twitter API Access, the application will use the **ClientId** and **ClientSecrect** that are stored in the appsettings.json to get the token from Twitter API's authentication endpoint:
https://api.twitter.com/oauth2/token

## Run The Blazor Application
![image](https://user-images.githubusercontent.com/50490528/193565989-4f26df68-22b3-4dee-b33a-14b2e35c4000.png)

## Start Stream
![image](https://user-images.githubusercontent.com/50490528/193566139-641576e2-bfef-4971-9647-6bd5f750c97e.png)

![image](https://user-images.githubusercontent.com/50490528/193566198-8bb70213-ebab-48e6-8694-c89aaabfdff2.png)

## See Live Stats
Statistics and top 10 hashtags are stored in redis from the broker app.  Each streaming session the throughput is reset and the Max Tweets/Second is recalculated for the current session only.

![image](https://user-images.githubusercontent.com/50490528/193568269-e018b860-9206-48f3-87af-a46e58e8bfbd.png)

## Statistics Configuration
The Blazor application uses SignalR to communicate with Redis to at a configurable rate to update the counters and hastags on the client.
![image](https://user-images.githubusercontent.com/50490528/193568625-f73bba71-a52a-4a04-b0d5-05aee1230f8e.png)

The "RefreshMS" key in the appsettings.json of the Blazor.UI.Server application specfies the time interval for this refresh of client data.  In this case it is set to every 200 ms.  The timer process runs for  "MaxRefreshRunMinutes".  So in this configuration as shown, the client will be updated for 120 minutes.   The stream will continue to run after 120 minutes, but results will no longer be sent to the client after 120 mins.



## Top Ten HashTags

Top ten hashtags are stored in memory on redis as each tweet is processed.

![image](https://user-images.githubusercontent.com/50490528/193566660-1353ae2b-1269-4f32-a0e8-650422c39128.png)

