using RecipeDiscovery.Services;
using RecipeDiscovery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.GraphQL
{
    public class Query
    {
        private readonly IRecipeService _recipeService;
        private readonly IUserService _userService;

        public Query(IRecipeService recipeService, IUserService userService)
        {
            _recipeService = recipeService;
            _userService = userService;
        }

        // Fetch all recipes with optional filters (cuisine and ingredient)
        public async Task<List<Recipe>> GetRecipes(string? cuisine = null, string? ingredient = null)
        {
            // Fetch all recipes from the RecipeService
            var recipes = await _recipeService.GetAllRecipes("");

            // Filter recipes by cuisine if provided
            if (!string.IsNullOrEmpty(cuisine))
            {
                recipes = recipes.Where(r => r.Cuisine.Equals(cuisine, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter recipes by ingredient if provided
            if (!string.IsNullOrEmpty(ingredient))
            {
                recipes = recipes.Where(r => r.Ingredients != null && r.Ingredients.Any(i => i.Contains(ingredient, System.StringComparison.OrdinalIgnoreCase))).ToList();
            }

            // Return the filtered list of recipes
            return recipes;
        }

        // Fetch a single recipe by ID
        public async Task<Recipe?> GetRecipeById(string id)
        {
            return await _recipeService.GetRecipeById(id);
        }

        // Fetch a user's favorite recipes
        public async Task<List<Recipe>> GetUserFavorites(string userId)
        {
            // Fetch the user's favorite recipes using the userService
            var favorites = await _userService.GetUserFavorites(userId);

            // Return the list of favorite recipes
            return favorites;
        }
    }
}
