import React from "react";
import { Card, CardMedia, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

const RecipeCard = ({ recipe }) => {
  const navigate = useNavigate();

  return (
    <Card 
      sx={{ 
        width: 300, 
        m: 2, 
        cursor: "pointer",
        transition: "transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out",
        "&:hover": {
          transform: "scale(1.10)", 
          boxShadow: "0 4px 20px rgba(0,0,0,0.2)" 
        }
      }} 
      onClick={() => navigate(`/recipes/${recipe.id}`)}
    >
      <CardMedia
        component="img"
        height="180"
        image={recipe.imageUrl.replace('/media/meals/', '/media/meals/medium/')}
        alt={recipe.name}
      />
      <CardContent>
        <Typography gutterBottom variant="h6">
          {recipe.name}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Cuisine: {recipe.cuisine}
        </Typography>
      </CardContent>
    </Card>
  );
};

export default RecipeCard;
