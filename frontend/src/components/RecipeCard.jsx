import React from "react";
import { Card, CardMedia, CardContent, Typography } from "@mui/material";

// RecipeCard component receives a single recipe object as a prop
const RecipeCard = ({ recipe }) => {
  return (
    <Card sx={{ maxWidth: 345, m: 2 }}>
      {/* Displays the image thumbnail for the recipe */}
      <CardMedia
        component="img"
        height="180"
        image={recipe.imageUrl} // Uses the backend-supplied image URL
        alt={recipe.name}
      />

      <CardContent>
        {/* Recipe name */}
        <Typography gutterBottom variant="h6" component="div">
          {recipe.name}
        </Typography>

        {/* Recipe cuisine */}
        <Typography variant="body2" color="text.secondary">
          Cuisine: {recipe.cuisine}
        </Typography>
      </CardContent>
    </Card>
  );
};

export default RecipeCard;
