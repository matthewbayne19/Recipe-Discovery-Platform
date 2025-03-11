using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public class UserService : IUserService
    {
        // Mocked in-memory store for user favorites
        private readonly Dictionary<string, List<string>> userFavorites = new();

        // Get favorite recipes for a user
        public Task<List<Recipe>> GetUserFavorites(string userId)
        {
            if (userFavorites.ContainsKey(userId))
            {
                // Get the list of favorite recipe IDs and fetch their details
                var favoriteRecipeIds = userFavorites[userId];
                var favorites = favoriteRecipeIds.Select(id => new Recipe
                {
                    Id = id,
                    Name = $"Recipe {id}", // Placeholder, replace with actual fetch logic
                    Description = $"Description for {id}",
                    Ingredients = new List<string>(), // Placeholder
                    Cuisine = "Unknown", // Placeholder
                    PreparationTime = "Unknown", // Placeholder
                    DifficultyLevel = "Unknown" // Placeholder
                }).ToList();
                return Task.FromResult(favorites);
            }
            return Task.FromResult(new List<Recipe>());
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
