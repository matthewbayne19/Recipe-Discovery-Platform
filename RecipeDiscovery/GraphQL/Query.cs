using RecipeDiscovery.Services;
using RecipeDiscovery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.GraphQL
{
    public class Query
    {
        private readonly IRecipeService _recipeService; // Handles fetching recipes from TheMealDB API
        private readonly IUserService _userService; // Manages user-related operations like favorites
        private readonly IEnrichmentService _enrichmentService; // Handles enrichment of recipe data with additional information

        public Query(IRecipeService recipeService, IUserService userService, IEnrichmentService enrichmentService)
        {
            _recipeService = recipeService;
            _userService = userService;
            _enrichmentService = enrichmentService;
        }

        // Fetch all recipes with optional filtering, sorting, pagination, and enrichment
        public async Task<List<Recipe>> GetRecipes(
            string? cuisine = null,
            string? ingredient = null,
            string? sortBy = null,
            string order = "asc", // Default order is ascending
            int page = 1,        // Default page is 1
            int pageSize = 10    // Default page size is 10
        )
        {
            // Fetch all recipes from the external API
            var recipes = await _recipeService.GetAllRecipes("");

            // Enrich each recipe with mock data for nutrition, preparation time, and difficulty
            foreach (var recipe in recipes)
            {
                await _enrichmentService.Enrich(recipe);
            }

            // Apply cuisine filter if specified
            if (!string.IsNullOrEmpty(cuisine))
            {
                recipes = recipes.Where(r => r.Cuisine.Equals(cuisine, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply ingredient filter if specified
            if (!string.IsNullOrEmpty(ingredient))
            {
                recipes = recipes.Where(r => r.Ingredients != null &&
                    r.Ingredients.Any(i => i.Contains(ingredient, System.StringComparison.OrdinalIgnoreCase))).ToList();
            }

            // Apply sorting based on the specified field
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "name")
                {
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
                else if (sortBy.ToLower() == "cuisine")
                {
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Cuisine).ToList()
                        : recipes.OrderBy(r => r.Cuisine).ToList();
                }
                else if (sortBy.ToLower() == "preparationtime")
                {
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.PreparationTime).ToList()
                        : recipes.OrderBy(r => r.PreparationTime).ToList();
                }
                else
                {
                    // Default sorting by name if an unknown field is specified
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
            }

            // Apply pagination: Skip previous pages and take only the requested number of items
            var pagedRecipes = recipes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return pagedRecipes;
        }

        // Fetch a single recipe by ID and enrich it with additional details
        public async Task<Recipe?> GetRecipeById(string id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            
            if (recipe != null)
            {
                // Enrich the recipe with mock data
                await _enrichmentService.Enrich(recipe);
            }

            return recipe;
        }

        // Fetch a user's favorite recipes (returns only recipe IDs)
        public async Task<List<string>> GetUserFavorites(string userId)
        {
            var favorites = await _userService.GetUserFavorites(userId);
            return favorites;
        }
    }
}
