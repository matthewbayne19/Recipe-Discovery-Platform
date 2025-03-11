using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RecipeDiscovery;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace RecipeDiscovery.Tests
{
    public class GraphQLTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public GraphQLTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        // Test for adding a recipe to favorites and then checking the favorites
        [Fact]
        public async Task AddFavoriteRecipe_Then_GetUserFavorites_ReturnsUpdatedFavorites()
        {
            // Add the recipe to the user's favorites
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

            // Make the POST request to the GraphQL endpoint
            var response = await _client.PostAsync("/graphql", content);
            response.EnsureSuccessStatusCode();  // Ensure the status is OK

            // Retrieve the user's favorite recipes
            var query = @"
                query {
                    userFavorites(userId: ""user123"") {
                        id
                        name
                        description
                        ingredients
                        cuisine
                        preparationTime
                        difficultyLevel
                    }
                }";

            var requestQuery = new
            {
                query
            };

            var jsonQuery = JsonConvert.SerializeObject(requestQuery);
            var queryContent = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            // Make the POST request to the GraphQL endpoint for the query
            var queryResponse = await _client.PostAsync("/graphql", queryContent);
            queryResponse.EnsureSuccessStatusCode();

            var queryResponseContent = await queryResponse.Content.ReadAsStringAsync();
            
            // Assert the response includes the added favorite recipe
            Assert.Contains("53086", queryResponseContent);  // Check if the added recipe is present in the response
        }
    }
}
