using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Used for parsing JSON responses

namespace RecipeDiscovery.Tests
{
    public class GraphQLTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string ApiKey = "simple-api-key"; // API key required for authentication

        public GraphQLTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // Test to add a favorite recipe and check if it succeeds
        [Fact]
        public async Task AddFavoriteRecipe_Returns_Success()
        {
            var mutation = @"
                mutation {
                    addFavoriteRecipe(userId: ""testUser"", recipeId: ""53086"") 
                }";

            var requestContent = new
            {
                query = mutation
            };

            var jsonRequest = JsonConvert.SerializeObject(requestContent);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Attach API key

            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode(); // Ensure request succeeded

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);

            Assert.Null(jsonResponse["errors"]); // Ensure no errors in response
        }

        // Test to fetch user's favorite recipe IDs
        [Fact]
        public async Task GetUserFavorites_Returns_Favorites()
        {
            var query = @"
                query {
                    userFavorites(userId: ""user123"")
                }";

            var requestQuery = new
            {
                query
            };

            var jsonQuery = JsonConvert.SerializeObject(requestQuery);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Attach API key

            var queryResponse = await _client.PostAsync("/graphql", queryContent);
            queryResponse.EnsureSuccessStatusCode(); // Ensure request succeeded

            var queryResponseContent = await queryResponse.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(queryResponseContent);

            Assert.Null(jsonResponse["errors"]); // Ensure no errors in response
        }

        // Test to add a favorite recipe and then verify it's in the favorites list
        [Fact]
        public async Task AddFavoriteRecipe_Then_GetUserFavorites_ReturnsUpdatedFavorites()
        {
            // Add a recipe to favorites
            var mutation = @"
                mutation {
                    addFavoriteRecipe(userId: ""user123"", recipeId: ""53086"") 
                }";

            var requestContent = new
            {
                query = mutation
            };

            var jsonRequest = JsonConvert.SerializeObject(requestContent);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey); // Attach API key

            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode(); // Ensure request succeeded

            // Fetch user's favorite recipes to verify the addition
            var query = @"
                query {
                    userFavorites(userId: ""user123"")
                }";

            var requestQuery = new
            {
                query
            };

            var jsonQuery = JsonConvert.SerializeObject(requestQuery);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            var queryResponse = await _client.PostAsync("/graphql", queryContent);
            queryResponse.EnsureSuccessStatusCode(); // Ensure request succeeded

            var queryResponseContent = await queryResponse.Content.ReadAsStringAsync();

            Assert.Contains("53086", queryResponseContent); // Ensure the added recipe ID exists in the response

            var jsonResponse = JObject.Parse(queryResponseContent);
            Assert.Null(jsonResponse["errors"]); // Ensure no errors in response
        }
    }
}
