using System.Text;
using System.IO.Compression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Template.Api.Brokers.Foundation.Storages;
using Template.Api.Services.Foundations.Organizations;
using Template.Api.Services.Foundations.Users;
using Serilog;
using Template.Api.Brokers.Logging;
using Template.Api.Brokers.Security;

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
builder.Services.AddScoped<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<IOrganizationService, OrganizationService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISecurityBroker, SecurityBroker>();

//Initializing JWT Authentication service.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

//Registering Authorization policies (these are place holders).
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

//Registering response compression with Brotli and Gzip.
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

//Brotli already compresses better than Gzip, so Fastest
//still yields good compression with minimal CPU cost.
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    options.Level = CompressionLevel.Fastest);

//Gzip is a fallback for older clients that don't support
//Brotli, so we prioritize max compression here.
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    options.Level = CompressionLevel.SmallestSize);

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
//Compresses response bodies using Brotli/Gzip.
app.UseResponseCompression();
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