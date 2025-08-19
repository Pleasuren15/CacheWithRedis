# CacheWithRedis

A .NET Web API demonstrating Redis caching implementation.

## Features

- ASP.NET Core Web API
- Redis caching with StackExchange.Redis
- Swagger UI for API documentation
- Docker Compose setup for Redis

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose

## Getting Started

1. **Start Redis container:**
   ```bash
   docker-compose up -d
   ```

2. **Run the application:**
   ```bash
   cd CacheWithRedis.Api
   dotnet run
   ```

3. **Access Swagger UI:**
   Navigate to `http://localhost:5000` (redirects to Swagger UI)

## Configuration

Redis connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379,password=mypassword123"
  }
}
```

## Docker Services

- **Redis**: Available on `localhost:6379` with password `mypassword123`