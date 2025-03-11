using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public interface IRecipeService
    {
        Task<List<Recipe>> SearchRecipesAsync(string query);

        //method to get a recipe by id
        Task<Recipe?> GetRecipeByIdAsync(string id);
    }
}
