import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { CircularProgress, Typography, Box, Button, Tooltip } from "@mui/material";
import StarIcon from "@mui/icons-material/Star";
import { GET_RECIPE_BY_ID } from "../api/graphql";
import { addFavorite } from "../api/rest"; 

const RecipePage = () => {
  const { id } = useParams();
  const [cachedRecipe, setCachedRecipe] = useState(null);

  // Load from localStorage on initial mount
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
    skip: !!cachedRecipe,
    fetchPolicy: "network-only",
  });

  // Cache data after GraphQL returns it
  useEffect(() => {
    if (!cachedRecipe && !loading && data?.recipeById) {
      localStorage.setItem(`recipe-${id}`, JSON.stringify(data.recipeById));
      setCachedRecipe(data.recipeById);
      console.log("Used GraphQL to get data.");
    }
  }, [data, cachedRecipe, loading, id]);

  const handleAddToFavorites = async () => {
    try {
      await addFavorite(id);  // Use the addFavorite function from rest.js
      alert("Recipe added to favorites!");
    } catch (err) {
      console.error("Failed to add to favorites", err);
      alert("Error adding to favorites.");
    }
  };

  if (loading && !cachedRecipe) return <CircularProgress />;
  if (error) return <Typography>Error loading recipe.</Typography>;

  const recipe = cachedRecipe || data?.recipeById;

  return (
    <Box p={3}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Typography variant="h4">{recipe.name}</Typography>
        <Tooltip title="Add to Favorites">
          <Button
            variant="contained"
            color="primary"
            size="large"
            startIcon={<StarIcon />}
            onClick={handleAddToFavorites}
          >
            Add to Favorites
          </Button>
        </Tooltip>
      </Box>

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
