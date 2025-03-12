using Microsoft.AspNetCore.Mvc;
using RecipeDiscovery.Services;
using RecipeDiscovery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.Controllers
{
    [ApiController]
    [Route("recipes")] // Defines the base route for the controller as "/recipes"
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService; // Service dependency for handling recipe-related logic

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService; // Injecting the recipe service for data retrieval
        }

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetRecipes(
            [FromQuery] string? cuisine = null,  // Optional filter for cuisine type
            [FromQuery] string? ingredient = null,  // Optional filter for ingredients
            [FromQuery] string? sortBy = null,  // Optional sorting field
            [FromQuery] string order = "asc",  // Sorting order: "asc" or "desc", default is ascending
            [FromQuery] int page = 1,  // Default pagination starts at page 1
            [FromQuery] int pageSize = 10  // Default number of items per page
        )
        {
            // Fetch all recipes from the external API
            var recipes = await _recipeService.GetAllRecipes("");

            // Apply cuisine filter if specified
            if (!string.IsNullOrEmpty(cuisine))
            {
                recipes = recipes.Where(r => r.Cuisine.Equals(cuisine, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply ingredient filter if specified
            if (!string.IsNullOrEmpty(ingredient))
            {
                recipes = recipes.Where(r => r.Ingredients != null &&
                    r.Ingredients.Any(i => i.Contains(ingredient, System.StringComparison.OrdinalIgnoreCase))).ToList();
            }

            // Apply sorting if sortBy parameter is provided
            if (!string.IsNullOrEmpty(sortBy))
            {
                // Sorting based on the requested field
                if (sortBy.ToLower() == "name")
                {
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
                else if (sortBy.ToLower() == "cuisine")
                {
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Cuisine).ToList()
                        : recipes.OrderBy(r => r.Cuisine).ToList();
                }
                else if (sortBy.ToLower() == "preparationtime")
                {
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.PreparationTime).ToList()
                        : recipes.OrderBy(r => r.PreparationTime).ToList();
                }
                else
                {
                    // Default to sorting by name if invalid sortBy field is provided
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
            }

            // Apply pagination by skipping items from previous pages and taking the required count
            var pagedRecipes = recipes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(pagedRecipes); // Return the final list of filtered, sorted, and paginated recipes
        }

        [HttpGet("{id}")] // Endpoint for fetching a specific recipe by ID
        public async Task<IActionResult> GetRecipeById(string id)
        {
            // Validate ID format (should be exactly 5 digits and numeric)
            // We want to check this before calling GetRecipeById so we dont waste the call on an invalid id
            if (string.IsNullOrEmpty(id) || id.Length != 5 || !id.All(char.IsDigit))
            {
                return BadRequest("Invalid recipe ID");
            }

            var recipe = await _recipeService.GetRecipeById(id);

            // If the recipe is not found, return 404 Not Found response
            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }

            return Ok(recipe); // Return the found recipe
        }
    }
}
