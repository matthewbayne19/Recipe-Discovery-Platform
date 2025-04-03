import axios from 'axios';

const API_URL = 'http://localhost:5011';
const API_KEY = 'simple-api-key';

const headers = {
  'X-API-KEY': API_KEY,
};

// Fetch all recipes (with optional filters)
export const fetchRecipes = async (params = {}) => {
  const query = new URLSearchParams(params).toString();
  const response = await axios.get(`${API_URL}/recipes?${query}`, { headers });
  console.log("USED REST GET /recipes TO FETCH DATA");
  return response.data;
};

// Add a recipe to favorites (hardcoded user ID)
export const addFavorite = async (recipeId, userId = "1") => {
  await axios.post(`${API_URL}/users/${userId}/favorites`, { recipeId }, { headers });
  console.log(`USED REST POST /users/${userId}/favorites TO ADD RECIPE ${recipeId}`);
};

// Get a user's favorite recipes
export const fetchFavorites = async (userId = "1") => {
  const response = await axios.get(`${API_URL}/users/${userId}/favorites`, { headers });
  console.log(`USED REST GET /users/${userId}/favorites TO FETCH FAVORITES`);
  return response.data;
};
