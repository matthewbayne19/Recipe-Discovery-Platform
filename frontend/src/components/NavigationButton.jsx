import React from 'react';
import { Button } from '@mui/material';

// Component for a fixed position navigation button with an icon
const NavigationButton = ({ icon, onClick, label, positionStyles }) => (
    <Button
        startIcon={icon} // Icon to display before the button label
        onClick={onClick} // Handler for click events
        sx={{
            position: 'fixed', // Fixes button position relative to the viewport
            zIndex: 10, // Stack order of the button
            ...positionStyles // Additional styling for position customization
        }}
    >
        {label} {/* Text label of the button */}
    </Button>
);

export default NavigationButton;
