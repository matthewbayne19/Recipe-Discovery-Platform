using System.Net.Http;
using System.Text.Json;
using RecipeDiscovery.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient;
        private readonly IEnrichmentService _enrichmentService;
        private const string ApiUrl = "https://www.themealdb.com/api/json/v1/1/search.php?s=";

        public RecipeService(HttpClient httpClient, IEnrichmentService enrichmentService)
        {
            _httpClient = httpClient;
            _enrichmentService = enrichmentService;
        }

        // Fetch basic recipe data only
        public async Task<List<Recipe>> GetAllRecipes(string query)
        {
            var response = await _httpClient.GetStringAsync(ApiUrl + query);
            var json = JsonDocument.Parse(response);

            var meals = json.RootElement.GetProperty("meals");

            if (meals.ValueKind == JsonValueKind.Null)
                return new List<Recipe>();

            var recipes = meals.EnumerateArray().Select(meal => new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString() ?? "",
                Name = meal.GetProperty("strMeal").GetString() ?? "",
                Description = meal.GetProperty("strInstructions").GetString() ?? "",
                Ingredients = new List<string> { meal.GetProperty("strIngredient1").GetString() ?? "" },
                Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                // Do not set PreparationTime and DifficultyLevel here
            }).ToList();

            // Enrich recipes with nutrition, preparation time, and difficulty level
            foreach (var recipe in recipes)
            {
                await _enrichmentService.Enrich(recipe);
            }

            return recipes;
        }

        // Fetch basic recipe by ID only
        public async Task<Recipe?> GetRecipeById(string id)
        {
            if (string.IsNullOrEmpty(id) || id.Length != 5 || !id.All(char.IsDigit))
            {
                return null; // Invalid ID format
            }

            string apiUrl = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={id}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var json = JsonDocument.Parse(response);

            var meals = json.RootElement.GetProperty("meals");

            if (meals.ValueKind == JsonValueKind.Null)
                return null;

            var meal = meals.EnumerateArray().FirstOrDefault();

            if (meal.ValueKind == JsonValueKind.Undefined)
                return null;

            var recipe = new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString() ?? "",
                Name = meal.GetProperty("strMeal").GetString() ?? "",
                Description = meal.GetProperty("strInstructions").GetString() ?? "",
                Ingredients = new List<string> { meal.GetProperty("strIngredient1").GetString() ?? "" },
                Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                // Do not set PreparationTime and DifficultyLevel here
            };

            // Enrich the recipe with missing data (preparation time, difficulty, nutrition)
            await _enrichmentService.Enrich(recipe);

            return recipe;
        }
    }
}
