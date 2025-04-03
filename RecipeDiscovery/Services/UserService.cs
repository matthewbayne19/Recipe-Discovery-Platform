using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.Services
{
    public class UserService : IUserService
    {
        private Dictionary<string, List<string>> userFavorites = new();

        public Task<List<string>> GetUserFavorites(string userId)
        {
            return Task.FromResult(userFavorites.ContainsKey(userId) ? userFavorites[userId] : new List<string>());
        }

        public Task<bool> ToggleFavorite(string userId, string recipeId)
        {
            if (!userFavorites.ContainsKey(userId))
            {
                userFavorites[userId] = new List<string>();
            }

            if (userFavorites[userId].Contains(recipeId))
            {
                userFavorites[userId].Remove(recipeId);
                return Task.FromResult(false);  // Return false when recipe is removed
            }
            else
            {
                userFavorites[userId].Add(recipeId);
                return Task.FromResult(true);  // Return true when recipe is added
            }
        }
    }
}
