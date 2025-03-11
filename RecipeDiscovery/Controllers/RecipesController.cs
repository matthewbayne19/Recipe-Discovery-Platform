using Microsoft.AspNetCore.Mvc;
using RecipeDiscovery.Services;
using RecipeDiscovery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeDiscovery.Controllers
{
    [ApiController]
    [Route("recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetRecipes(
            [FromQuery] string? cuisine = null,
            [FromQuery] string? ingredient = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string order = "asc", // Default order is ascending
            [FromQuery] int page = 1,        // Default page is 1
            [FromQuery] int pageSize = 10    // Default page size is 10
        )
        {
            // Fetch all recipes
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

            // Apply sorting if sortBy is specified
            if (!string.IsNullOrEmpty(sortBy))
            {
                // Check the sortBy field and apply the corresponding sorting logic
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
                    // If the sortBy doesn't match any predefined fields, fallback to sorting by name
                    recipes = order.ToLower() == "desc"
                        ? recipes.OrderByDescending(r => r.Name).ToList()
                        : recipes.OrderBy(r => r.Name).ToList();
                }
            }

            // Apply pagination: Skip the results for previous pages and take the pageSize number of recipes
            var pagedRecipes = recipes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Return the filtered, sorted, and paginated list of recipes
            return Ok(pagedRecipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(string id)
        {
            var recipe = await _recipeService.GetRecipeById(id);

            if (string.IsNullOrEmpty(id) || id.Length != 5 || !id.All(char.IsDigit))
            {
                return BadRequest("Invalid recipe ID");
            }

            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }

            return Ok(recipe);
        }
    }
}
