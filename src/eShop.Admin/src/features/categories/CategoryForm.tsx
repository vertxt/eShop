import { useState } from "react";
import { Box, Button, Grid, Paper, TextField, Typography } from "@mui/material";
import { useForm, SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Category } from "../../shared/types/category";
import { createCategorySchema, CreateCategorySchema } from "../../shared/schemas/createCategorySchema";
import { useCreateCategoryMutation, useUpdateCategoryMutation } from "./categoriesApi";

type Props = {
    isEditMode?: boolean;
    existingCategory?: Category | null;
    onSuccess?: () => void;
};

export default function CategoryForm({ isEditMode = false, existingCategory = null, onSuccess }: Props) {
    const [createCategory, { isLoading: isCreating }] = useCreateCategoryMutation();
    const [updateCategory, { isLoading: isUpdating }] = useUpdateCategoryMutation();
    const [error, setError] = useState<string | null>(null);

    const isLoading = isCreating || isUpdating;

    const defaultValues: CreateCategorySchema = {
        name: isEditMode && existingCategory?.name ? existingCategory.name : "",
        description: isEditMode && existingCategory?.description ? existingCategory.description : "",
    };

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<CreateCategorySchema>({
        resolver: zodResolver(createCategorySchema),
        defaultValues,
        mode: 'onBlur'
    });

    const onSubmit: SubmitHandler<CreateCategorySchema> = async (data) => {
        try {
            setError(null);
            if (isEditMode && existingCategory) {
                await updateCategory({ id: existingCategory.id, data }).unwrap();
            } else {
                await createCategory(data).unwrap();
            }
            onSuccess?.();
        } catch (err) {
            console.error("Submission error", err);
            setError("Failed to save category. Please try again.");
        }
    };

    return (
        <Paper sx={{ p: 3 }}>
            <Typography variant="h5" gutterBottom>
                {isEditMode ? "Edit Category" : "Add New Category"}
            </Typography>

            {error && (
                <Typography color="error" sx={{ mb: 2 }}>
                    {error}
                </Typography>
            )}

            <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
                <Grid container spacing={2}>
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            label="Name"
                            required
                            error={!!errors.name}
                            helperText={errors.name?.message}
                            {...register("name")}
                            disabled={isLoading}
                        />
                    </Grid>
                    <Grid size={12}>
                        <TextField
                            fullWidth
                            label="Description"
                            multiline
                            rows={3}
                            error={!!errors.description}
                            helperText={errors.description?.message}
                            {...register("description")}
                            disabled={isLoading}
                        />
                    </Grid>
                    <Grid size={12}>
                        <Button
                            type="submit"
                            variant="contained"
                            loading={isLoading}
                        >
                            {isEditMode ? "Update" : "Create"}
                        </Button>
                    </Grid>
                </Grid>
            </Box>
        </Paper>
    );
}