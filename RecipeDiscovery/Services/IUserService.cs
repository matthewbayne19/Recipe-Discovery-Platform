using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeDiscovery.Services
{
    public interface IUserService
    {
        // Retrieve a list of favorite recipe IDs for a user
        Task<List<string>> GetUserFavorites(string userId);

        // Toggle a recipe in the user's favorites
        Task<bool> ToggleFavorite(string userId, string recipeId);
    }
}
