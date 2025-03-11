using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;

public class RestApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private const string ApiKey = "simple-api-key"; 

    public RestApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Test GET /recipes
    [Fact]
    public async Task GetRecipes_ReturnsOkResponse()
    {
        // Add the API key to the request headers
        _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);

        var response = await _client.GetAsync("/recipes");
        response.EnsureSuccessStatusCode(); // Check if status code is 2xx
    }

    // Test GET /recipes/{id}
    [Fact]
    public async Task GetRecipeById_ReturnsOkResponse()
    {
        // Add the API key to the request headers
        _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);

        var response = await _client.GetAsync("/recipes/53086"); // recipe name: "Migas"
        response.EnsureSuccessStatusCode(); // Check if status code is 2xx
    }
}
