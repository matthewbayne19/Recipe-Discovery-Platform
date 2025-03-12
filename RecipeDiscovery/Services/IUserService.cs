using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    // Interface for the User Service
    // Defines the contract for managing user favorite recipes
    public interface IUserService
    {
        // Method to get the list of favorite recipe IDs for a given user
        // Returns an empty list if the user has no favorites
        Task<List<string>> GetUserFavorites(string userId);

        // Method to add a recipe to a user's favorites list
        // Ensures that duplicate entries are not added
        Task AddUserFavorite(string userId, string recipeId);
    }
}
