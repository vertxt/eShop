import { useNavigate, useParams } from "react-router-dom";
import CategoryForm from "./CategoryForm";
import { Box, CircularProgress, Typography, Button } from "@mui/material";
import { ArrowBack } from "@mui/icons-material";
import { useFetchCategoryDetailsQuery } from "./categoriesApi";

export default function CategoryEditWrapper() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const { data: category, isLoading, error } = useFetchCategoryDetailsQuery(
        Number(id)!,
        { skip: !id }
    );

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