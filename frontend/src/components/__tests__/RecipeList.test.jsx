import React from "react";
import { render, screen } from "@testing-library/react";
import RecipeList from "../RecipeList";

// Mock the RecipeCard component to isolate the test to only RecipeList functionality.
// This prevents the test from failing due to unrelated changes in RecipeCard.
jest.mock("../RecipeCard", () => ({ recipe }) => (
  <div data-testid="recipe-card">{recipe.name}</div>  // Mock returns a simplified div with a test ID and displays the recipe name.
));

// Sample data to use in tests. This simulates the data that the RecipeList component would receive.
const mockRecipes = [
  { id: "1", name: "Pizza", cuisine: "Italian", imageUrl: "" },
  { id: "2", name: "Sushi", cuisine: "Japanese", imageUrl: "" },
  { id: "3", name: "Tacos", cuisine: "Mexican", imageUrl: "" }
];

// Describe block defines a test suite for the RecipeList component.
describe("RecipeList", () => {
  // 'it' defines an individual test case.
  // This test checks if RecipeList renders the correct number of RecipeCard components based on the mock data provided.
  it("renders the correct number of RecipeCard components", () => {
    render(<RecipeList recipes={mockRecipes} />);  // Render the RecipeList component with the mock recipe data.
    const cards = screen.getAllByTestId("recipe-card");  // Retrieve all elements in the rendered output that have the 'recipe-card' test ID.
    expect(cards.length).toBe(3);  // Expect the number of rendered cards to be three.
    expect(cards[0]).toHaveTextContent("Pizza");  // Further asserts to check if each card contains the correct content.
    expect(cards[1]).toHaveTextContent("Sushi");
    expect(cards[2]).toHaveTextContent("Tacos");
  });
});  
