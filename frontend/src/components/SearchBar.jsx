import React from 'react';
import { TextField, InputAdornment, IconButton } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import ClearIcon from '@mui/icons-material/Clear';

const SearchBar = ({ searchTerm, setSearchTerm, handleSearch, handleClearSearch }) => (
    <TextField
        label="Search by name"
        variant="outlined"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        fullWidth
        InputProps={{
            endAdornment: (
            <InputAdornment position="end">
                {searchTerm && (
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
