using RecipeDiscovery.Services;
using System.Threading.Tasks;

namespace RecipeDiscovery.GraphQL
{
    public class Mutation
    {
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;

        public Mutation(IUserService userService, IRecipeService recipeService)
        {
            _userService = userService;
            _recipeService = recipeService;
        }

        // Mutation to add a recipe to user's favorites
        public async Task<string> AddFavoriteRecipe(string userId, string recipeId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(recipeId))
            {
                return "User ID and Recipe ID are required.";
            }

            // Validate the recipe ID format (should be a 5-digit number)
            if (recipeId.Length != 5 || !recipeId.All(char.IsDigit))
            {
                return "Invalid recipe ID.";
            }

            var recipe = await _recipeService.GetRecipeById(recipeId);
            if (recipe == null)
            {
                return "Recipe not found.";
            }

            var favorites = await _userService.GetUserFavorites(userId);
            if (favorites.Any(f => f == recipeId)) 
            {
                return "Recipe is already in favorites.";
            }

            await _userService.AddUserFavorite(userId, recipeId); 
            return "Recipe added to favorites successfully.";
        }
    }
}
