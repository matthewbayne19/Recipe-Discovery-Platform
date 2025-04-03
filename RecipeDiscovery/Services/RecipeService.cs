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
        private const string ApiUrl = "https://www.themealdb.com/api/json/v1/1/search.php?s="; // API endpoint for fetching recipes

        public RecipeService(HttpClient httpClient, IEnrichmentService enrichmentService)
        {
            _httpClient = httpClient;
            _enrichmentService = enrichmentService;
        }

        // Fetches all recipes from TheMealDB API
        public async Task<List<Recipe>> GetAllRecipes(string query)
        {
            var response = await _httpClient.GetStringAsync(ApiUrl + query);
            var json = JsonDocument.Parse(response);

            var meals = json.RootElement.GetProperty("meals");

            // If no meals are found, return an empty list
            if (meals.ValueKind == JsonValueKind.Null)
                return new List<Recipe>();

            // Convert API response into a list of Recipe objects
            var recipes = meals.EnumerateArray().Select(meal => new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString() ?? "",
                Name = meal.GetProperty("strMeal").GetString() ?? "",
                Description = meal.GetProperty("strInstructions").GetString() ?? "",
                Ingredients = ExtractIngredientNames(meal),
                Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                ImageUrl = meal.GetProperty("strMealThumb").GetString() ?? "" // 👈 Add this line
            }).ToList();

            // Enrich each recipe with additional details
            foreach (var recipe in recipes)
            {
                await _enrichmentService.Enrich(recipe);
            }

            return recipes;
        }

        // Fetches a single recipe by ID from TheMealDB API
        public async Task<Recipe?> GetRecipeById(string id)
        {
            // Validate that the ID is a 5-digit numeric string
            if (string.IsNullOrEmpty(id) || id.Length != 5 || !id.All(char.IsDigit))
            {
                return null;
            }

            string apiUrl = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={id}";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var json = JsonDocument.Parse(response);

            var meals = json.RootElement.GetProperty("meals");

            // If no meal is found, return null
            if (meals.ValueKind == JsonValueKind.Null)
                return null;

            var meal = meals.EnumerateArray().FirstOrDefault();

            if (meal.ValueKind == JsonValueKind.Undefined)
                return null;

            // Map API response to a Recipe object
            var recipe = new Recipe
            {
                Id = meal.GetProperty("idMeal").GetString() ?? "",
                Name = meal.GetProperty("strMeal").GetString() ?? "",
                Description = meal.GetProperty("strInstructions").GetString() ?? "",
                Ingredients = ExtractIngredientNames(meal),
                Cuisine = meal.GetProperty("strArea").GetString() ?? "",
                ImageUrl = meal.GetProperty("strMealThumb").GetString() ?? "" // 👈 Add this line
            };


            // Enrich the recipe with additional details (mock data)
            await _enrichmentService.Enrich(recipe);

            return recipe;
        }

        // Extracts up to 20 ingredient names from the API response dynamically
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
