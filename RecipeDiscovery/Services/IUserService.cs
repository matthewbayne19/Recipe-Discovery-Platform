using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public interface IUserService
    {
        //method to get favorite recipe list of a user
        Task<List<string>> GetUserFavorites(string userId);

        //method to add a recipe to a users favorites lists
        Task AddUserFavorite(string userId, string recipeId);
    }
}
