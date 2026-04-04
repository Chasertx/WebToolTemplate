# Getting Started
Follow these steps to get the API up and running on your local machine.

## Database Setup
1. Ensure you have a PostgreSQL instance running. You can use a local installation or a containerized version via Docker.

2. Configure Connection String
Store your database credentials securely using .NET User Secrets. Run the following command in your terminal from the project root:

##### Bash
        dotnet user-secrets init
        dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YourConnectionString"

### Build the Application:

##### Bash
        dotnet build

### Run the API

##### Bash
        dotnet run

### Explore the API
Once the application is running, navigate to the Swagger UI to view the documentation and test the available endpoints:

        URL: http://localhost:configuredPort/swagger/index.html