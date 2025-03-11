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

        public async Task<List<Recipe>> GetAllRecipes(string query)
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

        public async Task<Recipe?> GetRecipeById(string id)
        {
            // Validate that the ID is exactly 5 digits and numeric
            if (string.IsNullOrEmpty(id) || id.Length != 5 || !id.All(char.IsDigit))
            {
                return null; // Invalid ID format
            }

            // Proceed with the API call only if the ID is valid
            string apiUrl = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={id}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var json = JsonDocument.Parse(response);

            var meals = json.RootElement.GetProperty("meals");

            if (meals.ValueKind == JsonValueKind.Null)
                return null; // No meals found, return null

            var meal = meals.EnumerateArray().FirstOrDefault();

            if (meal.ValueKind == JsonValueKind.Undefined)
                return null; // No meal data found, return null

            return new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString() ?? "",
                Name = meal.GetProperty("strMeal").GetString() ?? "",
                Description = meal.GetProperty("strInstructions").GetString() ?? "",
                Ingredients = new List<string> { meal.GetProperty("strIngredient1").GetString() ?? "" },
                Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                PreparationTime = "Unknown", // Default value
                DifficultyLevel = "Unknown" // Default value
            };
        }
    }
}
