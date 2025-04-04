import React, { useEffect, useState } from 'react';
import { useQuery, useApolloClient } from "@apollo/client";
import { Container, Typography, Grid, CircularProgress, Alert, Link, Box, Button } from '@mui/material';
import RecipeCard from '../components/RecipeCard';
import { GET_USER_FAVORITES, GET_RECIPE_BY_ID } from '../api/graphql';
import { useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';  // Icon for the back button

const FavoritesPage = () => {
  const client = useApolloClient();
  const navigate = useNavigate();
  const [favoriteRecipes, setFavoriteRecipes] = useState([]);
  const [loadingDetails, setLoadingDetails] = useState(false);
  const { data, loading, error } = useQuery(GET_USER_FAVORITES);

  useEffect(() => {
    if (!loading && data && data.userFavorites) {
      if (data.userFavorites.length > 0) {
        fetchFavoriteRecipesDetails(data.userFavorites);
      }
    }
  }, [data, loading]);

  const fetchFavoriteRecipesDetails = async (recipeIds) => {
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
  };

  if (loading || loadingDetails) {
    return (
      <Container sx={{ py: 4, display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container sx={{ py: 4 }}>
        <Alert severity="error">{error.message}</Alert>
      </Container>
    );
  }

  return (
    <Container sx={{ py: 4, minHeight: '90vh', display: 'flex', flexDirection: 'column' }}>
      <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/')} sx={{ position: 'absolute', top: 30, left: 30 }}>
        Home
      </Button>
      <Typography variant="h4" gutterBottom sx={{ mt: 2, alignSelf: 'flex-start' }}>
        Your Favorite Recipes
      </Typography>
      {favoriteRecipes.length === 0 ? (
        <Typography variant="subtitle1" sx={{ mt: 2, alignSelf: 'center' }}>
          No favorites added yet. <Link component="button" variant="body2" onClick={() => navigate('/')}>
            Explore recipes
          </Link> and add them to favorites for them to appear here.
        </Typography>
      ) : (
        <Box flex={1} display="flex" flexDirection="column" justifyContent="center" alignItems="center">
          <Grid container spacing={2} justifyContent="center">
            {favoriteRecipes.map((recipe) => (
              <Grid item key={recipe.id} xs={12} sm={6} md={4}>
                <RecipeCard recipe={recipe} />
              </Grid>
            ))}
          </Grid>
        </Box>
      )}
    </Container>
  );
};

export default FavoritesPage;
