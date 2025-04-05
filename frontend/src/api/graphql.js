import { gql } from "@apollo/client";

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
      nutrition {
        calories
        protein
      }
    }
  }
`;

export const TOGGLE_FAVORITE_MUTATION = gql`
  mutation ToggleFavoriteRecipe($recipeId: String!) {
    toggleFavoriteRecipe(userId: "1", recipeId: $recipeId)
  }
`;

export const GET_USER_FAVORITES = gql`
  query GetUserFavorites {
    userFavorites(userId: "1")
  }
`;
