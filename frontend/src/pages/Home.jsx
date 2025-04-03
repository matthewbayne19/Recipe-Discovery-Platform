import React, { useEffect, useState } from 'react';
import { Container, Typography, CircularProgress, Alert, Grid } from '@mui/material';
import RecipeList from '../components/RecipeList';
import axios from 'axios';

// Home page shows all recipes, allows filtering, and handles loading/error states
const Home = () => {
  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Fetch recipes from REST endpoint on page load
  useEffect(() => {
    const fetchRecipes = async () => {
      try {
        const response = await axios.get('http://localhost:5011/recipes', {
          headers: {
            'X-API-KEY': 'simple-api-key' // Required by backend
          }
        });
        setRecipes(response.data);
      } catch (err) {
        setError('Failed to load recipes.');
      } finally {
        setLoading(false);
      }
    };

    fetchRecipes();
  }, []);

  return (
    <Container sx={{ paddingY: 4 }}>
      <Typography variant="h4" gutterBottom>
        Recipe Discovery
      </Typography>

      {/* Loading Spinner */}
      {loading && <CircularProgress />}

      {/* Error Alert */}
      {error && <Alert severity="error">{error}</Alert>}

      {/* Render recipes when loaded */}
      {!loading && !error && (
        <Grid container spacing={2}>
          <RecipeList recipes={recipes} />
        </Grid>
      )}
    </Container>
  );
};

export default Home;
