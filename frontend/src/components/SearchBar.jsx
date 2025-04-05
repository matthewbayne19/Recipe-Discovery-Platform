import React from 'react';
import { TextField, InputAdornment, IconButton } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';

// Component for a search input field with integrated search and clear buttons
const SearchBar = ({ searchTerm, setSearchTerm, handleSearch, handleClearSearch }) => (
    <TextField
        label="Search by name" // Label displayed above the TextField
        variant="outlined" // TextField style variant
        value={searchTerm} // Controlled value of the TextField
        onChange={(e) => setSearchTerm(e.target.value)} // Handler for updating the search term on input change
        fullWidth // TextField occupies the full width of its parent container
        InputProps={{
            endAdornment: ( // Adds icons to the end of the TextField
                <InputAdornment position="end">
                    {searchTerm && ( // Clear button shows only when there is text in the TextField
                        <IconButton onClick={handleClearSearch} edge="end">
                            <ClearIcon /> 
                        </IconButton>
                    )}
                    <IconButton onClick={handleSearch} edge="end"> 
                        <SearchIcon /> 
                    </IconButton>
                </InputAdornment>
            ),
        }}
    />
);

export default SearchBar;
