import React, { useEffect, useState } from 'react';
import {
  Container,
  Typography,
  CircularProgress,
  Alert,
  Grid,
  Pagination,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Box
} from '@mui/material';
import RecipeList from '../components/RecipeList';
import axios from 'axios';

const Home = () => {
  const [recipes, setRecipes] = useState([]);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [cache, setCache] = useState({}); // { [pageSize]: { [page]: data } }

  useEffect(() => {
    const fetchRecipes = async () => {
      const cachedPage = cache[pageSize]?.[page];

      if (cachedPage) {
        console.log(`Page ${page} (pageSize ${pageSize}) loaded from cache`);
        setRecipes(cachedPage);
        setLoading(false);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        const response = await axios.get('http://localhost:5011/recipes', {
          headers: {
            'X-API-KEY': 'simple-api-key'
          },
          params: {
            page,
            pageSize
          }
        });

        console.log(`Page ${page} (pageSize ${pageSize}) fetched from API`);
        setRecipes(response.data.recipes);

        // Cache the results
        setCache(prev => ({
          ...prev,
          [pageSize]: {
            ...prev[pageSize],
            [page]: response.data.recipes
          }
        }));

        setTotalCount(response.data.totalCount);
      } catch (err) {
        setError('Failed to load recipes.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchRecipes();
  }, [page, pageSize]);

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <Container sx={{ paddingY: 4 }}>
      <Typography variant="h4" gutterBottom>
        Recipe Discovery
      </Typography>

      {loading && <CircularProgress />}
      {error && <Alert severity="error">{error}</Alert>}

      {!loading && !error && (
        <>
          <Grid container spacing={2}>
            <RecipeList recipes={recipes || []} />
          </Grid>

          <Box display="flex" justifyContent="space-between" alignItems="center" mt={4}>
            <FormControl size="small">
              <InputLabel>Per Page</InputLabel>
              <Select
                value={pageSize}
                label="Per Page"
                onChange={(e) => {
                  setPageSize(e.target.value);
                  setPage(1); // reset to first page when page size changes
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
            />
          </Box>
        </>
      )}
    </Container>
  );
};

export default Home;
