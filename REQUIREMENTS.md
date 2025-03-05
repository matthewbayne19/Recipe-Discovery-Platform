## Requirements
Leverage the free recipe api at https://www.themealdb.com/api.php to source recipes. 

### Data Model
- **Recipe**: ID, Name, Description, Ingredients (list), Cuisine, Preparation Time, Difficulty Level.
- **User**: ID, Username, Favorite Recipes (list of Recipe IDs).

### API Features
#### REST API
- `GET /recipes` - List recipes with optional filters (e.g., `?cuisine=Italian`).
- `GET /recipes/{id}` - Get a single recipe by ID.
- `POST /users/{userId}/favorites` - Add a recipe to a user’s favorites (e.g., JSON payload with `recipeId`).
- `GET /users/{userId}/favorites` - List a user’s favorite recipes.

#### GraphQL API (using HotChocolate)
- **Query**: Fetch all recipes with optional filters (e.g., cuisine, ingredient).
- **Query**: Fetch a single recipe by ID, including nested ingredients.
- **Query**: Fetch a user’s favorite recipes.
- **Mutation**: Add a recipe to a user’s favorites.

### Technical Requirements
- Use .NET (preferably 6 or later) with HotChocolate for GraphQL.
- Implement both REST and GraphQL in the same application.
- Use dependency injection for service layers.
- Include basic error handling (e.g., 404 for missing resources).
- Data source: Either integrate a free public API (see below) or mock an in-memory store.

### Bonus Features (Optional)
- Add sorting or pagination to recipe queries.
- Implement simple authentication (e.g., API key or JWT simulation).
- Write unit tests for at least one REST endpoint and one GraphQL resolver.

## Bonus: GraphQL Orchestration Layer
Take it to the next level by using GraphQL as the entry point to orchestrate calls to a public REST API (e.g., TheMealDB). Enrich the recipe data with additional info (e.g., nutrition) by chaining API calls in resolvers.

### Example Workflow
- Client queries recipes with nutrition data.
- GraphQL fetches basic recipe data from TheMealDB REST API.
- Resolvers fetch additional data (e.g., nutrition from another source or mock) per recipe.
- Returns a unified response to the client.

### Example Schema
```graphql
type Recipe {
  id: ID!
  name: String!
  ingredients: [Ingredient!]!
  cuisine: String!
  nutrition: Nutrition
}

type Nutrition {
  calories: Float
  protein: Float
}

type Query {
  recipes(search: String): [Recipe!]!
}