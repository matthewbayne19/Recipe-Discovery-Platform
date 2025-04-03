import React from "react";
import { useParams } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { CircularProgress, Typography, Box } from "@mui/material";
import { GET_RECIPE_BY_ID } from "../api/graphql"; // Importing query

const RecipePage = () => {
  const { id } = useParams();

  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
  });

  console.log("USED GRAPHQL TO GET RECIPE DETAILS")

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
