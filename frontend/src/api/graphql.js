import { gql } from "@apollo/client";

// Query for a single recipe by ID
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
      imageUrl
    }
  }
`;

// Mutation to add a recipe to user's favorites
export const ADD_FAVORITE_MUTATION = gql`
  mutation AddFavorite($recipeId: String!) {
    addFavorite(userId: "1", recipeId: $recipeId)
  }
`;

// Query to get user's favorite recipe IDs
export const GET_USER_FAVORITES = gql`
  query GetUserFavorites {
    userFavorites(userId: "1")
  }
`;
