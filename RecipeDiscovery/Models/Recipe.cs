//this file is the data model for the recipe
//consists of all the data points related to a recipe

namespace RecipeDiscovery.Models
{
    public class Recipe
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public string Cuisine { get; set; } = string.Empty;
        public string PreparationTime { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
    }
}
