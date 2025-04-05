import React from 'react';
import {
  Box, FormControl, InputLabel, Select, MenuItem, Pagination
} from '@mui/material';

// PaginationControl component props
const PaginationControl = ({
  pageSize,
  setPageSize,
  totalPages,
  page,
  setPage,
  isLoading
}) => {
  return (
    <Box
      mt="3vh"
      borderTop="1px solid #ddd"
      py={2}
      display="flex"
      justifyContent="center"
      alignItems="center"
      gap={4}
      sx={{ backgroundColor: 'white' }}
    >
      <FormControl size="small" disabled={isLoading} sx={{ minWidth: 120 }}>
        <InputLabel>Per Page</InputLabel>
        <Select
          value={pageSize}
          label="Per Page"
          onChange={(e) => {
            setPageSize(e.target.value);
            setPage(1);
          }}
        >
          {[9, 18, 27, 36].map(size => (
            <MenuItem key={size} value={size}>{size}</MenuItem>
          ))}
        </Select>
      </FormControl>

      <Pagination
        count={totalPages}
        page={page}
        onChange={(_, newPage) => setPage(newPage)}
        color="primary"
        disabled={isLoading}
      />
    </Box>
  );
};

export default PaginationControl;
