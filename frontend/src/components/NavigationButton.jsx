import React from 'react';
import { Button } from '@mui/material';

const NavigationButton = ({ icon, onClick, label, positionStyles }) => (
    <Button
        startIcon={icon}
        onClick={onClick}
        sx={{
            position: 'fixed',
            zIndex: 10,
            ...positionStyles
        }}
    >
        {label}
    </Button>
);

export default NavigationButton;
