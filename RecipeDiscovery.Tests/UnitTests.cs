using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly string _testUserId = "11111111-1111-1111-1111-111111111111"; // Replace with a valid GUID

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Test GET /recipes
    [Fact]
    public async Task GetRecipes_ReturnsOkResponse()
    {
        var response = await _client.GetAsync("/recipes");
        response.EnsureSuccessStatusCode(); // Check if status code is 2xx
    }

    // Test GET /recipes/{id}
    [Fact]
    public async Task GetRecipeById_ReturnsOkResponse()
    {
        var response = await _client.GetAsync("/recipes/53086"); // recipe name: "Migas"
        response.EnsureSuccessStatusCode();
    }
}
