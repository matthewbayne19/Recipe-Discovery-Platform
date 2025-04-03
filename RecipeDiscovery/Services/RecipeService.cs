using System.Net.Http;
using System.Text.Json;
using RecipeDiscovery.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RecipeDiscovery.Services
{
    // Service for handling recipe-related operations, including fetching recipes from TheMealDB API
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient; // HttpClient for making API requests
        private readonly IEnrichmentService _enrichmentService; // Service to enrich recipe data

        public RecipeService(HttpClient httpClient, IEnrichmentService enrichmentService)
        {
            _httpClient = httpClient;
            _enrichmentService = enrichmentService;
        }

        // Fetches all recipes from TheMealDB API across A-Z
        public async Task<List<Recipe>> GetAllRecipes(string query)
        {
            var allRecipes = new List<Recipe>();

            // Loop through letters a-z to get all meals
            for (char letter = 'a'; letter <= 'z'; letter++)
            {
                var response = await _httpClient.GetStringAsync($"https://www.themealdb.com/api/json/v1/1/search.php?f={letter}");
                var json = JsonDocument.Parse(response);

                if (json.RootElement.TryGetProperty("meals", out JsonElement meals) && meals.ValueKind != JsonValueKind.Null)
                {
                    var recipes = meals.EnumerateArray().Select(meal => new Recipe
                    {
                        Id = meal.GetProperty("idMeal").GetString() ?? "",
                        Name = meal.GetProperty("strMeal").GetString() ?? "",
                        Description = meal.GetProperty("strInstructions").GetString() ?? "",
                        Ingredients = ExtractIngredientNames(meal),
                        Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                        ImageUrl = meal.GetProperty("strMealThumb").GetString() ?? ""
                    }).ToList();

                    allRecipes.AddRange(recipes);
                }
            }

            // Enrich each recipe with additional details
            foreach (var recipe in allRecipes)
            {
                await _enrichmentService.Enrich(recipe);
            }

            return allRecipes;
        }

        // Fetches a single recipe by ID from TheMealDB API
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
                Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                ImageUrl = meal.GetProperty("strMealThumb").GetString() ?? ""
            };

            await _enrichmentService.Enrich(recipe);

            return recipe;
        }

        private static List<string> ExtractIngredientNames(JsonElement meal)
        {
            var ingredients = new List<string>();
            for (int i = 1; i <= 20; i++)
            {
                string key = $"strIngredient{i}";
                if (meal.TryGetProperty(key, out JsonElement ingredientEl))
                {
                    string ingredient = ingredientEl.GetString() ?? "";
                    if (!string.IsNullOrWhiteSpace(ingredient))
                        ingredients.Add(ingredient);
                }
            }
            return ingredients;
        }
    }
}
