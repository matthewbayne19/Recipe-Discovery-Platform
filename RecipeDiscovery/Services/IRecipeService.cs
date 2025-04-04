using RecipeDiscovery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeDiscovery.Services
{
    // Interface for the Recipe Service
    // Defines the contract for retrieving recipe data from an external API (TheMealDB)
    public interface IRecipeService
    {
        // Method to retrieve all recipes, with an optional search query
        // The query parameter allows filtering recipes by a search keyword
        Task<List<Recipe>> GetAllRecipes(string query);

        // Method to fetch a single recipe by its unique ID
        // Returns null if the recipe does not exist
        Task<Recipe?> GetRecipeById(string id);

        // Method to retrieve recipes based on a search term for the name
        // Returns a list of recipes that match the search term
        Task<List<Recipe>> GetRecipesByName(string name);
    }
}
