import { ArrowBack } from "@mui/icons-material";
import { Box, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm";

export default function CategoryCreateWrapper() {
    const navigate = useNavigate();

    const navigateBack = () => {
        navigate(-1);
    }

    const handleSuccess = () => {
        navigate('/categories');
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

            <CategoryForm onSuccess={handleSuccess} />
        </Box>
    );
}