using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;

public class RestApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private const string ApiKey = "simple-api-key"; // API key required for authentication

    public RestApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Test to check if the GET /recipes endpoint returns a successful response
    [Fact]
    public async Task GetRecipes_ReturnsOkResponse()
    {
        _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Attach API key to request

        var response = await _client.GetAsync("/recipes");
        response.EnsureSuccessStatusCode(); // Ensure the response status code is in the 2xx range
    }

    // Test to check if GET /recipes/{id} returns a specific recipe successfully
    [Fact]
    public async Task GetRecipeById_ReturnsOkResponse()
    {
        _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Attach API key to request

        var response = await _client.GetAsync("/recipes/53086"); // Fetch recipe with ID "53086"
        response.EnsureSuccessStatusCode(); // Ensure the response status code is in the 2xx range
    }
}
