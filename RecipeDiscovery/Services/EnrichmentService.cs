using RecipeDiscovery.Models;
using System.Threading.Tasks;

namespace RecipeDiscovery.Services
{
    public class EnrichmentService : IEnrichmentService
    {
        public Task<Recipe> Enrich(Recipe recipe)
        {
            // Mock enrichment data for nutrition
            recipe.Nutrition = new Nutrition
            {
                Calories = 200,  // Example mock data
                Protein = 10     // Example mock data
            };

            // Mock enrichment data for preparation time
            recipe.PreparationTime = "30 minutes";  // Example mock data

            // Mock enrichment data for difficulty level
            recipe.DifficultyLevel = "Medium";  // Example mock data

            // Return the enriched recipe as a Task
            return Task.FromResult(recipe);
        }
    }
}
