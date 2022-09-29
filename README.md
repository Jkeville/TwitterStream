# TwitterSampledStreamDemo

## Introduction

Twitter Service API communicates with Twitter API to consume a random sample of approximately 1% of the full tweet stream and keep track of the following:

- Total number of tweets received
- Top 10 Hashtags

![image](https://user-images.githubusercontent.com/54370206/185527020-1c78edd6-60a0-4881-b95b-7dd313777fa1.png)


## Setting up project

Requirements
- .NET 6.0 SDK
- Visual Studio / VS Code 
- Docker Desktop

1. Running **Docker Desktop** to make sure docker daemon is running
2. Running Redis:

	Open command prompt  CMD to go to the location where the docker-compose.yaml file is located in the app source code at: 
	**..\TwitterSampledStreamDemo\TwitterService.API**
	
3.	Run this in CMD to start docker container running Redis
	>docker compose up -d
4.  Set **TwitterService.API** as Startup Project
5. Run TwitterSampledStreamDemo

## Authentication
For Twitter API Access, the application will use the **ClientId** and **ClientSecrect** that are stored in the appsettings.json to get the token from Twitter API's authentication endpoint:
https://api.twitter.com/oauth2/token

![image](https://user-images.githubusercontent.com/54370206/185506995-cd96f82e-72f3-4091-9918-62d1413044b5.png)


## Endpoints

![image](https://user-images.githubusercontent.com/54370206/185501874-e264d79f-a201-4e32-a453-fc47f9778f15.png)

| Method   | URL                                      | Description                                                   |
| -------- | ---------------------------------------- | ------------------------------------------------------------- |
| `GET`    | `/api/tweets/sampled/stream`             | get the sampled stream from TwitterAPI endpoint.            |
| `GET`    | `/api/tweetreport/total_tweets`          | retrieve total number of tweets have received.                |
| `GET`    | `/api/tweetreport/top10_hashtags`        | retrieve top 10 hashtags from total of tweets have received   |



### Get sampled stream from Twitter API v2 endpoint:
https://api.twitter.com/2/tweets/sample/stream

![image](https://user-images.githubusercontent.com/54370206/185519259-d408a8e6-2532-42bb-b45d-1978ce1ed97d.png)

### Get total number of tweets received:
![image](https://user-images.githubusercontent.com/54370206/185517155-36a922b7-fa4c-4790-baea-f069868b460d.png)

### Get top 10 hashtags based on total tweets received so far:
this will return top 10 hashtags with the tag name and [number of occurrences]

example response: 
karşıyaka [91],SheHulk [61],unsubscribe [59],buca [53],KCAMexico [51],BLACKPINK [43],çerkezköy [43],alsancak [40],cigli [37],BTS [36]

![image](https://user-images.githubusercontent.com/54370206/185517076-7d851dec-b986-435a-87f5-04b2ebf94836.png)


