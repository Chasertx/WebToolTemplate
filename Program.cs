using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Template.Api.Brokers.Foundation.Storages;
using Template.Api.Services.Foundations.Organizations;
using Template.Api.Services.Foundations.Users;
using Serilog;
using Template.Api.Brokers.Logging;

//Initializing the web application builder.
var builder = WebApplication.CreateBuilder(args);

//Telling the host to use serilog.
//This logging configuration has been delegated to
//Appsettings.json
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//Mapping logger interface to it's implementation.
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();

//Registering DB context with PGSQL configuration for now.
builder.Services.AddDbContext<StorageBroker>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registering controller services.
builder.Services.AddControllers();
//Registering api explorer for documentation.
builder.Services.AddEndpointsApiExplorer();
//Register OpenApi generation.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
        {

        }

        return Task.CompletedTask;
    });
});

//Mapping storage interface to implementation.
builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddTransient<IUserService, UserService>();

//Initializing JWT Authentication service.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MyApi",
            ValidAudience = "MyUsers",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourVerySecretKey123!"))
        };
    });

//Registering Authorization policies (these are place holders).
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

//Constructing instance of the application.
var app = builder.Build();

//Enabling logging across all requests.
app.UseSerilogRequestLogging();
app.Logger.LogInformation("Serilog is now targeting the console!");
//Development specific middleware.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

//Creating a service scope for initial database operations.
using (var scope = app.Services.CreateScope())
{
    var broker = scope.ServiceProvider.GetRequiredService<IStorageBroker>() as StorageBroker;
    app.Logger.LogInformation("Checking DB Connectivity...");
    broker?.Database.Migrate();
}
//Redirects requests to https.
app.UseHttpsRedirection();
//Enabling identity identification middleware.
app.UseAuthentication();
//Enabling permission enforcement middleware.
app.UseAuthorization();
//Route incoming requests to controllers.
app.MapControllers();
//Execute the application.
app.Run();