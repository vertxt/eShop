import { useNavigate, useParams, useLocation } from "react-router-dom";
import { useFetchCategoryByIdQuery } from "./categoriesApi";
import CategoryForm from "./CategoryForm";
import { Box, CircularProgress, Typography, Button } from "@mui/material";
import { ArrowBack } from "@mui/icons-material";

export default function CategoryEditWrapper() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const location = useLocation();

    // Try to get the category from location state first (passed from list)
    const categoryFromState = location.state?.category;

    // If not available in state, fetch it from API
    const { data: categoryFromApi, isLoading, error } = useFetchCategoryByIdQuery(
        Number(id)!,
        { skip: !!categoryFromState }
    );

    const category = categoryFromState || categoryFromApi;

    const handleSuccess = () => {
        navigate("/categories");
    };

    const navigateBack = () => {
        navigate("/categories");
    };

    if (isLoading) {
        return (
            <Box sx={{ display: "flex", justifyContent: "center", p: 4 }}>
                <CircularProgress />
            </Box>
        );
    }

    if (error || (!isLoading && !category)) {
        return (
            <Box sx={{ p: 3 }}>
                <Typography color="error" variant="h6">
                    Category not found or could not be loaded
                </Typography>
                <Button
                    startIcon={<ArrowBack />}
                    onClick={navigateBack}
                    sx={{ mt: 2 }}
                >
                    Back to Categories
                </Button>
            </Box>
        );
    }

    return (
        <Box sx={{ p: 2 }}>
            <Button
                startIcon={<ArrowBack />}
                onClick={navigateBack}
                sx={{ mb: 2 }}
            >
                Back to Categories
            </Button>

            <CategoryForm
                isEditMode={true}
                existingCategory={category}
                onSuccess={handleSuccess}
            />
        </Box>
    );
}