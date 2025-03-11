using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;

public class RestApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RestApiTests(WebApplicationFactory<Program> factory)
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
