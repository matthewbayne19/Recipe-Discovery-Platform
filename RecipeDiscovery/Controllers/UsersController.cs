using Microsoft.AspNetCore.Mvc;
using RecipeDiscovery.Services;
using RecipeDiscovery.Models;

namespace RecipeDiscovery.Controllers
{
    // Controller for user-specific actions like managing favorites
    [Route("users/{userId}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;

        public UserController(IUserService userService, IRecipeService recipeService)
        {
            _userService = userService; // Dependency injection of user service
            _recipeService = recipeService; // Dependency injection of recipe service
        }

        // GET endpoint to retrieve a user's favorite recipes
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var favorites = await _userService.GetUserFavorites(userId);
            if (!favorites.Any())
            {
                return Ok(new List<string>()); // Return empty list if no favorites
            }

            return Ok(favorites); // Return list of favorite recipe IDs
        }

        // POST endpoint to toggle a favorite recipe for a user
        [HttpPost("favorites/toggle")]
        public async Task<IActionResult> ToggleFavorite(string userId, [FromBody] FavoriteRecipePayload payload)
        {
            if (string.IsNullOrEmpty(userId) || payload == null || string.IsNullOrEmpty(payload.RecipeId))
            {
                return BadRequest("Invalid input.");
            }

            var recipe = await _recipeService.GetRecipeById(payload.RecipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            var result = await _userService.ToggleFavorite(userId, payload.RecipeId);
            return Ok(result ? "Recipe toggled in favorites successfully." : "Failed to toggle recipe in favorites.");
        }
    }
}
