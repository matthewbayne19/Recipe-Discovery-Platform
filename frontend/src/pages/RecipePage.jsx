import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import {
    CircularProgress, Typography, Box, Button, Tooltip, Snackbar,
    Card, CardContent, List, ListItem, ListItemText, Checkbox, CardMedia
} from "@mui/material";
import StarIcon from "@mui/icons-material/Star";
import ArrowBackIcon from "@mui/icons-material/ArrowBack"; // Icon for the button
import { GET_RECIPE_BY_ID, TOGGLE_FAVORITE_MUTATION, GET_USER_FAVORITES } from "../api/graphql";

const RecipePage = () => {
  const navigate = useNavigate(); // Hook for navigation
  const { id } = useParams();
  const [cachedRecipe, setCachedRecipe] = useState(null);
  const [isFavorite, setIsFavorite] = useState(false);
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
    skip: !!cachedRecipe,
    fetchPolicy: "network-only",
  });

  useQuery(GET_USER_FAVORITES, {
    fetchPolicy: "cache-and-network",
    onCompleted: data => {
      setIsFavorite(data.userFavorites.includes(id));
    }
  });

  const [toggleFavorite, { loading: togglingFavorite, error: toggleError }] = useMutation(TOGGLE_FAVORITE_MUTATION, {
    variables: { recipeId: id },
    refetchQueries: [{ query: GET_USER_FAVORITES }],
    onCompleted: (data) => {
      setIsFavorite(!isFavorite);
      setSnackbarMessage(data.toggleFavoriteRecipe);
      setSnackbarOpen(true);
    }
  });

  useEffect(() => {
    const stored = localStorage.getItem(`recipe-${id}`);
    if (stored) {
      const parsed = JSON.parse(stored);
      setCachedRecipe(parsed);
    }
  }, [id]);

  useEffect(() => {
    if (!cachedRecipe && !loading && data?.recipeById) {
      localStorage.setItem(`recipe-${id}`, JSON.stringify(data.recipeById));
      setCachedRecipe(data.recipeById);
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
    <Box p={3} sx={{ maxWidth: 900, margin: 'auto' }}>
      <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/')} sx={{ position: 'absolute', top: 30, left: 30 }}>
        Home
      </Button>
      <Card raised sx={{ mb: 4 }}>
        {recipe.imageUrl && (
          <CardMedia
            component="img"
            height="300"
            image={recipe.imageUrl}
            alt={`Image of ${recipe.name}`}
          />
        )}
        <CardContent>
          <Box display="flex" alignItems="center" justifyContent="space-between" mb={2}>
            <Typography variant="h4" gutterBottom>{recipe.name}</Typography>
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

          <Typography variant="subtitle1" gutterBottom sx={{ fontSize: '1.1rem' }}>
            Cuisine: {recipe.cuisine} | Prep Time: {recipe.preparationTime} | Difficulty: {recipe.difficultyLevel}
          </Typography>

          <Card sx={{ mt: 2, p: 2 }}>
            <Typography variant="body1" paragraph>
              {recipe.description}
            </Typography>
          </Card>
        </CardContent>
      </Card>

      <Card raised>
        <CardContent>
          <Typography variant="h6" gutterBottom>Ingredients:</Typography>
          <List dense>
            {recipe.ingredients.map((ingredient, index) => (
              <ListItem key={index} secondaryAction={
                <Checkbox
                  edge="end"
                />
              }>
                <ListItemText primary={ingredient} />
              </ListItem>
            ))}
          </List>
        </CardContent>
      </Card>

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
