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
    SelectChangeEvent,
    TablePagination,
    TablePaginationActionsSlotPropsOverrides,
    Tooltip,
    IconButton,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogContentText,
    DialogActions,
    CircularProgress,
} from "@mui/material";
import { useDeleteProductMutation, useFetchProductsQuery } from "./productsApi";
import { FormEvent, useState } from "react";
import { Add, CommentOutlined, DeleteOutline, Refresh } from "@mui/icons-material";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { Link, useNavigate } from "react-router-dom";
import { setPageNumber, setPageSize, setSearchTerm, setSortBy } from "./productsSlice";
import { toast } from "react-toastify";

const EmptyAction = (_props: TablePaginationActionsSlotPropsOverrides) => null;

export default function ProductListView() {
    const productListParams = useAppSelector(state => state.products);
    const { data, isLoading, error, refetch } = useFetchProductsQuery(productListParams);
    const [deleteProduct, { isLoading: isDeleting }] = useDeleteProductMutation();
    const [localSearch, setLocalSearch] = useState(productListParams.searchTerm || '');
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    // State for delete confirmation dialog
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [productToDelete, setProductToDelete] = useState<number | null>(null);

    const handleSearchSubmit = (e: FormEvent) => {
        e.preventDefault();
        dispatch(setSearchTerm(localSearch));
    };

    const handleSearchReset = () => {
        setLocalSearch('');
        dispatch(setSearchTerm(''));
    };

    const handleSortChange = (e: SelectChangeEvent) => {
        dispatch(setSortBy(e.target.value));
    };

    const handlePageNumberChange = (_: unknown, newPage: number) => {
        dispatch(setPageNumber(newPage));
    };

    const handlePageSizeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        dispatch(setPageSize(parseInt(e.target.value, 10)));
    };

    const handleRowClick = (productId: number) => {
        navigate(`/products/edit/${productId}`);
    }

    const handleReviewsClick = (e: React.MouseEvent, productId: number) => {
        e.stopPropagation();
        navigate(`/products/${productId}/reviews`);
    }

    const handleDeleteClick = (e: React.MouseEvent, productId: number) => {
        e.stopPropagation();
        setProductToDelete(productId);
        setDeleteDialogOpen(true);
    }

    const confirmDelete = async () => {
        if (productToDelete) {
            try {
                await deleteProduct(productToDelete).unwrap();
                toast.success("Product deleted successfully");
                setDeleteDialogOpen(false);
            } catch (err) {
                console.error("Delete error: " + err);
                toast.error("Error deleting product");
            }
        }
    }

    if (isLoading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return (
            <Box sx={{ p: 3 }}>
                <Typography variant="h6" color="error">
                    Error loading products. Please try again.
                </Typography>
                {error && <Typography>An unexpected error occurred</Typography>}
                <Button
                    variant="contained"
                    onClick={() => refetch()}
                    sx={{ mt: 2 }}
                >
                    Retry
                </Button>
            </Box>
        );
    }

    if (!data || data.items.length === 0) {
        return (
            <Box sx={{ p: 3 }}>
                <Typography variant="h6">No products found.</Typography>
                <Button
                    variant="contained"
                    sx={{ mt: 2 }}
                    onClick={() => {
                        dispatch(setSearchTerm(''));
                        dispatch(setSortBy('name'));
                        dispatch(setPageNumber(1));
                    }}
                >
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
                    <Box component="form" sx={{ display: 'flex' }} onSubmit={handleSearchSubmit}>
                        <TextField
                            fullWidth
                            label="Search products"
                            variant="outlined"
                            value={localSearch}
                            onChange={(e) => setLocalSearch(e.target.value)}
                        />
                        <Button type="submit" variant="contained" sx={{ ml: 1 }}>
                            Search
                        </Button>
                        <Button variant="outlined" sx={{ ml: 1 }} onClick={handleSearchReset}>
                            Reset
                        </Button>
                    </Box>
                </Grid>
            </Grid>

            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography>
                    {data.metadata?.totalCount ?? 0} products found
                </Typography>
                <FormControl sx={{ minWidth: 200 }}>
                    <InputLabel>Sort By</InputLabel>
                    <Select
                        value={productListParams.sortBy}
                        label="Sort By"
                        onChange={handleSortChange}
                    >
                        <MenuItem value="id">Id (Ascending)</MenuItem>
                        <MenuItem value="id-desc">Id (Descending)</MenuItem>
                        <MenuItem value="name">Name (A-Z)</MenuItem>
                        <MenuItem value="name-desc">Name (Z-A)</MenuItem>
                        <MenuItem value="price">Price (Low-High)</MenuItem>
                        <MenuItem value="price-desc">Price (High-Low)</MenuItem>
                        <MenuItem value="created-date">Oldest First</MenuItem>
                        <MenuItem value="created-date-desc">Newest First</MenuItem>
                    </Select>
                </FormControl>
            </Box>

            <Box display="flex" alignItems="center" justifyContent="space-between" sx={{ mb: 2 }}>
                <Button component={Link} to="/products/create" variant="outlined" startIcon={<Add />}>
                    Create
                </Button>
                <Tooltip title="Refresh data">
                    <IconButton onClick={() => refetch()}>
                        <Refresh />
                    </IconButton>
                </Tooltip>
            </Box>

            <TableContainer component={Paper} sx={{ mb: 2 }}>
                <Table sx={{ minWidth: 700 }}>
                    <TableHead>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>Name</TableCell>
                            <TableCell>Base Price</TableCell>
                            <TableCell>Active</TableCell>
                            <TableCell>Category</TableCell>
                            <TableCell>Quantity</TableCell>
                            <TableCell>Created</TableCell>
                            <TableCell>Updated</TableCell>
                            <TableCell align="right">Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {data.items.map(item => (
                            <TableRow
                                key={item.id}
                                onClick={() => handleRowClick(item.id)}
                                hover
                                sx={{
                                    cursor: 'pointer',
                                }}
                            >
                                <TableCell>{item.id}</TableCell>
                                <TableCell>{item.name}</TableCell>
                                <TableCell>${item.basePrice.toFixed(2)}</TableCell>
                                <TableCell>
                                    <Chip
                                        label={item.isActive ? "Active" : "Inactive"}
                                        color={item.isActive ? "success" : "default"}
                                    />
                                </TableCell>
                                <TableCell>{item.categoryName}</TableCell>
                                <TableCell>{item.quantityInStock ?? "N/A"}</TableCell>
                                <TableCell>{new Date(item.createdDate).toLocaleDateString()}</TableCell>
                                <TableCell>
                                    {item.updatedDate
                                        ? new Date(item.updatedDate).toLocaleDateString()
                                        : "Never"}
                                </TableCell>
                                <TableCell align="right" onClick={(e) => e.stopPropagation()}>
                                    <Tooltip title="View reviews">
                                        <IconButton
                                            color="primary"
                                            onClick={(e) => handleReviewsClick(e, item.id)}
                                            sx={{ mr: 1 }}
                                        >
                                            <CommentOutlined />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Delete product">
                                        <IconButton
                                            color="error"
                                            onClick={(e) => handleDeleteClick(e, item.id)}
                                        >
                                            <DeleteOutline />
                                        </IconButton>
                                    </Tooltip>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <Box sx={{ mt: 1, display: 'flex', justifyContent: 'flex-end', alignItems: 'center' }}>
                <TablePagination
                    component="div"
                    count={data.metadata.totalCount || 0}
                    page={(data.metadata.currentPage || 1) - 1}
                    rowsPerPage={data.metadata.pageSize || 10}
                    rowsPerPageOptions={[3, 6, 9, 12]}
                    onPageChange={handlePageNumberChange}
                    onRowsPerPageChange={handlePageSizeChange}
                    ActionsComponent={EmptyAction}
                />
                <Pagination
                    count={data.metadata?.totalPages ?? 0}
                    page={productListParams.pageNumber || 1}
                    color="primary"
                    onChange={handlePageNumberChange}
                />
            </Box>

            <Dialog
                open={deleteDialogOpen}
                onClose={() => setDeleteDialogOpen(false)}
            >
                <DialogTitle>Delete Product</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Are you sure you want to delete this product? This action cannot be undone.
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialogOpen(false)} disabled={isDeleting}>
                        Cancel
                    </Button>
                    <Button onClick={confirmDelete} color="error" disabled={isDeleting}>
                        {isDeleting ? 'Deleting...' : 'Delete'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}
