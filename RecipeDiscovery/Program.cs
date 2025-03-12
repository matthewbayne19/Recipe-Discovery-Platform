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

        // Add services to the container
        builder.Services.AddControllers(); // Enables REST API controllers

        // Enable API documentation using Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            // Define API key security for REST API authentication
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "X-API-KEY", // The header name used to pass the API key
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

            // Define Bearer authentication for GraphQL requests
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization", // Authorization header is used for passing tokens in GraphQL
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

        // Register services with singleton lifetimes to persist data across requests
        builder.Services.AddSingleton<IRecipeService, RecipeService>(); 
        builder.Services.AddSingleton<IUserService, UserService>(); 
        builder.Services.AddSingleton<IEnrichmentService, EnrichmentService>(); 

        // Register HttpClient for RecipeService to interact with external APIs
        builder.Services.AddHttpClient<IRecipeService, RecipeService>();

        // Register GraphQL services and define query/mutation types
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<Query>()  // Register GraphQL queries
            .AddMutationType<Mutation>(); // Register GraphQL mutations

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Enable Swagger in development mode
            app.UseSwaggerUI();
        }

        app.UseRouting();

        // Apply the API Key middleware to enforce authentication on all requests
        app.UseMiddleware<ApiKeyMiddleware>();

        // Enable authorization for secure endpoints
        app.UseAuthorization(); 

        // Maps REST API controllers
        app.MapControllers();

        // Maps GraphQL endpoint for handling GraphQL queries and mutations
        app.MapGraphQL();

        // Start the application
        app.Run();
    }
}
