import React, { useEffect, useState } from 'react';
import { useQuery, useLazyQuery } from "@apollo/client";
import { Container, Typography, Grid, CircularProgress, Alert } from '@mui/material';
import RecipeCard from '../components/RecipeCard';
import { GET_USER_FAVORITES, GET_RECIPE_BY_ID } from '../api/graphql';

const FavoritesPage = () => {
  const userId = "1";
  const [favoriteRecipes, setFavoriteRecipes] = useState([]);
  const { data, loading, error } = useQuery(GET_USER_FAVORITES, { variables: { userId } });
  const [getRecipeById, { called, loading: recipeLoading, data: recipeData }] = useLazyQuery(GET_RECIPE_BY_ID);

  useEffect(() => {
    if (data?.userFavorites) {
      data.userFavorites.forEach(recipeId => {
        getRecipeById({ variables: { id: recipeId } });
      });
    }
  }, [data, getRecipeById]);

  useEffect(() => {
    if (recipeData?.recipeById) {
      setFavoriteRecipes(prevRecipes => [...prevRecipes, recipeData.recipeById]);
    }
  }, [recipeData]);

  const isLoading = loading || (recipeLoading && called);

  return (
    <Container sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Your Favorite Recipes
      </Typography>
      {isLoading ? (
        <CircularProgress />
      ) : error ? (
        <Alert severity="error">{error.message}</Alert>
      ) : (
        <Grid container spacing={2}>
          {favoriteRecipes.map((recipe) => (
            <Grid item key={recipe.id} xs={12} sm={6} md={4}>
              <RecipeCard recipe={recipe} />
            </Grid>
          ))}
        </Grid>
      )}
    </Container>
  );
};

export default FavoritesPage;
