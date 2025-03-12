// This file defines the Recipe model, which represents a recipe with various attributes.
// It includes basic details as well as enriched information such as preparation time, 
// difficulty level, and nutrition.

using System.Collections.Generic;

namespace RecipeDiscovery.Models
{
    public class Recipe
    {
        // Unique identifier for the recipe, sourced from TheMealDB API.
        public string Id { get; set; } = string.Empty;

        // Name of the recipe.
        public string Name { get; set; } = string.Empty;

        // Detailed instructions on how to prepare the recipe.
        public string Description { get; set; } = string.Empty;

        // List of ingredients required for the recipe.
        public List<string> Ingredients { get; set; } = new();

        // The cuisine type associated with the recipe (e.g., Italian, Mexican).
        public string Cuisine { get; set; } = string.Empty;

        // Preparation time for the recipe.
        // TheMealDB API does not provide this, enriched with mock data.
        public string PreparationTime { get; set; } = string.Empty;

        // Difficulty level of the recipe.
        // TheMealDB API does not provide this, enriched with mock data.
        public string DifficultyLevel { get; set; } = string.Empty;

        // Nutritional information for the recipe.
        // TheMealDB API does not provide this, enriched with mock data.
        public Nutrition Nutrition { get; set; } = new Nutrition();
    }
}
