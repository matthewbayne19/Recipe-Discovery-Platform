import React, { useEffect, useState } from 'react';
import {
  Container, CircularProgress, Alert, Grid,
  Pagination, FormControl, InputLabel, Select, MenuItem, Box, TextField, Button,
  InputAdornment, IconButton
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import RecipeList from '../components/RecipeList';
import { fetchRecipes, searchRecipesByName } from '../api/rest'; 
import ClearIcon from '@mui/icons-material/Clear';
import FavoriteIcon from '@mui/icons-material/Favorite';
import SearchIcon from '@mui/icons-material/Search';

const LOCAL_STORAGE_KEY = 'recipe_cache';

const Home = () => {
  const [recipes, setRecipes] = useState([]);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(9);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filterCuisine, setFilterCuisine] = useState(localStorage.getItem('filterCuisine') || '');
  const [filterIngredient, setFilterIngredient] = useState(localStorage.getItem('filterIngredient') || '');
  const [debouncedCuisine, setDebouncedCuisine] = useState(filterCuisine);
  const [debouncedIngredient, setDebouncedIngredient] = useState(filterIngredient);
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();

  // Debounce filters
  useEffect(() => {
    const timeout = setTimeout(() => {
      setDebouncedCuisine(filterCuisine);
      setDebouncedIngredient(filterIngredient);
      setPage(1);
      localStorage.setItem('filterCuisine', filterCuisine);
      localStorage.setItem('filterIngredient', filterIngredient);
    }, 500);
    return () => clearTimeout(timeout);
  }, [filterCuisine, filterIngredient]);

  useEffect(() => {
    const saved = localStorage.getItem(LOCAL_STORAGE_KEY);
  
    const handleFetch = async () => {
      setLoading(true);
      setError(null);
  
      try {
        const result = await fetchRecipes({
          page,
          pageSize,
          cuisine: debouncedCuisine,
          ingredient: debouncedIngredient
        });

        console.log(`Page ${page} (pageSize ${pageSize}) fetched from API`);
  
        setRecipes(result.recipes);
        setTotalCount(result.totalCount);
  
        if (!debouncedCuisine && !debouncedIngredient) {
          const existing = JSON.parse(localStorage.getItem(LOCAL_STORAGE_KEY) || '{}');
          const updated = {
            ...existing,
            [pageSize]: {
              ...existing[pageSize],
              [page]: result
            }
          };
          localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(updated));
        }
      } catch (err) {
        setError('Failed to load recipes.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
  
    if (saved && !debouncedCuisine && !debouncedIngredient) {
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
  
    handleFetch();
  }, [page, pageSize, debouncedCuisine, debouncedIngredient]);

  const handleClearCuisine = () => {
    setFilterCuisine('');
  };

  const handleClearIngredient = () => {
    setFilterIngredient('');
  };

  const handleClearSearch = async () => {
    setSearchTerm('');
    setPage(1);
    setLoading(true);
    setError(null);
  
    try {
      const result = await fetchRecipes({
        page: 1,
        pageSize,
        cuisine: debouncedCuisine,
        ingredient: debouncedIngredient
      });
  
      console.log('Reset to default recipe list after clearing search');
      setRecipes(result.recipes);
      setTotalCount(result.totalCount);
    } catch (err) {
      console.error('Failed to reload recipes after clearing search:', err);
      setError('Failed to load recipes.');
    } finally {
      setLoading(false); // <-- this makes the spinner disappear once done
    }
  };

  const handleSearch = async () => {
    if (!searchTerm.trim()) return;
    setLoading(true);
    setError(null);
    try {
      const results = await searchRecipesByName(searchTerm.trim());
      setRecipes(results || []);
      setTotalCount(results?.length || 0);
      setPage(1);
    } catch (err) {
      setError("Search failed.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <Container
      maxWidth="lg"
      sx={{
        paddingY: 4,
        minHeight: '90vh',
        display: 'flex',
        flexDirection: 'column'
      }}
    >
    <Button
        startIcon={<FavoriteIcon />}
        onClick={() => navigate('/favorites')}
        sx={{
            position: 'fixed',
            top: 30,
            right: 30,
            zIndex: 10
        }}
        >
        View Favorites
    </Button>

    {/* Filters */}
    <Box
        mb={3}
        display="flex"
        justifyContent="center"
        alignItems="center"
        gap={2}
        flexWrap="wrap"
        >
        <TextField
        label="Search by name"
        variant="outlined"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        InputProps={{
            endAdornment: (
            <InputAdornment position="end">
                {searchTerm && (
                <IconButton onClick={handleClearSearch} edge="end">
                    <ClearIcon />
                </IconButton>
                )}
                <IconButton onClick={handleSearch} edge="end">
                <SearchIcon />
                </IconButton>
            </InputAdornment>
            ),
        }}
    />

  <TextField
    label="Filter by cuisine"
    variant="outlined"
    value={filterCuisine}
    onChange={(e) => setFilterCuisine(e.target.value)}
    InputProps={{
      endAdornment: filterCuisine && (
        <InputAdornment position="end">
          <IconButton onClick={handleClearCuisine} edge="end">
            <ClearIcon />
          </IconButton>
        </InputAdornment>
      ),
    }}
  />
  <TextField
    label="Filter by ingredient"
    variant="outlined"
    value={filterIngredient}
    onChange={(e) => setFilterIngredient(e.target.value)}
    InputProps={{
      endAdornment: filterIngredient && (
        <InputAdornment position="end">
          <IconButton onClick={handleClearIngredient} edge="end">
            <ClearIcon />
          </IconButton>
        </InputAdornment>
      ),
    }}
  />
</Box>


      {/* Main content */}
      <Box
        flex={1}
        display="flex"
        flexDirection="column"
        justifyContent="center"
        alignItems="center"
        minHeight="80vh"
      >
        {loading ? (
          <CircularProgress />
        ) : error ? (
          <Alert severity="error">{error}</Alert>
        ) : recipes.length === 0 ? (
          <Alert severity="info" sx={{ textAlign: 'center', maxWidth: 400 }}>
            No recipes found for this filter. Please try another filter.
          </Alert>
        ) : (
          <Grid container spacing={2} justifyContent="center">
            <RecipeList recipes={recipes} />
          </Grid>
        )}
      </Box>

      {/* Pagination */}
      <Box
        mt="3vh"
        borderTop="1px solid #ddd"
        py={2}
        display="flex"
        justifyContent="center"
        alignItems="center"
        gap={4}
        sx={{ backgroundColor: 'white' }}
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
            {[9, 18, 27, 36].map(size => (
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
