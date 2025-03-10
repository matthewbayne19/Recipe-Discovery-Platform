using RecipeDiscovery.Models;
using RecipeDiscovery.Services;
using HotChocolate;

namespace RecipeDiscovery.GraphQL
{
    public class Query
    {
        // Define the resolver for the 'GetRecipes' query
        public async Task<List<Recipe>> GetRecipes([Service] IRecipeService recipeService)
        {
            // Call the RecipeService to get the recipes (passing an empty string for now)
            return await recipeService.SearchRecipesAsync(""); 
        }
    }
}
