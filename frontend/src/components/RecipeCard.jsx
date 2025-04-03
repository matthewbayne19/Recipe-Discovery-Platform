import React from "react";
import { Card, CardMedia, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

// RecipeCard displays a single recipe summary and navigates on click
const RecipeCard = ({ recipe }) => {
  const navigate = useNavigate();

  // Handle card click to navigate to detailed recipe view
  const handleClick = () => {
    navigate(`/recipes/${recipe.id}`);
  };

  return (
    <Card 
      sx={{ maxWidth: 345, m: 2, cursor: "pointer" }} 
      onClick={handleClick}
    >
      {/* Recipe thumbnail image */}
      <CardMedia
        component="img"
        height="180"
        image={recipe.imageUrl}
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
