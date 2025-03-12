// This file defines the User model, which represents a user in the system.
// It includes user details and a list of favorite recipe IDs.

namespace RecipeDiscovery.Models
{
    public class User
    {
        // Unique identifier for the user. 
        // This is stored as a string rather than a GUID for simplicity.
        public string Id { get; set; } = string.Empty;

        // List of favorite recipe IDs that the user has saved.
        // Only the IDs are stored as per requirements.
        public List<string> FavoriteRecipes { get; set; } = new();
    }
}
