using Microsoft.AspNetCore.Mvc;
using RecipeDiscovery.Services;
using RecipeDiscovery.Models;

namespace RecipeDiscovery.Controllers
{
    [Route("users/{userId}")] // Base route for the controller
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;

        public UserController(IUserService userService, IRecipeService recipeService)
        {
            _userService = userService;
            _recipeService = recipeService;
        }

        // Endpoint to get a user's favorite recipes
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites(string userId)
        {
            // Check to see if we have a user id
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var favorites = await _userService.GetUserFavorites(userId);

            // Check if the user's favorites list is empty
            if (favorites == null || !favorites.Any())
            {
                // Return an empty list with OK status - shouldn't be an error to have an empty list (new user)
                return Ok(new List<string>());
            }

            return Ok(favorites);
        }

        // Endpoint to add a recipe to the user's favorites
        [HttpPost("favorites")]
        public async Task<IActionResult> AddFavorite(string userId, [FromBody] FavoriteRecipePayload payload)
        {
            // Check to see if we have a user id
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            // Check to see if we have a recipe id
            if (payload == null || string.IsNullOrEmpty(payload.RecipeId))
            {
                return BadRequest("Recipe ID is required.");
            }

            // Validate the recipe ID format (should be a 5-digit number)
            if (payload.RecipeId.Length != 5 || !payload.RecipeId.All(char.IsDigit))
            {
                return BadRequest("Invalid recipe ID.");
            }

            // Check if the recipe exists before adding to favorites
            var recipe = await _recipeService.GetRecipeById(payload.RecipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            // Check if the recipe is already in the user's favorites
            var favorites = await _userService.GetUserFavorites(userId);
            if (favorites.Any(f => f.Id == payload.RecipeId)) // Compare Recipe Ids
            {
                return Ok(new { message = "Recipe is already in favorites." });
            }

            // Proceed to add to favorites
            try
            {
                await _userService.AddUserFavorite(userId, payload.RecipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(new { message = "Recipe added to favorites successfully." });
        }
    }
}
