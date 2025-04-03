using RecipeDiscovery.Services;
using RecipeDiscovery.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RecipeDiscovery.Middleware;

// Allows test project to access internal classes
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RecipeDiscovery.Tests")]

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add REST API controller support
        builder.Services.AddControllers();

        // Enable API documentation with Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            // REST API Key header configuration
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                Description = "Please enter your API key"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    new string[] { }
                }
            });

            // GraphQL Bearer token support
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Description = "Bearer {your_api_key}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        // Enable CORS for frontend (React)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Register services (singleton for state persistence across requests)
        builder.Services.AddSingleton<IRecipeService, RecipeService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IEnrichmentService, EnrichmentService>();

        // Register HttpClient for external API calls
        builder.Services.AddHttpClient<IRecipeService, RecipeService>();

        // Add GraphQL support
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>();

        var app = builder.Build();

        // Swagger UI only in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        // Enable CORS before anything that uses authentication
        app.UseCors("AllowAll");

        // Custom middleware for API key validation
        app.UseMiddleware<ApiKeyMiddleware>();

        // Authorization middleware
        app.UseAuthorization();

        // Map REST API controllers
        app.MapControllers();

        // Map GraphQL endpoint
        app.MapGraphQL();

        // Start the application
        app.Run();
    }
}
