import React, { useEffect, useState, useCallback } from 'react';
import { useQuery, useApolloClient } from "@apollo/client";
import {
  Container, Typography, CircularProgress, Alert, Link
} from '@mui/material';
import RecipeList from '../components/RecipeList';  // Import RecipeList component
import { GET_USER_FAVORITES, GET_RECIPE_BY_ID } from '../api/graphql';
import { useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import NavigationButton from '../components/NavigationButton';

// Component for displaying user's favorite recipes
const FavoritesPage = () => {
  const client = useApolloClient();
  const navigate = useNavigate();
  const [favoriteRecipes, setFavoriteRecipes] = useState([]);
  const [loadingDetails, setLoadingDetails] = useState(false);
  const { data, loading, error } = useQuery(GET_USER_FAVORITES);

  // Fetches detailed data for favorite recipes
  const fetchFavoriteRecipesDetails = useCallback(async (recipeIds) => {
    setLoadingDetails(true);
    try {
      const recipes = await Promise.all(
        recipeIds.map(id =>
          client.query({
            query: GET_RECIPE_BY_ID,
            variables: { id }
          }).then(response => response.data.recipeById)
        )
      );
      setFavoriteRecipes(recipes);
    } catch (err) {
      console.error("Error fetching recipes details:", err);
    }
    setLoadingDetails(false);
  }, [client]);

  // Load recipe details after favorites IDs are fetched
  useEffect(() => {
    if (!loading && data?.userFavorites?.length > 0) {
      fetchFavoriteRecipesDetails(data.userFavorites);
    }
  }, [data, loading, fetchFavoriteRecipesDetails]);

  // Handle loading states
  if (loading || loadingDetails) {
    return (
      <Container sx={{ py: 4, display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
        <CircularProgress />
      </Container>
    );
  }

  // Handle error state
  if (error) {
    return (
      <Container sx={{ py: 4 }}>
        <Alert severity="error">{error.message}</Alert>
      </Container>
    );
  }

  // Render the favorite recipes page
  return (
    <Container sx={{ py: 4, minHeight: '90vh', display: 'flex', flexDirection: 'column' }}>
      <NavigationButton
        icon={<ArrowBackIcon />}
        onClick={() => navigate('/')}
        label="Home"
        positionStyles={{ top: 30, left: 30 }}
      />
      <Typography variant="h4" gutterBottom sx={{ mt: 2, mb: 5, alignSelf: 'flex-start' }}>
        Your Favorite Recipes
      </Typography>
      {favoriteRecipes.length === 0 ? (
        <Typography variant="h5" sx={{ mt: 10, alignSelf: 'center' }}>
          No favorites added yet.{' '}
          <Link
            component="span"
            onClick={() => navigate('/')}
            sx={{
              fontSize: 'inherit',
              color: 'inherit',
              textDecoration: 'underline',
              cursor: 'pointer',
              transition: 'color 0.2s ease-in-out',
              '&:hover': {
                color: 'primary.main',
              },
            }}
          >
            Explore recipes
          </Link>{' '}
          and add them to favorites for them to appear here.
        </Typography>
      ) : (
        <RecipeList recipes={favoriteRecipes} />  // Using RecipeList to display recipes
      )}
    </Container>
  );
};

export default FavoritesPage;
