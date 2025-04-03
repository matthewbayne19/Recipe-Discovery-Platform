import React, { useEffect, useState } from 'react';
import {
  Container, Typography, CircularProgress, Alert, Grid,
  Pagination, FormControl, InputLabel, Select, MenuItem, Box
} from '@mui/material';
import RecipeList from '../components/RecipeList';
import axios from 'axios';

const LOCAL_STORAGE_KEY = 'recipe_cache';

const Home = () => {
  const [recipes, setRecipes] = useState([]);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Load from cache on mount
  useEffect(() => {
    const saved = localStorage.getItem(LOCAL_STORAGE_KEY);
    if (saved) {
      const parsed = JSON.parse(saved);
      const cachedPage = parsed[pageSize]?.[page];
      if (cachedPage) {
        console.log(`Page ${page} (pageSize ${pageSize}) loaded from localStorage`);
        setRecipes(cachedPage.recipes);
        setTotalCount(cachedPage.totalCount);
        setLoading(false);
        return;
      }
    }
    fetchRecipes();
  }, [page, pageSize]);

  const fetchRecipes = async () => {
    setLoading(true);
    setError(null);

    try {
      const response = await axios.get('http://localhost:5011/recipes', {
        headers: { 'X-API-KEY': 'simple-api-key' },
        params: { page, pageSize }
      });

      const result = {
        recipes: response.data.recipes,
        totalCount: response.data.totalCount
      };

      console.log(`Page ${page} (pageSize ${pageSize}) fetched from API`);

      setRecipes(result.recipes);
      setTotalCount(result.totalCount);

      const existing = JSON.parse(localStorage.getItem(LOCAL_STORAGE_KEY) || '{}');
      const updated = {
        ...existing,
        [pageSize]: {
          ...existing[pageSize],
          [page]: result
        }
      };
      localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(updated));
    } catch (err) {
      setError('Failed to load recipes.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <Container sx={{ paddingY: 4 }}>
      <Typography variant="h4" gutterBottom>
        Recipe Discovery
      </Typography>

      {/* Recipe content */}
      {loading && <Box textAlign="center" py={4}><CircularProgress /></Box>}
      {error && <Alert severity="error">{error}</Alert>}
      {!loading && !error && (
        <Grid container spacing={2}>
          <RecipeList recipes={recipes} />
        </Grid>
      )}

      {/* Always visible pagination controls */}
      <Box
        mt={6}
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        flexWrap="wrap"
      >
        <FormControl size="small" disabled={loading} sx={{ minWidth: 120 }}>
          <InputLabel>Per Page</InputLabel>
          <Select
            value={pageSize}
            label="Per Page"
            onChange={(e) => {
              setPageSize(e.target.value);
              setPage(1);
            }}
          >
            {[5, 10, 20, 50].map(size => (
              <MenuItem key={size} value={size}>{size}</MenuItem>
            ))}
          </Select>
        </FormControl>

        <Pagination
          count={totalPages}
          page={page}
          onChange={(_, newPage) => setPage(newPage)}
          color="primary"
          disabled={loading}
        />
      </Box>
    </Container>
  );
};

export default Home;
