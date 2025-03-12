using System.Net.Http;
using System.Text.Json;
using RecipeDiscovery.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                Ingredients = ExtractIngredientNames(meal), 
                Cuisine = meal.GetProperty("strArea").GetString() ?? ""
            }).ToList();

            foreach (var recipe in recipes)
            {
                await _enrichmentService.Enrich(recipe);
            }

            return recipes;
        }

        public async Task<Recipe?> GetRecipeById(string id)
        {
            if (string.IsNullOrEmpty(id) || id.Length != 5 || !id.All(char.IsDigit))
            {
                return null;
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
                Ingredients = ExtractIngredientNames(meal), 
                Cuisine = meal.GetProperty("strArea").GetString() ?? ""
            };

            await _enrichmentService.Enrich(recipe);

            return recipe;
        }

        // Method to extract ingredient names
        private static List<string> ExtractIngredientNames(JsonElement meal)
        {
            var ingredients = new List<string>();
            for (int i = 1; i <= 20; i++)
            {
                string ingredientKey = $"strIngredient{i}";

                if (meal.TryGetProperty(ingredientKey, out JsonElement ingredientElement))
                {
                    string ingredient = ingredientElement.GetString() ?? "";

                    if (!string.IsNullOrWhiteSpace(ingredient))
                    {
                        ingredients.Add(ingredient);
                    }
                }
            }
            return ingredients;
        }
    }
}