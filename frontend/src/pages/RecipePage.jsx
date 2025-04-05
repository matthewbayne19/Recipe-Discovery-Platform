import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import {
  CircularProgress, Typography, Box, Tooltip, Snackbar,
  Card, CardContent, List, ListItem, ListItemText, Checkbox, CardMedia, Divider, Button
} from "@mui/material";
import StarIcon from "@mui/icons-material/Star";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import FavoriteIcon from "@mui/icons-material/Favorite";
import { GET_RECIPE_BY_ID, TOGGLE_FAVORITE_MUTATION, GET_USER_FAVORITES } from "../api/graphql";
import NavigationButton from '../components/NavigationButton';

const RecipePage = () => {
  const navigate = useNavigate();
  const { id } = useParams(); // Gets the recipe ID from the URL
  const [cachedRecipe, setCachedRecipe] = useState(null); // Stores recipe data from local storage
  const [isFavorite, setIsFavorite] = useState(false); // Tracks if the recipe is favorited
  const [snackbarOpen, setSnackbarOpen] = useState(false); // Controls visibility of the snackbar
  const [snackbarMessage, setSnackbarMessage] = useState(''); // Message to display in the snackbar

  // GraphQL query to fetch recipe by ID
  const { data, loading, error } = useQuery(GET_RECIPE_BY_ID, {
    variables: { id },
    skip: !!cachedRecipe, // Skip query if we already have cached data
    fetchPolicy: "network-only", // Ensures fresh data is fetched
  });

  // GraphQL query to check if the recipe is in user's favorites
  useQuery(GET_USER_FAVORITES, {
    fetchPolicy: "cache-and-network", // Optimistic UI response
    onCompleted: data => {
      setIsFavorite(data.userFavorites.includes(id)); // Update favorite status based on response
    }
  });

  // Mutation to toggle favorite status
  const [toggleFavorite, { loading: togglingFavorite, error: toggleError }] = useMutation(TOGGLE_FAVORITE_MUTATION, {
    variables: { recipeId: id },
    refetchQueries: [{ query: GET_USER_FAVORITES }],
    onCompleted: (data) => {
      setIsFavorite(!isFavorite);
      setSnackbarMessage(data.toggleFavoriteRecipe); // Show result of toggle operation
      setSnackbarOpen(true);
    }
  });

  // Load recipe from local storage
  useEffect(() => {
    const stored = localStorage.getItem(`recipe-${id}`);
    if (stored) {
      const parsed = JSON.parse(stored);
      setCachedRecipe(parsed);
    }
  }, [id]);

  // Cache recipe data once fetched
  useEffect(() => {
    if (!cachedRecipe && !loading && data?.recipeById) {
      localStorage.setItem(`recipe-${id}`, JSON.stringify(data.recipeById));
      setCachedRecipe(data.recipeById);
    }
  }, [data, cachedRecipe, loading, id]);

  // Function to handle favorite toggle
  const handleToggleFavorite = async () => {
    try {
      await toggleFavorite();
    } catch (err) {
      console.error("Failed to toggle recipe in favorites", err);
      setSnackbarMessage('Error toggling recipe in favorites.');
      setSnackbarOpen(true);
    }
  };

  // Close snackbar
  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  // Render loading or error states
  if (loading && !cachedRecipe) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          height: '80vh',
        }}
      >
        <CircularProgress />
      </Box>
    );
  }
  if (error || toggleError) return <Typography>Error loading recipe.</Typography>;

  const recipe = cachedRecipe || data?.recipeById;

  // Main content render
  return (
    <Box p={3} sx={{ maxWidth: 900, margin: 'auto' }}>
      <Box>
        <NavigationButton
          icon={<ArrowBackIcon />}
          onClick={() => navigate('/')}
          label="Home"
          positionStyles={{ top: 30, left: 30 }}
        />
        <NavigationButton
          icon={<FavoriteIcon />}
          onClick={() => navigate('/favorites')}
          label="View Favorites"
          positionStyles={{ top: 30, right: 30 }}
        />
      </Box>
      <Card raised sx={{ mb: 2 }}>
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
          <Typography variant="subtitle1" gutterBottom>
            <strong>Cuisine:</strong> {recipe.cuisine} | <strong>Prep Time:</strong> {recipe.preparationTime} | <strong>Difficulty:</strong> {recipe.difficultyLevel}
            {recipe.nutrition && (
                <Box mt={1}>
                    <Typography variant="subtitle2" sx={{ fontWeight: 'bold' }}>Nutrition Info:</Typography>
                    <Typography variant="body2">
                    Calories: {recipe.nutrition.calories} | Protein: {recipe.nutrition.protein}g 
                    </Typography>
                </Box>
            )}
          </Typography>
        </CardContent>
      </Card>
      <Card raised>
        <CardContent>
          <Typography variant="h6" gutterBottom component="div" sx={{ fontWeight: 'bold' }}>Ingredients:</Typography>
          <List dense>
            {recipe.ingredients.map((ingredient, index) => (
              <React.Fragment key={index}>
                <ListItem secondaryAction={<Checkbox edge="end" />}>
                  <ListItemText primary={ingredient} />
                </ListItem>
                {index !== recipe.ingredients.length - 1 && <Divider />}
              </React.Fragment>
            ))}
          </List>
        </CardContent>
      </Card>
      <Card raised sx={{ mt: 2 }}>
        <CardContent>
          <Typography variant="h6" gutterBottom component="div" sx={{ fontWeight: 'bold' }}>Directions:</Typography>
          <Typography variant="body1">
            {recipe.description}
          </Typography>
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
