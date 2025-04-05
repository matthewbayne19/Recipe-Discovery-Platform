import React from "react";
import { render, screen } from "@testing-library/react";
import RecipeList from "../RecipeList";

// Mock RecipeCard to isolate RecipeList
jest.mock("../RecipeCard", () => ({ recipe }) => (
  <div data-testid="recipe-card">{recipe.name}</div>
));

const mockRecipes = [
  { id: "1", name: "Pizza", cuisine: "Italian", imageUrl: "" },
  { id: "2", name: "Sushi", cuisine: "Japanese", imageUrl: "" },
  { id: "3", name: "Tacos", cuisine: "Mexican", imageUrl: "" }
];

describe("RecipeList", () => {
  it("renders the correct number of RecipeCard components", () => {
    render(<RecipeList recipes={mockRecipes} />);
    const cards = screen.getAllByTestId("recipe-card");
    expect(cards.length).toBe(3);
    expect(cards[0]).toHaveTextContent("Pizza");
    expect(cards[1]).toHaveTextContent("Sushi");
    expect(cards[2]).toHaveTextContent("Tacos");
  });
});
