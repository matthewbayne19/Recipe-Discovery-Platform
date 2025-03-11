using RecipeDiscovery.Models;

namespace RecipeDiscovery.Services
{
    public interface IRecipeService
    {
        //method to get all recipes with the option to filter by cuisine
        Task<List<Recipe>> GetAllRecipes(string query);

        //method to get a recipe by id
        Task<Recipe?> GetRecipeById(string id);
    }
}
