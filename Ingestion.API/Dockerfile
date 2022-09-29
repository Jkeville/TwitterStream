#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ingestion.API/Ingestion.API.csproj", "Ingestion.API/"]
COPY ["TwitterStream.Contracts/TwitterStream.Contracts.csproj", "TwitterStream.Contracts/"]
COPY ["TwitterStream.Models/TwitterStream.Models.csproj", "TwitterStream.Models/"]
COPY ["TwitterStream.LoggerService/TwitterStream.LoggerService.csproj", "TwitterStream.LoggerService/"]
RUN dotnet restore "Ingestion.API/Ingestion.API.csproj"
COPY . .
WORKDIR "/src/Ingestion.API"
RUN dotnet build "Ingestion.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ingestion.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ingestion.API.dll"]