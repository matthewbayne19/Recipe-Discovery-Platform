using System.Net.Http;
using System.Text.Json;
using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://www.themealdb.com/api/json/v1/1/search.php?s=";

        public RecipeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string query)
        {
            var response = await _httpClient.GetStringAsync(ApiUrl + query);
            var json = JsonDocument.Parse(response);

            var meals = json.RootElement.GetProperty("meals");

            if (meals.ValueKind == JsonValueKind.Null)
                return new List<Recipe>();

            return meals.EnumerateArray().Select(meal => new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString() ?? "",
                Name = meal.GetProperty("strMeal").GetString() ?? "",
                Description = meal.GetProperty("strInstructions").GetString() ?? "",
                Ingredients = new List<string> { meal.GetProperty("strIngredient1").GetString() ?? "" }, 
                Cuisine = meal.GetProperty("strArea").GetString() ?? "", 
                PreparationTime = "Unknown", //API does not provide this, set a default value
                DifficultyLevel = "Unknown" //API does not provide this, set a default value
            }).ToList();
        }
    }
}
