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

        // Mutation to add a recipe to user's favorites
        public async Task<string> AddFavoriteRecipe(string userId, string recipeId)
        {
            // Validate that both userId and recipeId are provided
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(recipeId))
            {
                return "User ID and Recipe ID are required."; // Return an error if either ID is missing
            }

            // Validate the recipe ID format, expecting a 5-digit numeric string
            if (recipeId.Length != 5 || !recipeId.All(char.IsDigit))
            {
                return "Invalid recipe ID."; // Return an error if the recipe ID doesn't meet the format
            }

            // Check if the recipe exists in the database using the recipe service
            var recipe = await _recipeService.GetRecipeById(recipeId);
            if (recipe == null)
            {
                return "Recipe not found."; // Return an error if the recipe ID doesn't match any existing recipe
            }

            // Check if the recipe is already in the user's favorites
            var favorites = await _userService.GetUserFavorites(userId);
            if (favorites.Any(f => f == recipeId)) 
            {
                return "Recipe is already in favorites."; // Return a message if the recipe is already in the favorites list
            }

            // Add the recipe to the user's favorites list
            await _userService.AddUserFavorite(userId, recipeId); 
            return "Recipe added to favorites successfully."; // Return a success message after adding to favorites
        }
    }
}
