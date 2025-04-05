import React from 'react';
import { TextField, InputAdornment, IconButton } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';

// Defines a Filter component with a clearable TextField.
const Filter = ({ label, value, onChange, onClear }) => (
    <TextField
        label={label}  // Text field label
        variant="outlined"  // Style variant of the text field
        value={value}  // Current value of the text field
        onChange={onChange}  // Function to call when the value changes
        fullWidth  // Ensures the text field occupies the full width available
        InputProps={{  // Props for configuring the end adornment
            endAdornment: value && (  
                <InputAdornment position="end">
                    <IconButton onClick={onClear} edge="end">  
                        <ClearIcon />
                    </IconButton>
                </InputAdornment>
            ),
        }}
    />
);

export default Filter;
