//this file is the data model for the recipe
//consists of all the data points related to a recipe

namespace RecipeDiscovery.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public List<string> FavoriteRecipes { get; set; } = new();
    }
}
