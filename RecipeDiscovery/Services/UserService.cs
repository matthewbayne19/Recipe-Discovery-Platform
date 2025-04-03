using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    // Service for managing user favorite recipes
    public class UserService : IUserService
    {
        // In-memory dictionary to store users and their favorite recipe IDs
        private readonly Dictionary<string, List<string>> userFavorites = new(); //O(1)
        private readonly IRecipeService _recipeService; // Service to fetch recipe details if needed

        public UserService(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /*
        // Alternative method: Get full recipe details instead of just IDs
        public async Task<List<Recipe>> GetUserFavorites(string userId)
        {
            if (userFavorites.ContainsKey(userId))
            {
                // Get the list of favorite recipe IDs and fetch their details
                var favoriteRecipeIds = userFavorites[userId];
                var favoriteRecipes = new List<string>();

                // Fetch full details for each favorite recipe ID
                foreach (var recipeId in favoriteRecipeIds)
                {
                    var recipe = await _recipeService.GetRecipeById(recipeId);
                    if (recipe != null)
                    {
                        favoriteRecipes.Add(recipe); // Add the complete recipe to the list
                    }
                }

                return favoriteRecipes;
            }

            return new List<Recipe>(); // Return an empty list if no favorites exist
        }
        */

        // Returns a list of recipe IDs that the user has favorited
        public Task<List<string>> GetUserFavorites(string userId)
        {
            // Return their favorite recipe IDs, otherwise return an empty list
            return Task.FromResult(userFavorites.ContainsKey(userId) ? userFavorites[userId] : new List<string>());
        }

        // Adds a recipe to the user's favorites
        public Task AddUserFavorite(string userId, string recipeId)
        {
            // If the user doesn't have a favorites list yet, create one
            if (!userFavorites.ContainsKey(userId))
            {
                userFavorites[userId] = new List<string>();
            }

            // Add the recipe ID to the favorites list if it isn't already present
            if (!userFavorites[userId].Contains(recipeId))
            {
                userFavorites[userId].Add(recipeId);
            }

            return Task.CompletedTask;
        }
    }
}
