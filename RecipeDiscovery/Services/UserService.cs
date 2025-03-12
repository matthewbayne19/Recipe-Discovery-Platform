using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, List<string>> userFavorites = new();
        private readonly IRecipeService _recipeService;

        public UserService(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /*
        // Get favorite recipes COMPLETE for a user (not using because we are just returning the ids, but could use this to have full recipes)
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

            return new List<Recipe>(); //return empty list, bad API practice to change response structure based on content (ie. adding no favs message)
        }
        */

        // Get the list of recipe ids in the favorites list
        public Task<List<string>> GetUserFavorites(string userId)
        {
            return Task.FromResult(userFavorites.ContainsKey(userId) ? userFavorites[userId] : new List<string>());
        }

        // Add a recipe to a user's favorites
        public Task AddUserFavorite(string userId, string recipeId)
        {
            if (!userFavorites.ContainsKey(userId))
            {
                userFavorites[userId] = new List<string>();
            }

            if (!userFavorites[userId].Contains(recipeId))
            {
                userFavorites[userId].Add(recipeId);
            }

            return Task.CompletedTask;
        }
    }
}
