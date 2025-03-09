namespace RecipeDiscovery.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public string Cuisine { get; set; } = string.Empty;
        public int PreparationTime { get; set; }
        public string DifficultyLevel { get; set; } = string.Empty;
    }
}
