import React, { useEffect, useState } from 'react';
import { Container, Typography, CircularProgress, Alert, Grid } from '@mui/material';
import RecipeList from '../components/RecipeList';
import { fetchRecipes } from '../api/rest';

const Home = () => {
  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadRecipes = async () => {
      try {
        const data = await fetchRecipes();
        setRecipes(data);
      } catch (err) {
        setError('Failed to load recipes.');
      } finally {
        setLoading(false);
      }
    };

    loadRecipes();
  }, []);

  return (
    <Container sx={{ paddingY: 4 }}>
      <Typography variant="h4" gutterBottom>
        Recipe Discovery
      </Typography>

      {loading && <CircularProgress />}
      {error && <Alert severity="error">{error}</Alert>}

      {!loading && !error && (
        <Grid container spacing={2}>
          <RecipeList recipes={recipes} />
        </Grid>
      )}
    </Container>
  );
};

export default Home;
