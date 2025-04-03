import React, { useEffect, useState } from "react";
import { Grid, Typography, TextField, CircularProgress, Alert } from "@mui/material";
import RecipeCard from "./RecipeCard";
import axios from "axios";

const API_URL = "http://localhost:5011/recipes"; // REST endpoint
const API_KEY = "simple-api-key"; // Must match your middleware check

const RecipeList = () => {
  const [recipes, setRecipes] = useState([]);
  const [filter, setFilter] = useState(""); // Input value
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Fetch recipes from backend on filter change
  useEffect(() => {
    const fetchRecipes = async () => {
      setLoading(true);
      setError("");

      try {
        const response = await axios.get(API_URL, {
          params: {
            cuisine: filter, // Adjust this to use "ingredient" if needed
          },
          headers: {
            "X-API-KEY": API_KEY,
          },
        });

        setRecipes(response.data);
      } catch (err) {
        setError("Failed to fetch recipes");
      } finally {
        setLoading(false);
      }
    };

    fetchRecipes();
  }, [filter]);

  return (
    <>
      {/* Filter input */}
      <TextField
        label="Filter by cuisine"
        variant="outlined"
        fullWidth
        sx={{ mb: 3 }}
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
      />

      {/* Loading spinner */}
      {loading && <CircularProgress />}

      {/* Error message */}
      {error && <Alert severity="error">{error}</Alert>}

      {/* Render recipe cards */}
      <Grid container spacing={2} justifyContent="center">
        {recipes.map((recipe) => (
          <Grid item key={recipe.id}>
            <RecipeCard recipe={recipe} />
          </Grid>
        ))}
      </Grid>
    </>
  );
};

export default RecipeList;
