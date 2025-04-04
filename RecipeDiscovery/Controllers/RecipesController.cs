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
        public async Task<ActionResult<object>> GetRecipes(
            [FromQuery] string? cuisine = null,
            [FromQuery] string? ingredient = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var recipes = await _recipeService.GetAllRecipes("");

            // Apply cuisine filter (starts with)
            if (!string.IsNullOrEmpty(cuisine))
            {
                recipes = recipes.Where(r => r.Cuisine != null &&
                    r.Cuisine.StartsWith(cuisine, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply ingredient filter (starts with)
            if (!string.IsNullOrEmpty(ingredient))
            {
                recipes = recipes.Where(r => r.Ingredients != null &&
                    r.Ingredients.Any(i => i.StartsWith(ingredient, System.StringComparison.OrdinalIgnoreCase))).ToList();
            }

            // Store total after filtering, before pagination
            int totalCount = recipes.Count;

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
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
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
            }

            // Pagination
            var pagedRecipes = recipes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return wrapped response with total count
            return Ok(new
            {
                recipes = pagedRecipes,
                totalCount = totalCount
            });
        }

        [HttpGet("{id}")] // Endpoint for fetching a specific recipe by ID
        public async Task<IActionResult> GetRecipeById(string id)
        {
            // Validate ID format (should be exactly 5 digits and numeric)
            // We want to check this before calling GetRecipeById so we don't waste the call on an invalid id
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

        [HttpGet("search")] // Endpoint for fetching recipes by searching the name
        public async Task<IActionResult> GetRecipesByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Search query is required.");
            }

            var recipes = await _recipeService.GetRecipesByName(name);
            return Ok(recipes); // Always return Ok, even if the list is empty
        }
    }
}
