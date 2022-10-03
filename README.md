# TwitterStream

## Introduction

Twitter Service API communicates with Twitter API to consume a random sample of approximately 1% of the full tweet stream and keep track of the following:

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


4.  Set **Blazor.UI.Server** as Startup Project


## Authentication
For Twitter API Access, the application will use the **ClientId** and **ClientSecrect** that are stored in the appsettings.json to get the token from Twitter API's authentication endpoint:
https://api.twitter.com/oauth2/token

## Run The Blazor Application
![image](https://user-images.githubusercontent.com/50490528/193565989-4f26df68-22b3-4dee-b33a-14b2e35c4000.png)

## Start Stream
![image](https://user-images.githubusercontent.com/50490528/193566139-641576e2-bfef-4971-9647-6bd5f750c97e.png)

![image](https://user-images.githubusercontent.com/50490528/193566198-8bb70213-ebab-48e6-8694-c89aaabfdff2.png)

## See Live Stats
![image](https://user-images.githubusercontent.com/50490528/193566442-2010838e-145e-434b-b6ac-28315b26f3b2.png)


## Top Ten HashTags
![image](https://user-images.githubusercontent.com/50490528/193566660-1353ae2b-1269-4f32-a0e8-650422c39128.png)

