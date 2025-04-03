import React from "react";
import { Card, CardMedia, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

const RecipeCard = ({ recipe }) => {
  const navigate = useNavigate();

  return (
    <Card sx={{ width: 300, m: 2, cursor: "pointer" }} onClick={() => navigate(`/recipes/${recipe.id}`)}>
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
