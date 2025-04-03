// React Router is used for handling navigation between different pages
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

// Import the page components for routing
import Home from './pages/Home';
import RecipePage from './pages/RecipePage';
import FavoritesPage from './pages/FavoritesPage';

function App() {
  return (
    // Router wraps the entire app and enables route-based navigation
    <Router>
      <Routes>
        {/* Route for the home page which displays all recipes */}
        <Route path="/" element={<Home />} />

        {/* Route for viewing the details of a specific recipe (by ID) */}
        <Route path="/recipe/:id" element={<RecipePage />} />

        {/* Route for viewing the user's list of favorite recipes */}
        <Route path="/favorites" element={<FavoritesPage />} />
      </Routes>
    </Router>
  );
}

export default App;
