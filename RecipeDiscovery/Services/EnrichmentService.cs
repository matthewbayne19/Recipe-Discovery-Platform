using RecipeDiscovery.Models;
using System.Threading.Tasks;

namespace RecipeDiscovery.Services
{
    // Service responsible for enriching recipe data with additional details.
    // Since TheMealDB does not provide some fields (e.g., nutrition, preparation time, difficulty level),
    // this service fills in those missing fields with mock data.
    public class EnrichmentService : IEnrichmentService
    {
        // Enriches the provided recipe with additional mock data.
        public Task<Recipe> Enrich(Recipe recipe)
        {
            // Mock enrichment data for nutrition details, prep time, and difficulty
            recipe.Nutrition = new Nutrition
            {
                Calories = 200,  
                Protein = 10    
            };

            recipe.PreparationTime = "30 minutes";  
            recipe.DifficultyLevel = "Medium";  

            // Return the enriched recipe wrapped in a Task to maintain async compatibility
            return Task.FromResult(recipe);
        }
    }
}
