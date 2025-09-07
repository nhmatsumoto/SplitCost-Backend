using Microsoft.IdentityModel.Tokens;
using Playground.API.Middlewares;
using Playground.Infrastructure.DependencyInjection;
using Serilog;
using SplitCost.Application.Common.Services;
using SplitCost.Application.Extensions;
using SplitCost.Domain.Exceptions.Extensions;
using SplitCost.Infrastructure.Services;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SplitCost.Infrastructure"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<IKeycloakService, KeycloakService>();

// Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:8080/realms/split-costs";
        options.Audience = "split-costs-client";
        options.RequireHttpsMetadata = false; //true em produção

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            NameClaimType = "preferred_username",
            RoleClaimType = "realm_access.roles"
        };
    });

// Authorization
builder.Services.AddAuthorization();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application 
builder.Services.AddApplication();

// Exceptions
builder.Services.AddExceptions(builder.Configuration);

// Alterar para operar em produção
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("FrontendPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();