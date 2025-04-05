import React, { useEffect, useState } from 'react';
import {
  Container, CircularProgress, Alert, Grid, FormControl, InputLabel, Select, MenuItem, Box
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import RecipeList from '../components/RecipeList';
import { fetchRecipes, searchRecipesByName } from '../api/rest'; 
import SearchBar from '../components/SearchBar';
import Filter from '../components/Filter';
import NavigationButton from '../components/NavigationButton';
import FavoriteIcon from '@mui/icons-material/Favorite';
import Pagination from '../components/Pagination';

const LOCAL_STORAGE_KEY = 'recipe_cache';

const Home = () => {
  const [recipes, setRecipes] = useState([]); // Stores the recipes
  const [page, setPage] = useState(1); // Current page number
  const [pageSize, setPageSize] = useState(9); // Recipes per page
  const [totalCount, setTotalCount] = useState(0); // Total number of recipes
  const [loading, setLoading] = useState(true); // Loading state
  const [error, setError] = useState(null); // Error state
  const [filterCuisine, setFilterCuisine] = useState(localStorage.getItem('filterCuisine') || ''); // Cuisine filter
  const [filterIngredient, setFilterIngredient] = useState(localStorage.getItem('filterIngredient') || ''); // Ingredient filter
  const [debouncedCuisine, setDebouncedCuisine] = useState(filterCuisine); // Debounced cuisine filter
  const [debouncedIngredient, setDebouncedIngredient] = useState(filterIngredient); // Debounced ingredient filter
  const [searchTerm, setSearchTerm] = useState(''); // Search term state
  const navigate = useNavigate();

  // Debounce filters to reduce API calls
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

  // Fetch recipes from the API or cache
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
        setRecipes(cachedPage.recipes);
        setTotalCount(cachedPage.totalCount);
        setLoading(false);
        return;
      }
    }
  
    handleFetch();
  }, [page, pageSize, debouncedCuisine, debouncedIngredient]);

  // Clear cuisine filter
  const handleClearCuisine = () => {
    setFilterCuisine('');
  };

  // Clear ingredient filter
  const handleClearIngredient = () => {
    setFilterIngredient('');
  };

  // Clear search and fetch default recipe list
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
  
      setRecipes(result.recipes);
      setTotalCount(result.totalCount);
    } catch (err) {
      console.error('Failed to reload recipes after clearing search:', err);
      setError('Failed to load recipes.');
    } finally {
      setLoading(false);
    }
  };

  // Handle recipe search
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
    <Container maxWidth="lg" sx={{ paddingY: 4, minHeight: '90vh', display: 'flex', flexDirection: 'column' }}>
      <NavigationButton
        icon={<FavoriteIcon />}
        onClick={() => navigate('/favorites')}
        label="View Favorites"
        positionStyles={{ top: 30, right: 30 }}
      />

      <Box mb={3} display="flex" justifyContent="center" alignItems="center" gap={2} flexWrap="wrap">
        <Box sx={{ minWidth: 250, flex: 1 }}>
            <SearchBar searchTerm={searchTerm} setSearchTerm={setSearchTerm} handleSearch={handleSearch} handleClearSearch={handleClearSearch} />
        </Box>
        <Box sx={{ minWidth: 180, flex: 1 }}>
            <Filter label="Filter by cuisine" value={filterCuisine} onChange={(e) => setFilterCuisine(e.target.value)} onClear={handleClearCuisine} />
        </Box>
        <Box sx={{ minWidth: 180, flex: 1 }}>
            <Filter label="Filter by ingredient" value={filterIngredient} onChange={(e) => setFilterIngredient(e.target.value)} onClear={handleClearIngredient} />
        </Box>
      </Box>

      {/* Main content */}
      <Box flex={1} display="flex" flexDirection="column" justifyContent="center" alignItems="center" minHeight="80vh">
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

      {/* Pagination controls */}
      <Pagination
        pageSize={pageSize}
        setPageSize={setPageSize}
        totalPages={totalPages}
        page={page}
        setPage={setPage}
        isLoading={loading}
      />
    </Container>
  );
};

export default Home;
