using RecipeDiscovery.Services;
using System.Threading.Tasks;

namespace RecipeDiscovery.GraphQL
{
    public class Mutation
    {
        private readonly IUserService _userService; // Service to manage user data and favorites
        private readonly IRecipeService _recipeService; // Service to fetch recipe details

        public Mutation(IUserService userService, IRecipeService recipeService)
        {
            _userService = userService; // Injecting the IUserService to handle favorites
            _recipeService = recipeService; // Injecting the IRecipeService to validate and retrieve recipes
        }

        // Mutation to toggle a recipe in user's favorites
        public async Task<string> ToggleFavoriteRecipe(string userId, string recipeId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(recipeId))
            {
                return "User ID and Recipe ID are required.";
            }

            var recipe = await _recipeService.GetRecipeById(recipeId);
            if (recipe == null)
            {
                return "Recipe not found.";
            }

            // Toggle the recipe in the user's favorites
            await _userService.ToggleFavorite(userId, recipeId);
            var favorites = await _userService.GetUserFavorites(userId);
            if (favorites.Contains(recipeId))
            {
                return "Recipe added to favorites.";
            }
            else
            {
                return "Recipe removed from favorites.";
            }
        }
    }
}
