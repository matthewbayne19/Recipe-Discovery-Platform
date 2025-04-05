import React from "react";
import { Card, CardMedia, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

// Displays a single recipe in a card format with clickable functionality
const RecipeCard = ({ recipe }) => {
  const navigate = useNavigate();

  return (
    <Card 
      sx={{ 
        width: 300, 
        m: 2, 
        cursor: "pointer", // Makes the card clickable
        transition: "transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out", // Smooth transition for hover effects
        "&:hover": {
          transform: "scale(1.10)", // Enlarges the card on hover
          boxShadow: "0 4px 20px rgba(0,0,0,0.2)" // Adds shadow effect on hover
        }
      }} 
      onClick={() => navigate(`/recipes/${recipe.id}`)} // Navigates to the recipe's detailed page
    >
      <CardMedia
        component="img"
        height="180"
        image={recipe.imageUrl.replace('/media/meals/', '/media/meals/medium/')} // Adjusts the image path to fetch medium size
        alt={recipe.name} // Provides a text description of the image for accessibility
      />
      <CardContent>
        <Typography gutterBottom variant="h6">
          {recipe.name} {/* Displays the name of the recipe */}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Cuisine: {recipe.cuisine} {/* Displays the cuisine type of the recipe */}
        </Typography>
      </CardContent>
    </Card>
  );
};

export default RecipeCard;
