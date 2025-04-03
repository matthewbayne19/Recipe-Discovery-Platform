import React from "react";
import { Grid } from "@mui/material";
import RecipeCard from "./RecipeCard";

// RecipeList receives recipes via props
const RecipeList = ({ recipes }) => {
  return (
    <Grid container spacing={2} justifyContent="center">
      {recipes.map((recipe) => (
        <Grid item key={recipe.id}>
          <RecipeCard recipe={recipe} />
        </Grid>
      ))}
    </Grid>
  );
};

export default RecipeList;
