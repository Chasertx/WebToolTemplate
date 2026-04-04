Getting Started
Follow these steps to get the API up and running on your local machine.

1. Database Setup
Ensure you have a PostgreSQL instance running. You can use a local installation or a containerized version via Docker.

2. Configure Connection String
Store your database credentials securely using .NET User Secrets. Run the following command in your terminal from the project root:

Bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<YourConnectionString>"
3. Build the Application
Compile the project and restore all necessary dependencies:

Bash
dotnet build
4. Run the API
Launch the application:

Bash
dotnet run
5. Explore the API
Once the application is running, navigate to the Swagger UI to view the documentation and test the available endpoints:

URL: http://localhost:<configuredPort>/swagger/index.html