# Recipe Discovery Platform

A full-stack recipe discovery web application built with a .NET backend and React frontend. Users can browse, filter, search, and favorite recipes. Both REST and GraphQL APIs are used to demonstrate flexibility and effective data management.

## Tech Stack

- **Frontend**: React, React Router, MUI (Material UI), Apollo Client
- **Backend**: .NET, C#, HotChocolate (GraphQL)
- **API**: TheMealDB (external REST API)
- **Data Caching**: LocalStorage (browser)

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/matthewbayne19/Bayne-Recipe-Discovery-Solution.git
```

---

### 2. Start the Backend

Make sure you have the .NET SDK installed.

```bash
cd Bayne-Recipe-Discovery-Solution/RecipeDiscovery
dotnet run
```

This will start the backend server on `http://localhost:5011`.

---

### 3. Start the Frontend

```bash
cd Bayne-Recipe-Discovery-Solution/frontend
npm install
npm start
```

This starts the development server at `http://localhost:3000`.

---

### 4. Run Tests

```bash
npm test
```

This runs unit tests using **React Testing Library**.

---

## Features & Design Decisions

### API Integration

- **REST API** was used for:
  - Fetching all recipes (with filters)
  - Searching recipes by name

- **GraphQL** was used for:
  - Fetching recipe by ID
  - Fetching user favorites
  - Adding/removing favorites

This dual approach demonstrates flexibility, with a preference for GraphQL due to its efficiency and better fit for structured queries like retrieving nested favorite data.

---

### User Interface & Experience

- **MUI** was used for UI components including Cards, Buttons, Grid, Alerts, Snackbars, etc.
- Responsive **card grid layout** for browsing recipes, showing essential info up front.
- Clicking a card opens a detailed view with full information and favorites functionality.
- Consistent layout across all screens.
- Card hover effect with scale-up for interactive responsiveness.
- A **snackbar** provides user feedback when adding/removing favorites.
- Loading states handled with **MUI CircularProgress** and **Alerts**.
- Clean, minimal UI that stays out of the way of the core experience.

---

### Navigation & Layout

- Persistent **"View Favorites"** button in the top-right across pages for easy access.
- Consistent **"Back to Home"** button in the top-left for navigation.
- Pagination implemented with selectable items per page (9, 18, etc.) to maintain a neat 3-column grid.

---

### Filtering & Searching

- Search bar and filters are placed at the top of the home page for visibility.
- Users can filter by cuisine, ingredient, or both in conjunction.
- Clear icons next to each input for fast removal of filters/search terms.
- Search is executed only on button press to prevent unnecessary API calls.
- If the search is cleared, the default recipe list is restored.
- Loading spinner appears while the app resets back to default recipes after search is cleared.

---

### Favorites

- User can favorite/unfavorite from both the detail page and view favorites list.
- The "Add to Favorites" button switches to "Remove from Favorites" when already favorited.
- Ingredients list has checkboxes for users to tick off what they have or need.
- Favorites page shows all favorited recipes using the same card layout.

---

### Performance

- Recipes are cached in **localStorage** per page and page size to minimize API calls and improve performance.
- Individual recipes are cached after being fetched once for fast repeat viewing.
```