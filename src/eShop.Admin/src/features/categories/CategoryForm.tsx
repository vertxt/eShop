import { useEffect, useState } from "react";
import { Box, Button, Divider, Grid, IconButton, Paper, TextField, Typography } from "@mui/material";
import { useForm, SubmitHandler, useFieldArray } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { CategoryDetail } from "../../shared/types/category";
import { createCategorySchema, CreateCategorySchema } from "../../shared/schemas/createCategorySchema";
import { useCreateCategoryMutation, useUpdateCategoryMutation } from "./categoriesApi";
import { Add, DeleteOutline } from "@mui/icons-material";

type Props = {
    isEditMode?: boolean;
    existingCategory?: CategoryDetail | null;
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
        attributes: isEditMode && existingCategory?.attributes ?
            existingCategory.attributes.map(attr => ({
                displayName: attr.displayName,
                name: attr.name,
            })) : [],
    };

    const {
        register,
        handleSubmit,
        control,
        formState: { errors },
    } = useForm<CreateCategorySchema>({
        resolver: zodResolver(createCategorySchema),
        defaultValues,
        mode: 'onBlur'
    });

    const { fields, append, remove } = useFieldArray({
        control,
        name: "attributes",
    })

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

    const addNewAttribute = () => {
        append({
            name: "",
            displayName: "",
        })
    }

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
                    <Grid size={12}>
                        <Divider sx={{ my: 2 }} />
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                            <Typography variant="h6">Category Attributes</Typography>
                            <Button
                                variant="outlined"
                                startIcon={<Add />}
                                onClick={addNewAttribute}
                                disabled={isLoading}
                            >
                                Add Attribute
                            </Button>
                        </Box>
                        {fields.map((field, index) => (
                            <Grid key={field.id} container spacing={2} alignItems='center' sx={{ my: '0.5rem' }}>
                                <Grid size={{ xs: 12, md: 5 }}>
                                    <TextField
                                        fullWidth
                                        label="Name"
                                        required
                                        size="small"
                                        disabled={isLoading}
                                        error={!!errors.attributes?.[index]?.displayName}
                                        helperText={errors.attributes?.[index]?.displayName?.message}
                                        {...register(`attributes.${index}.name`)}
                                        placeholder="Color"
                                    />
                                </Grid>

                                <Grid size={{ xs: 12, md: 5 }}>
                                    <TextField
                                        fullWidth
                                        label="Display Name"
                                        required
                                        size="small"
                                        disabled={isLoading}
                                        error={!!errors.attributes?.[index]?.displayName}
                                        helperText={errors.attributes?.[index]?.displayName?.message}
                                        {...register(`attributes.${index}.displayName`)}
                                        placeholder="Color"
                                    />
                                </Grid>

                                <Grid size={{ xs: 12, md: 2 }}>
                                    <IconButton
                                        color="error"
                                        onClick={() => remove(index)}
                                        disabled={isLoading}
                                        aria-label="Remove attribute"
                                    >
                                        <DeleteOutline />
                                    </IconButton>
                                </Grid>
                            </Grid>
                        ))}

                        {fields.length === 0 && (
                            <Typography color="text.secondary" sx={{ mt: 2, mb: 2, fontStyle: 'italic' }}>
                                No attributes defined. Click "Add Attribute" to define attributes for this category.
                            </Typography>
                        )}
                    </Grid>
                </Grid>
            </Box>
        </Paper>
    );
}