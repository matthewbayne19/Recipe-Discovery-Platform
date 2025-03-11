using RecipeDiscovery.Services;
using RecipeDiscovery.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;  // Added for Swagger API key configuration
using RecipeDiscovery.Middleware; // Added this to include ApiKeyMiddleware

// Make class accessible to test project
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
            // Configure Swagger to accept the API Key header for both REST and GraphQL
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

            // Enable passing the API key in GraphQL requests
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

        /* 
        Register RecipeService with dependency injection
        Why AddHttpClient() for IRecipeService: IRecipeService interacts with an external API (TheMealDB) to fetch data. 
        Since HttpClient is designed to be reused for making HTTP requests (instead of creating a new instance for each call), 
        we use AddHttpClient() to register IRecipeService and configure an HttpClient to be injected into it. 
        */
        builder.Services.AddHttpClient<IRecipeService, RecipeService>();

        /* 
        Register UserService with dependency injection
        IUserService is a service that we want to have a single instance for the lifetime of the application. Since it manages user data 
        and does not need to be re-created for each request, we use AddSingleton(). It ensures that there is a single instance of UserService 
        throughout the application lifecycle, which is efficient when dealing with stateful services or services that don't need to be 
        recreated per request.
        */
        builder.Services.AddSingleton<IUserService, UserService>();

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
        app.UseMiddleware<ApiKeyMiddleware>(); // This is where the middleware is applied, no need to register it in services

        app.UseAuthorization();

        // Maps controllers to endpoints
        app.MapControllers();

        // Maps GraphQL endpoint
        app.MapGraphQL();

        app.Run();
    }
}
