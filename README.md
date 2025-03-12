### Running the Project
1. Install dependencies
   ```bash
   dotnet restore
   ```

2. Run the project
   ```bash
   dotnet run
   ```
   The app will start at `http://localhost:5011`.

3. Access Swagger UI (REST API)
   Open `http://localhost:5011/swagger`

4. Access GraphQL Playground (Nitro)
   Open `http://localhost:5011/graphql`

### API Key Authentication
- Every request must include an API key in the headers:
  ```
  X-API-KEY: simple-api-key
  ```

## GraphQL Usage
### Fetch All Recipes with Sorting & Pagination
```graphql
query {
  recipes(page: 1, pageSize: 5, sortBy: "name", order: "asc") {
    id
    name
    cuisine
    preparationTime
    difficultyLevel
    nutrition {
      calories
      protein
    }
  }
}
```

### Fetch a Recipe by ID
```graphql
query {
  recipeById(id: "53086") {
    id
    name
    description
    ingredients
    cuisine
    preparationTime
    difficultyLevel
    nutrition {
      calories
      protein
    }
  }
}
```

### Add a Recipe to User Favorites
```graphql
mutation {
  addFavoriteRecipe(userId: "user123", recipeId: "53086") 
}
```

### Fetch User's Favorite Recipe IDs
```graphql
query {
  userFavorites(userId: "user123")
}
```

## Running Unit Tests
The project includes **unit tests** for both **REST API** and **GraphQL** functionalities.

### Prerequisites
- Ensure the project builds successfully before running tests.
- API Key authentication is required; tests have been updated to include the API key.

### Running All Tests
Use the following command to run all unit tests:
```bash
cd RecipeDiscovery.Tests
```
```bash
dotnet test
```

### Running a Specific Test File
To run only **REST API tests**:
```bash
dotnet test --filter FullyQualifiedName=RecipeDiscovery.Tests.RestApiTests
```

To run only **GraphQL tests**:
```bash
dotnet test --filter FullyQualifiedName=RecipeDiscovery.Tests.GraphQLTests
```

### Test Cases Covered
- **REST API Tests**
  - Fetch all recipes (`GET /recipes`)
  - Fetch a single recipe by ID (`GET /recipes/{id}`)

- **GraphQL Tests**
  - Add a recipe to user favorites (`mutation addFavoriteRecipe`)
  - Fetch user's favorite recipes (`query userFavorites`)

## Explanation of Design Choices
### GraphQL Orchestration
- Part of the GraphQL implementation serves as an orchestration layer, fetching data from TheMealDB and enriching it with additional details like **nutrition, preparation time, and difficulty level**.
- The `EnrichmentService` handles adding mock enrichment data to recipes.

### Sorting & Pagination
- Implemented in both REST and GraphQL.
- Sorting can be done on fields like `name`, `cuisine`, and `preparationTime`.
- Pagination allows fetching a subset of data for performance optimization.

### API Key Authentication
- Implemented via middleware to require an API key for all requests.
- Prevents unauthorized access to the platform.

### User Favorites
- Stored in-memory for this implementation (using a dictionary). Registered as singleton service to persist data.
- Only stores recipe IDs to reduce data redundancy.

### Unit Testing
- Includes **REST API tests** and **GraphQL tests**.
- Tests for adding a recipe to favorites and retrieving the favorites list.

### Comments
- Additional explanations can be found throughout code comments.
```