using RecipeDiscovery.Models;

// Interface for the Enrichment Service
// Defines a contract for enriching recipe data with additional details (e.g., nutrition, preparation time, difficulty level).
public interface IEnrichmentService
{
    // This method will provide mock enrichment data to supplement missing fields from TheMealDB API.
    Task<Recipe> Enrich(Recipe recipe);
}
