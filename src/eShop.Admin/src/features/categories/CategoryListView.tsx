import { 
  Box, 
  Button, 
  ButtonGroup, 
  Paper, 
  Table, 
  TableBody, 
  TableCell, 
  TableContainer, 
  TableHead, 
  TableRow, 
  Toolbar, 
  Typography,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  CircularProgress,
  Alert,
} from "@mui/material";
import { useDeleteCategoryMutation, useFetchCategoriesQuery } from "./categoriesApi";
import { Link, useNavigate } from "react-router-dom";
import { Add, Edit, Delete } from "@mui/icons-material";
import { useState } from "react";
import { Category } from "../../shared/types/category";
import { toast } from "react-toastify";

export default function CategoryListView() {
  const { data, isLoading, error, refetch } = useFetchCategoriesQuery();
  const [deleteCategory, { isLoading: isDeleting }] = useDeleteCategoryMutation();
  const navigate = useNavigate();
  
  // State for delete confirmation dialog
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [categoryToDelete, setCategoryToDelete] = useState<Category | null>(null);
  
  const handleEditClick = (category: Category) => {
    navigate(`/categories/edit/${category.id}`, { state: { category } });
  };

  const handleDeleteClick = (category: Category) => {
    setCategoryToDelete(category);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!categoryToDelete) return;
    
    try {
      await deleteCategory(categoryToDelete.id).unwrap();
      toast.success(`Category "${categoryToDelete.name}" deleted successfully`);
      // Refresh the list after deletion
      // refetch();
    } catch (err) {
      console.error("Delete error:", err);
      toast.error("Failed to delete category. Please try again.");
    } finally {
      setDeleteDialogOpen(false);
      setCategoryToDelete(null);
    }
  };

  if (isLoading) {
    return (
      <Box sx={{ display: "flex", justifyContent: "center", p: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ m: 2 }}>
        Failed to load categories. Please try again later.
      </Alert>
    );
  }

  return (
    <Box sx={{ width: "100%", height: "100%", display: "flex", flexDirection: "column", p: 2 }}>
      <Typography variant="h5" sx={{ mb: 2 }}>
        Categories
      </Typography>

      <Toolbar sx={{ p: 0, mb: 2 }}>
        <Button
          component={Link}
          to="/categories/create"
          variant="contained"
          color="primary"
          startIcon={<Add />}
          sx={{ mr: 2 }}
        >
          Create New Category
        </Button>
      </Toolbar>

      {data && data.length > 0 ? (
        <TableContainer component={Paper}>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Name</TableCell>
                <TableCell>Description</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {data.map((category) => (
                <TableRow key={category.id} hover>
                  <TableCell>{category.id}</TableCell>
                  <TableCell>{category.name}</TableCell>
                  <TableCell>{category.description}</TableCell>
                  <TableCell align="right">
                    <ButtonGroup size="small">
                      <Button
                        startIcon={<Edit />}
                        onClick={() => handleEditClick(category)}
                        color="primary"
                        size="small"
                      >
                        Edit
                      </Button>
                      <Button
                        startIcon={<Delete />}
                        onClick={() => handleDeleteClick(category)}
                        color="error"
                        size="small"
                      >
                        Delete
                      </Button>
                    </ButtonGroup>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      ) : (
        <Alert severity="info">No categories found. Create your first one!</Alert>
      )}

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Confirm Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete the category "{categoryToDelete?.name}"? 
            This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)} disabled={isDeleting}>
            Cancel
          </Button>
          <Button 
            onClick={handleDeleteConfirm} 
            color="error" 
            variant="contained"
            loading={isDeleting}
          >
            {isDeleting ? "Deleting..." : "Delete"}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}