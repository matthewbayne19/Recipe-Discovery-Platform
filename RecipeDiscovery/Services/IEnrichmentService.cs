using RecipeDiscovery.Models;

public interface IEnrichmentService
{
    Task<Recipe> Enrich(Recipe recipe);
}