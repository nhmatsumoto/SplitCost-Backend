using Microsoft.IdentityModel.Tokens;
using Playground.API.Middlewares;
using Playground.Infrastructure.DependencyInjection;
using SplitCost.Application.DependencyInjection;
using SplitCost.Domain.Interfaces;
using SplitCost.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SplitCost.Infrastructure"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<IKeycloakService, KeycloakService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:8080/realms/split-costs";
        options.Audience = "split-costs-client";
        options.RequireHttpsMetadata = false; //true em produção

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, // Se não usar `aud` no token
            NameClaimType = "preferred_username", // ou "name", se quiser o nome do usuário
            RoleClaimType = "realm_access.roles" // Para usar roles do Keycloak
        };
    });

builder.Services.AddAuthorization();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application 
builder.Services.AddApplication();


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

// Exception Middleware 
app.UseMiddleware<ExceptionHandlingMiddleware>();

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