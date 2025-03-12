using Microsoft.AspNetCore.Mvc;
using RecipeDiscovery.Services;
using RecipeDiscovery.Models;

namespace RecipeDiscovery.Controllers
{
    [Route("users/{userId}")] // Base route for the user-related endpoints, userId is part of the route
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService; // Service handling user data and favorites
        private readonly IRecipeService _recipeService; // Service for retrieving recipe details

        public UserController(IUserService userService, IRecipeService recipeService)
        {
            _userService = userService; // Injecting user service
            _recipeService = recipeService; // Injecting recipe service for recipe validation
        }

        // Endpoint to get a user's favorite recipes
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites(string userId)
        {
            // Validate userId parameter
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var favorites = await _userService.GetUserFavorites(userId);

            // If the user has no favorite recipes, return an empty list with an OK response
            if (favorites == null || !favorites.Any())
            {
                return Ok(new List<string>()); // Returning an empty list follows best practices for consistency
            }

            return Ok(favorites); // Return the list of favorite recipe IDs
        }

        // Endpoint to add a recipe to the user's favorites
        [HttpPost("favorites")]
        public async Task<IActionResult> AddFavorite(string userId, [FromBody] FavoriteRecipePayload payload)
        {
            // Validate userId
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            // Validate payload and recipeId
            if (payload == null || string.IsNullOrEmpty(payload.RecipeId))
            {
                return BadRequest("Recipe ID is required.");
            }

            // Validate the recipe ID format (should be exactly 5 digits and numeric)
            // We want to check this before calling GetRecipeById so we dont waste the call on an invalid id
            if (payload.RecipeId.Length != 5 || !payload.RecipeId.All(char.IsDigit))
            {
                return BadRequest("Invalid recipe ID.");
            }

            // Check if the recipe exists before adding to favorites
            var recipe = await _recipeService.GetRecipeById(payload.RecipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found."); // Ensures users can't add non-existent recipes
            }

            // Check if the recipe is already in the user's favorites
            var favorites = await _userService.GetUserFavorites(userId);
            if (favorites.Any(f => f == payload.RecipeId)) // Compare IDs instead of objects
            {
                return Ok(new { message = "Recipe is already in favorites." });
            }

            // Proceed to add the recipe to the user's favorites
            try
            {
                await _userService.AddUserFavorite(userId, payload.RecipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return error in case of an unexpected failure
            }

            return Ok(new { message = "Recipe added to favorites successfully." });
        }
    }
}
