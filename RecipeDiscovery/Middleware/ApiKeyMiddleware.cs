using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next; // Represents the next middleware in the pipeline
        private const string ApiKeyHeaderName = "X-API-KEY";  // The header key used to pass the API Key
        private const string ValidApiKey = "simple-api-key";  // The valid API key to check

        // Constructor to receive the next middleware in the pipeline
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next)); // Ensure next middleware is not null
        }

        // Middleware logic to validate API Key
        public async Task InvokeAsync(HttpContext context)
        {
            // Allow Nitro to load without API key in development (Noticed unable to get to Nitro)
            if (context.Request.Path.StartsWithSegments("/graphql") &&
                context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }
            // Check if the API key is present in the request headers
            if (!context.Request.Headers.ContainsKey(ApiKeyHeaderName))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;  // Unauthorized if missing
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            // Extract the API key from the request header
            var apiKey = context.Request.Headers[ApiKeyHeaderName].FirstOrDefault();

            // Validate the API key
            if (apiKey != ValidApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;  // Unauthorized if invalid
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            // If the API key is valid, proceed to the next middleware in the pipeline
            await _next(context);
        }
    }
}
