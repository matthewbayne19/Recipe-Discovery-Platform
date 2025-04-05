import React from 'react';
import { TextField, InputAdornment, IconButton } from '@mui/material';
import ClearIcon from '@mui/icons-material/Clear';

const Filter = ({ label, value, onChange, onClear }) => (
    <TextField
        label={label}
        variant="outlined"
        value={value}
        onChange={onChange}
        fullWidth
        InputProps={{
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
