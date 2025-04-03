import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { CircularProgress, Typography, Box, Button, Tooltip, Snackbar } from "@mui/material";
import StarIcon from "@mui/icons-material/Star";
import { GET_RECIPE_BY_ID, TOGGLE_FAVORITE_MUTATION, GET_USER_FAVORITES } from "../api/graphql";

const RecipePage = () => {
  const { id } = useParams();
  const [cachedRecipe, setCachedRecipe] = useState(null);
  const [isFavorite, setIsFavorite] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  // Query to get the recipe details
  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
    skip: !!cachedRecipe,
    fetchPolicy: "network-only",
  });

  // Query to check if the recipe is in user's favorites
  const { data: favoritesData } = useQuery(GET_USER_FAVORITES);

  // Mutation to toggle the favorite status of the recipe
  const [toggleFavorite, { loading: togglingFavorite, error: toggleError }] = useMutation(TOGGLE_FAVORITE_MUTATION, {
    variables: { recipeId: id },
    onCompleted: (data) => {
      setIsFavorite(!isFavorite); // Toggle the local favorite state
      setSnackbarMessage(data.toggleFavoriteRecipe);
      setSnackbarOpen(true);
    }
  });

  // Load from localStorage and determine if it's a favorite
  useEffect(() => {
    const stored = localStorage.getItem(`recipe-${id}`);
    if (stored) {
      const parsed = JSON.parse(stored);
      setCachedRecipe(parsed);
      console.log(`Loaded recipe ${id} from cache`);
    }

    // Determine if the recipe is in the favorites list
    if (favoritesData && favoritesData.userFavorites.includes(id)) {
      setIsFavorite(true);
    }
  }, [id, favoritesData]);

  // Cache recipe details
  useEffect(() => {
    if (!cachedRecipe && !loading && data?.recipeById) {
      localStorage.setItem(`recipe-${id}`, JSON.stringify(data.recipeById));
      setCachedRecipe(data.recipeById);
      console.log("Used GraphQL to get data.");
    }
  }, [data, cachedRecipe, loading, id]);

  const handleToggleFavorite = async () => {
    try {
      await toggleFavorite();
    } catch (err) {
      console.error("Failed to toggle recipe in favorites", err);
      setSnackbarMessage('Error toggling recipe in favorites.');
      setSnackbarOpen(true);
    }
  };

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  if (loading && !cachedRecipe) return <CircularProgress />;
  if (error || toggleError) return <Typography>Error loading recipe.</Typography>;

  const recipe = cachedRecipe || data?.recipeById;

  return (
    <Box p={3}>
      <Box display="flex" alignItems="center" justifyContent="space-between">
        <Typography variant="h4">{recipe.name}</Typography>
        <Tooltip title={isFavorite ? "Remove from Favorites" : "Add to Favorites"}>
          <Button
            variant="contained"
            color="primary"
            size="large"
            startIcon={<StarIcon />}
            onClick={handleToggleFavorite}
            disabled={togglingFavorite}
          >
            {isFavorite ? "Remove from Favorites" : "Add to Favorites"}
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

      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        message={snackbarMessage}
      />
    </Box>
  );
};

export default RecipePage;
