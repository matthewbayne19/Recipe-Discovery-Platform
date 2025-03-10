using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public interface IRecipeService
    {
        Task<List<Recipe>> SearchRecipesAsync(string query);
    }
}
