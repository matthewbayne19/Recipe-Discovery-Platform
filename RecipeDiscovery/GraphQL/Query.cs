using RecipeDiscovery.Services;
using RecipeDiscovery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;              // HotChocolate GraphQL namespace
using HotChocolate.Data;        // For [UseFiltering], [UseSorting], etc.

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

        // GraphQL query to fetch all recipes with HotChocolate filtering, sorting, and pagination
        [UseFiltering] // Enables automatic filtering on fields like cuisine, ingredients, etc.
        [UseSorting]   // Enables automatic sorting on recipe fields
        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            var recipes = await _recipeService.GetAllRecipes("");

            foreach (var recipe in recipes)
            {
                await _enrichmentService.Enrich(recipe); // Add mock enrichment fields
            }

            return recipes.AsQueryable(); // Required for HotChocolate filtering to work
        }

        // Fetch a single recipe by ID and enrich it with additional details
        public async Task<Recipe?> GetRecipeById(string id)
        {
            var recipe = await _recipeService.GetRecipeById(id);

            if (recipe != null)
            {
                await _enrichmentService.Enrich(recipe); // Enrich the returned recipe
            }

            return recipe;
        }

        // Fetch a user's favorite recipes (returns only recipe IDs)
        public async Task<List<string>> GetUserFavorites(string userId)
        {
            var favorites = await _userService.GetUserFavorites(userId);
            return favorites;
        }

        // Fetch recipes by name with filtering and sorting
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<Recipe>> GetRecipesByName(string name)
        {
            var recipes = await _recipeService.GetRecipesByName(name);

            foreach (var recipe in recipes)
            {
                await _enrichmentService.Enrich(recipe);
            }

            return recipes.AsQueryable();
        }
    }
}
