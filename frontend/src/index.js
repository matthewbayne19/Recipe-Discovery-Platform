import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { ApolloClient, InMemoryCache, ApolloProvider } from "@apollo/client";

// Initialize ApolloClient with specific configuration
const client = new ApolloClient({
  uri: "http://localhost:5011/graphql", // URI for the GraphQL server
  cache: new InMemoryCache(), // Setup a new in-memory cache
  headers: {
    "X-API-KEY": "simple-api-key"
  }
});

// Get the root element from the DOM
const root = ReactDOM.createRoot(document.getElementById("root"));

// Render the App component wrapped with ApolloProvider to provide GraphQL capabilities to the entire app
root.render(
  <ApolloProvider client={client}>
    <App />
  </ApolloProvider>
);
