import { gql } from "@apollo/client";

// Example: query for a recipe by ID
export const GET_RECIPE_BY_ID = gql`
  query GetRecipeById($id: String!) {
    recipeById(id: $id) {
      id
      name
      description
      cuisine
      preparationTime
      difficultyLevel
      ingredients
    }
  }
`;