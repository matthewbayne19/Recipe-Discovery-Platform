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
                return Ok(new List<string>()); // Return an empty list with OK status - shouldnt be an error to have an empty list (new user)
            }

            return Ok(favorites);
        }

        // Endpoint to add a recipe to the user's favorites
        [HttpPost("favorites")]
        public async Task<IActionResult> AddFavorite(string userId, [FromBody] string recipeId)
        {
            // Check to see if we have a user id
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            // Check to see if we have a recipe id
            if (string.IsNullOrEmpty(recipeId))
            {
                return BadRequest("Recipe ID is required.");
            }

            // Check if the recipe exists before adding to favorites
            var recipe = await _recipeService.GetRecipeById(recipeId);  
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            // Proceed to add to favorites
            try
            {
                await _userService.AddUserFavorite(userId, recipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Successful operation, no content returned
        }
    }
}
