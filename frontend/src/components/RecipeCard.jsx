import React from "react";
import { useNavigate } from "react-router-dom";
import { Card, CardMedia, CardContent, Typography, CardActionArea } from "@mui/material";

// RecipeCard component displays a single recipe with image and basic info
const RecipeCard = ({ recipe }) => {
  const navigate = useNavigate();

  // Enhance thumbnail quality and sizing using MealDB's /medium variant
  const getImageUrl = (url) => {
    if (!url) return "";
    return url.replace("/images/media/meals/", "/images/media/meals/") + "/medium";
  };

  const handleClick = () => {
    navigate(`/recipes/${recipe.id}`);
  };

  return (
    <Card sx={{ width: 320, height: 320 }}>
      <CardActionArea onClick={handleClick} sx={{ height: "100%" }}>
        <CardMedia
          component="img"
          height="180"
          image={getImageUrl(recipe.imageUrl)}
          alt={recipe.name}
        />
        <CardContent>
          <Typography gutterBottom variant="h6" component="div" noWrap>
            {recipe.name}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Cuisine: {recipe.cuisine}
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default RecipeCard;
