# WebTool Template API

A .NET 10 Web API template using PostgreSQL, Entity Framework Core, and JWT authentication.

## Tech Stack

- **.NET 10** (ASP.NET Core Web API)
- **PostgreSQL 16** via Npgsql + EF Core
- **JWT Bearer Authentication** with role-based authorization (`AdminOnly`, `UserOnly`)
- **Serilog** for structured logging (console + rolling file)
- **Swagger UI** (development only, available at `/swagger`)
- **OpenAPI** spec at `/openapi/v1.json`

## Project Structure

```
Template.Api/                        ← Main API project
├── Program.cs                       ← App entry point, DI, middleware
├── appsettings.json                 ← Configuration (DB, Serilog, etc.)
├── Controllers/                     ← Thin HTTP layer (routes → services)
├── Services/Foundations/             ← Business logic, validation, exception handling
│   ├── Users/
│   │   ├── UserService.cs           ← Core operations
│   │   ├── UserService.Validation.cs← Validation rules
│   │   └── UserService.Exceptions.cs← TryCatch wrappers
│   └── Organizations/               ← Same pattern
├── Brokers/
│   ├── Foundation/Storages/         ← EF Core data access (no business logic)
│   └── Logging/                     ← Logging abstraction
├── Models/Foundation/               ← Entity classes + custom exceptions
└── Migrations/                      ← EF Core migrations

Template.Api.Tests.Unit/             ← Unit test project
└── Services/Foundations/Users/       ← Tests mirror the service structure
    ├── UserServiceTests.cs           ← Setup + mocks
    ├── UserServiceTests.Add.cs       ← AddUserAsync tests
    └── UserServiceTests.RetrieveAll.cs← RetrieveAllUsers tests
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for PostgreSQL)

## Getting Started

### 1. Clone and Restore

```bash
git clone <repo-url>
cd WebToolTemplate
dotnet restore
```

All NuGet packages for both the API and test projects are restored automatically.

### 2. Database Setup

Start a PostgreSQL instance using Docker:

```bash
docker run --name webtool-pg \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=webtool \
  -p 5432:5432 \
  -d postgres:16
```

### 3. Configure Connection String

The default connection string in `appsettings.json` matches the Docker command above. For custom credentials, use .NET User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=webtool;Username=postgres;Password=postgres"
```

### 4. Run the API

```bash
dotnet run
```

The app automatically applies pending EF Core migrations on startup. No manual `dotnet ef database update` is needed.

- **Swagger UI**: http://localhost:5025/swagger/index.html
- **OpenAPI spec**: http://localhost:5025/openapi/v1.json

## Running Tests

Unit tests use **xUnit**, **Moq** (mocking), and **FluentAssertions**. They test the service layer in isolation — no database required.

```bash
# Run all unit tests
dotnet test Template.Api.Tests.Unit

# Run with detailed output
dotnet test Template.Api.Tests.Unit --verbosity normal

# Run a specific test by name
dotnet test Template.Api.Tests.Unit --filter "ShouldAddUserAsync"

# Run all tests matching a keyword
dotnet test Template.Api.Tests.Unit --filter "Add"
```

Tests also appear in VS Code's **Testing** sidebar for clickable run/debug.

## Architecture

Request flow: **HTTP → Controller → Service → Broker → Database**

| Layer          | Responsibility                                                                                                          | Example                        |
| -------------- | ----------------------------------------------------------------------------------------------------------------------- | ------------------------------ |
| **Controller** | Receives HTTP requests, returns responses. No logic.                                                                    | `UsersController`              |
| **Service**    | Validates input, orchestrates operations, handles exceptions. Split into partial classes: core, validation, exceptions. | `UserService`                  |
| **Broker**     | Raw data access via EF Core. Delegates to generic `InsertAsync<T>` / `SelectAll<T>`. No business logic.                 | `StorageBroker`                |
| **Model**      | Entity definitions + custom exception classes.                                                                          | `User`, `InvalidUserException` |
