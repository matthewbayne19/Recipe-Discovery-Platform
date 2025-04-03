import React from "react";
import { useParams } from "react-router-dom";
import { gql, useQuery } from "@apollo/client";
import { CircularProgress, Typography, Box } from "@mui/material";

const GET_RECIPE_BY_ID = gql`
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

const RecipePage = () => {
  const { id } = useParams();

  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
  });

  if (loading) return <CircularProgress />;
  if (error) return <Typography>Error loading recipe.</Typography>;

  const recipe = data.recipeById;

  return (
    <Box p={3}>
      <Typography variant="h4">{recipe.name}</Typography>
      <Typography variant="subtitle1" gutterBottom>
        Cuisine: {recipe.cuisine} | Prep Time: {recipe.preparationTime} | Difficulty: {recipe.difficultyLevel}
      </Typography>
      <Typography variant="body1" paragraph>
        {recipe.description}
      </Typography>
      <Typography variant="h6">Ingredients:</Typography>
      <ul>
        {recipe.ingredients.map((ing, index) => (
          <li key={index}>{ing}</li>
        ))}
      </ul>
    </Box>
  );
};

export default RecipePage;
