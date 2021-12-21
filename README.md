# Fund Transfer

## What's it for?

This API makes transfer of values between two accounts.

## Getting Started

First, you must have [.NET Core 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed. But, that's it.

After, just run the command below:
``` console
docker-compose up -d
```

This will start some services like ElasticSearch, Kibana and RabbitMQ.

## How to run it?
It's ready! 🚀 Now, you can start this API, just run: 
``` console
dotnet run --project FundTransferAPI
```
or press F5.

## API endpoints
To consult the available endpoints see:

https://localhost:7253/swagger

## Unit tests
``` console
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
```