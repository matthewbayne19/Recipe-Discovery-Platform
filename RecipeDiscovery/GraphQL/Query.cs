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

        // Query to get all recipes with optional filters and pagination
        public async Task<List<Recipe>> GetRecipes(
            string? cuisine = null,
            string? ingredient = null,
            string? sortBy = null,
            string order = "asc", // Default order is ascending
            int page = 1,        // Default page is 1
            int pageSize = 10    // Default page size is 10
        )
        {
            // Fetch all recipes
            var recipes = await _recipeService.GetAllRecipes("");

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

            // Apply sorting if sortBy is specified
            if (!string.IsNullOrEmpty(sortBy))
            {
                // Check the sortBy field and apply the corresponding sorting logic
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
                    // If the sortBy doesn't match any predefined fields, fallback to sorting by name
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
            }

            // Apply pagination: Skip the results for previous pages and take the pageSize number of recipes
            var pagedRecipes = recipes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Return the filtered, sorted, and paginated list of recipes
            return pagedRecipes;
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
