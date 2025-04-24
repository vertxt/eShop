import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Box,
    Typography,
    Grid,
    TextField,
    MenuItem,
    Select,
    FormControl,
    InputLabel,
    Pagination,
    Button,
    Chip,
} from "@mui/material";
import { useFetchProductsQuery } from "./productsApi";
import { useState } from "react";
import { Add } from "@mui/icons-material";
import { useAppSelector } from "../../app/store/store";

export default function ProductListView() {
    const productListParams = useAppSelector(state => state.products);
    const { data, isLoading, error } = useFetchProductsQuery(productListParams);
    const [localSearch, setLocalSearch] = useState(productListParams.searchTerm || '');

    if (isLoading) {
        return (
            <Box sx={{ p: 3 }}>
                <Typography variant="h6">Loading products...</Typography>
            </Box>
        );
    }

    if (error) {
        return (
            <Box sx={{ p: 3 }}>
                <Typography variant="h6" color="error">
                    Error loading products. Please try again.
                </Typography>
                <pre>{JSON.stringify(error, null, 2)}</pre>
            </Box>
        );
    }

    if (!data || data.items.length === 0) {
        return (
            <Box sx={{ p: 3 }}>
                <Typography variant="h6">No products found.</Typography>
                <Button variant="contained" sx={{ mt: 2 }}>
                    Reset Filters
                </Button>
            </Box>
        );
    }

    return (
        <Box sx={{ width: '100%', height: '100%', display: 'flex', flexDirection: 'column', p: 2 }}>
            <Grid container spacing={3} sx={{ mb: 3 }}>
                <Grid size={{ xs: 12, md: 6 }}>
                    <Typography variant="h5">Products</Typography>
                </Grid>
                <Grid size={{ xs: 12, md: 6 }}>
                    <Box component="form" sx={{ display: 'flex' }}>
                        <TextField
                            fullWidth
                            label="Search products"
                            variant="outlined"
                            size="small"
                            value={localSearch}
                            onChange={(e) => setLocalSearch(e.target.value)}
                        />
                        <Button type="submit" variant="contained" sx={{ ml: 1 }}>
                            Search
                        </Button>
                        <Button variant="outlined" sx={{ ml: 1 }}>
                            Reset
                        </Button>
                    </Box>
                </Grid>
            </Grid>

            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography>
                    {data.metadata?.totalCount ?? 0} products found
                </Typography>
                <FormControl sx={{ minWidth: 200 }} size="small">
                    <InputLabel>Sort By</InputLabel>
                    <Select
                        value={productListParams.sortBy}
                        label="Sort By"
                    >
                        <MenuItem value="name">Name (A-Z)</MenuItem>
                        <MenuItem value="nameDesc">Name (Z-A)</MenuItem>
                        <MenuItem value="priceAsc">Price (Low-High)</MenuItem>
                        <MenuItem value="priceDesc">Price (High-Low)</MenuItem>
                        <MenuItem value="newest">Newest First</MenuItem>
                    </Select>
                </FormControl>
            </Box>
            
            <Box>
                <Button>
                    <Add />
                    Create
                </Button>
            </Box>

            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 700 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Base Price</TableCell>
                            <TableCell>Active</TableCell>
                            <TableCell>Category</TableCell>
                            <TableCell>Has Variants</TableCell>
                            <TableCell>Quantity</TableCell>
                            <TableCell>Created</TableCell>
                            <TableCell>Updated</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data.items.map(item => (
                            <TableRow key={item.id}>
                                <TableCell>{item.id}</TableCell>
                                <TableCell>{item.name}</TableCell>
                                <TableCell>${item.basePrice.toFixed(2)}</TableCell>
                                <TableCell>
                                    <Chip
                                        label={item.isActive ? "Active" : "Inactive"}
                                        color={item.isActive ? "success" : "default"}
                                        size="small"
                                    />
                                </TableCell>
                                <TableCell>{item.categoryName}</TableCell>
                                <TableCell>{item.hasVariants ? "Yes" : "No"}</TableCell>
                                <TableCell>{item.quantityInStock ?? "N/A"}</TableCell>
                                <TableCell>{new Date(item.createdDate).toLocaleDateString()}</TableCell>
                                <TableCell>
                                    {item.updatedDate
                                        ? new Date(item.updatedDate).toLocaleDateString()
                                        : "Never"}
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
                <Pagination
                    count={data.metadata?.totalPages ?? 0}
                    page={productListParams.pageNumber}
                    color="primary"
                />
            </Box>
        </Box>
    );
}
