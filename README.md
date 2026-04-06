# WebTool Template API

A .NET 10 Web API template using PostgreSQL, Entity Framework Core, and JWT authentication.

## Tech Stack

- **.NET 10** (ASP.NET Core Web API)
- **PostgreSQL 16** via Npgsql + EF Core
- **JWT Bearer Authentication** with role-based authorization (`AdminOnly`, `UserOnly`)
- **Swagger UI** (development only, available at `/swagger`)
- **OpenAPI** spec at `/openapi/v1.json`

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for PostgreSQL)

## Getting Started

Follow these steps to get the API up and running on your local machine.

### 1. Database Setup

Start a PostgreSQL instance using Docker:

```bash
docker run --name webtool-pg \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=webtool \
  -p 5432:5432 \
  -d postgres:16
```

### 2. Configure Connection String

Store your database credentials securely using .NET User Secrets. Run the following commands from the project root:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=webtool;Username=postgres;Password=postgres"
```

### 3. Run Database Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Build the Application

```bash
dotnet build
```

### 5. Run the API

```bash
dotnet run
```

The API will be available at:

- **HTTP:** http://localhost:5025
- **HTTPS:** https://localhost:7065

### 6. Explore the API

Once the application is running, navigate to the Swagger UI to view the documentation and test the available endpoints:

http://localhost:5025/swagger

## Project Structure

```
├── Program.cs                    # App entry point and middleware configuration
├── Models/Foundation/            # Domain models (User, Organization)
├── Brokers/Foundation/Storages/  # EF Core DbContext and storage interfaces
├── Brokers/Logging/              # Serilog logging broker
├── Controllers/                  # API controllers
├── Services/Foundations/         # Business logic services
├── Migrations/                   # EF Core migrations
├── Properties/                   # Launch profiles
└── appsettings.json              # Configuration (connection strings, logging)
```