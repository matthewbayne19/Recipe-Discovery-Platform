import React from "react";
import { Grid } from "@mui/material";
import RecipeCard from "./RecipeCard";

// Displays a list of recipes using RecipeCard components
const RecipeList = ({ recipes }) => {
  return (
    <Grid
      container
      spacing={2} // Spacing between each grid item
      justifyContent="center" // Centers the grid items horizontally
      alignItems="flex-start" // Aligns grid items to the start of the flex container vertically
      sx={{ maxWidth: 1100, margin: '0 auto' }} // Limits the width and centers the grid in the parent container
    >
      {recipes.map((recipe) => ( // Maps each recipe to a RecipeCard component
        <Grid item key={recipe.id}>
          <RecipeCard recipe={recipe} />
        </Grid>
      ))}
    </Grid>
  );
};

export default RecipeList;
