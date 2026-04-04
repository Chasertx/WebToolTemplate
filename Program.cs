using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Template.Api.Brokers.Foundation.Storages;
using Template.Api.Services.Foundations.Organizations;

//Initializing the web application builder.
var builder = WebApplication.CreateBuilder(args);

//Registering DB context with PGSQL configuration for now.
//Setup a PGSQL docker image, get the connection string, and put it in user secrets.
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