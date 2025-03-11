using RecipeDiscovery.Services;
using RecipeDiscovery.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RecipeDiscovery.Middleware;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RecipeDiscovery.Tests")]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();

        // Enables API controllers
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
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

        // Register services with correct lifetimes
        builder.Services.AddSingleton<IRecipeService, RecipeService>(); 
        builder.Services.AddSingleton<IUserService, UserService>(); 
        builder.Services.AddSingleton<IEnrichmentService, EnrichmentService>(); 

        // Register HttpClient for RecipeService
        builder.Services.AddHttpClient<IRecipeService, RecipeService>();

        // Register GraphQL services
        builder.Services
            .AddGraphQLServer()
            .AddQueryType<Query>()  // Register GraphQL queries
            .AddMutationType<Mutation>(); // Register GraphQL mutations

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        // Apply the middleware to check API key for every request
        app.UseMiddleware<ApiKeyMiddleware>(); // Middleware applied here

        app.UseAuthorization();

        // Maps controllers to endpoints
        app.MapControllers();

        // Maps GraphQL endpoint
        app.MapGraphQL();

        app.Run();
    }
}
