using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-API-KEY";  // The header key used to pass the API Key
        private const string ValidApiKey = "simple-api-key";  // The valid API key to check

        // Constructor to receive the next middleware in the pipeline
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        // The main method for processing the API Key validation
        public async Task InvokeAsync(HttpContext context)
        {
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

            // If the API key is valid, call the next middleware in the pipeline
            await _next(context);
        }
    }
}
