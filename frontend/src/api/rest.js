import axios from 'axios';

const API_URL = 'http://localhost:5011';
const API_KEY = 'simple-api-key';

// Fetch all recipes (optional: pass filters like cuisine or ingredient)
export const fetchRecipes = async (params = {}) => {
  const query = new URLSearchParams(params).toString();
  const response = await axios.get(`${API_URL}/recipes?${query}`, {
    headers: {
      'X-API-KEY': API_KEY,
    },
  });
  console.log("USED REST GET /recipes TO FETCH DATA")
  return response.data;
};

// Fetch a single recipe by ID - unused because I am getting individual recipes through GraphQL instead of REST
/*export const fetchRecipeById = async (id) => {
  const response = await axios.get(`${API_URL}/recipes/${id}`, {
    headers: {
      'X-API-KEY': API_KEY,
    },
  });
  return response.data;
};*/ 