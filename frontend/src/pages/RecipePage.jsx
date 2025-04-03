import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { CircularProgress, Typography, Box, Button, Tooltip } from "@mui/material";
import StarIcon from "@mui/icons-material/Star";
import { GET_RECIPE_BY_ID, TOGGLE_FAVORITE_MUTATION, GET_USER_FAVORITES } from "../api/graphql";

const RecipePage = () => {
  const { id } = useParams();
  const [cachedRecipe, setCachedRecipe] = useState(null);
  const [isFavorite, setIsFavorite] = useState(false);

  // Query to get the recipe by id if it's not in cache
  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
    skip: !!cachedRecipe,
    fetchPolicy: "network-only",
  });

  // Query to check if the recipe is in user's favorites
  const { data: favoritesData } = useQuery(GET_USER_FAVORITES);

  // Mutation to add/remove recipe
  const [toggleFavorite, { loading: togglingFavorite, error: toggleError }] = useMutation(TOGGLE_FAVORITE_MUTATION, {
    variables: { recipeId: id },
    refetchQueries: [{ query: GET_USER_FAVORITES }],  // Refetch favorites list after toggling
  });

  // Load from localStorage on initial mount
  useEffect(() => {
    const stored = localStorage.getItem(`recipe-${id}`);
    if (stored) {
      const parsed = JSON.parse(stored);
      setCachedRecipe(parsed);
      console.log(`Loaded recipe ${id} from cache`);
    }
  }, [id]);

  // Cache data after GraphQL returns it
  useEffect(() => {
    if (!cachedRecipe && !loading && data?.recipeById) {
      localStorage.setItem(`recipe-${id}`, JSON.stringify(data.recipeById));
      setCachedRecipe(data.recipeById);
      console.log("Used GraphQL to get data.");
    }
  }, [data, cachedRecipe, loading, id]);

  // Check if the recipe is a favorite
  useEffect(() => {
    if (favoritesData && favoritesData.userFavorites) {
      setIsFavorite(favoritesData.userFavorites.includes(id));
    }
  }, [favoritesData, id]);

  const handleToggleFavorite = async () => {
    try {
      const response = await toggleFavorite();
      console.log("Toggle favorite method hit.")
      alert(response.data.toggleFavoriteRecipe);
    } catch (err) {
      console.error("Failed to toggle recipe in favorites", err);
      alert("Error toggling recipe in favorites.");
    }
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
    </Box>
  );
};

export default RecipePage;
