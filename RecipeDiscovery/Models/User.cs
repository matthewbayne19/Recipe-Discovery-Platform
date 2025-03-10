//this file is the data model for the recipe
//consists of all the data points related to a recipe

namespace RecipeDiscovery.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<int> FavoriteRecipes { get; set; } = new();
    }
}
