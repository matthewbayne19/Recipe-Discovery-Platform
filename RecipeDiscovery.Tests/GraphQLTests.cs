using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Add this for JSON parsing

namespace RecipeDiscovery.Tests
{
    public class GraphQLTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string ApiKey = "simple-api-key";

        public GraphQLTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

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

            _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);

            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);
            Assert.Null(jsonResponse["errors"]);
        }

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
            _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);

            var queryResponse = await _client.PostAsync("/graphql", queryContent);
            queryResponse.EnsureSuccessStatusCode();

            var queryResponseContent = await queryResponse.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(queryResponseContent);
            Assert.Null(jsonResponse["errors"]);
        }

                [Fact]
        public async Task AddFavoriteRecipe_Then_GetUserFavorites_ReturnsUpdatedFavorites()
        {
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

            _client.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);

            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode();

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
            queryResponse.EnsureSuccessStatusCode();

            var queryResponseContent = await queryResponse.Content.ReadAsStringAsync();

            Assert.Contains("53086", queryResponseContent);

            var jsonResponse = JObject.Parse(queryResponseContent);
            Assert.Null(jsonResponse["errors"]);
        }
    }
}