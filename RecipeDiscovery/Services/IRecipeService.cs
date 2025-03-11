using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public interface IRecipeService
    {
        // Method to get all recipes with the option to filter by cuisine
        Task<List<Recipe>> GetAllRecipes(string query);

        // Method to get a recipe by ID
        Task<Recipe?> GetRecipeById(string id);

    }
}
