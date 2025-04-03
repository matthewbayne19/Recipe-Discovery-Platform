import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { CircularProgress, Typography, Box } from "@mui/material";
import { GET_RECIPE_BY_ID } from "../api/graphql";

const RecipePage = () => {
  const { id } = useParams();
  const [cachedRecipe, setCachedRecipe] = useState(null);

  // Try to load from localStorage on initial mount
  useEffect(() => {
    const stored = localStorage.getItem(`recipe-${id}`);
    if (stored) {
      const parsed = JSON.parse(stored);
      setCachedRecipe(parsed);
      console.log(`Loaded recipe ${id} from cache`);
    }
  }, [id]);

  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
    skip: !!cachedRecipe, // Don't run query if we have cached data
    fetchPolicy: "network-only", // Avoid Apollo cache interference
  });

  // Cache the data after GraphQL returns it
  useEffect(() => {
    if (!cachedRecipe && !loading && data?.recipeById) {
      localStorage.setItem(`recipe-${id}`, JSON.stringify(data.recipeById));
      setCachedRecipe(data.recipeById);
      console.log("USED GRAPHQL TO GET RECIPE DETAILS");
    }
  }, [data, cachedRecipe, loading, id]);

  if (loading && !cachedRecipe) return <CircularProgress />;
  if (error) return <Typography>Error loading recipe.</Typography>;

  const recipe = cachedRecipe || data?.recipeById;

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
