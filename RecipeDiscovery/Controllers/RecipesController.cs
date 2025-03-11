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
            [FromQuery] string? ingredient = null)
        {
            // Fetch all recipes
            var recipes = await _recipeService.GetAllRecipes("");

            // Apply filters based on query parameters
            if (!string.IsNullOrEmpty(cuisine))
            {
                recipes = recipes.FindAll(r => r.Cuisine.Equals(cuisine, System.StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(ingredient))
            {
                recipes = recipes.FindAll(r => r.Ingredients != null &&
                    r.Ingredients.Any(i => i.Contains(ingredient, System.StringComparison.OrdinalIgnoreCase)));
            }

            // Return the filtered list of recipes
            return Ok(recipes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(string id) // Async method
        {
            var recipe = await _recipeService.GetRecipeById(id); // Call async method

            if (recipe == null)
            {
                return NotFound(new { message = "Recipe not found" });
            }

            return Ok(recipe); // Return the recipe
        }
    }
}
