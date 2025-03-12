// This file defines the data model for the favorite recipe payload,
// which is used when adding a recipe to a user's favorites.

namespace RecipeDiscovery.Models
{
    public class FavoriteRecipePayload
    {
        // The ID of the recipe that the user wants to add to favorites.
        public required string RecipeId { get; set; }
    }
}