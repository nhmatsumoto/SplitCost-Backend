using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SplitCost.Domain.Repository;
using System.Data;

namespace SplitCost.Infrastructure.IoC
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("NextHomeCorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Keycloak Configuration
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:8080/realms/next-home";
                    options.Audience = "next-home-client";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"];
                            Console.WriteLine($"Token Recebido no Backend: {token}");
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Erro na Autenticação: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };
                });

        


            // Adiciona autorização
            services.AddAuthorization();

            //Application 
            services.AddApplicationServices();

            // Repository
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IDbConnection>(sp =>
                new SqlConnection(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
           
            services.AddScoped<IUserRepository, UserRepository>();

            // Fluent Validation
            // Registra automaticamente todos os validadores da camada Application
            services.AddValidatorsFromAssemblyContaining<PropertyValidator>();

            // Add FluentValidation to API pipeline
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();


            // Controllers
            services.AddControllers();


            return services;
        }
    }
}
