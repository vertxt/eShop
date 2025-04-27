import { SyntheticEvent, useState } from "react";
import { useFetchCategoriesQuery } from "../categories/categoriesApi";
import { useCreateProductMutation, useFetchProductDetailsQuery, useUpdateProductMutation } from "./productsApi";
import { Controller, FieldValues, useForm } from "react-hook-form";
import { productSchema, ProductSchema } from "../../shared/schemas/createProductSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-toastify";
import { Box, Button, Card, CardContent, FormControl, FormControlLabel, FormHelperText, Grid, InputAdornment, InputLabel, MenuItem, Select, Switch, Tab, Tabs, TextField, Typography } from "@mui/material";
import TabPanel from "../../shared/components/TabPanel";
import { Save } from "@mui/icons-material";

type Props = {
    productId?: number | null,
    onSuccess?: () => void,
}

export default function ProductForm({ productId, onSuccess }: Props) {
    const [activeTab, setActiveTab] = useState<number>(0);
    // const isEditMode = Boolean(productId);

    const { data: categories, isLoading: isLoadingCategories } = useFetchCategoriesQuery();
    // const { data: existingProduct, isLoading: isLoadingProduct } = useFetchProductDetailsQuery(
    //     productId!,
    //     { skip: isEditMode }
    // );
    const [createProduct, { isLoading: isAddingProduct }] = useCreateProductMutation();
    // const [updateProduct, { isLoading: isUpdatingProduct }] = useUpdateProductMutation();

    // const isSubmitting = isAddingProduct || isUpdatingProduct;

    const { control, handleSubmit, watch, reset, setValue, formState: { errors } } = useForm<ProductSchema>({
        resolver: zodResolver(productSchema),
        defaultValues: {
            name: '',
            basePrice: 0,
            description: '',
            shortDescription: '',
            categoryId: 1,
            isActive: true,
            hasVariants: false,
            quantityInStock: 0,
        }
    });

    const hasVariants = watch('hasVariants');

    // const createFormData = (items: FieldValues) => {
    //     const formData = new FormData();
    //     for (const key in items) {
    //         formData.append(key, items[key]);
    //     }

    //     return formData;
    // };

    const onSubmit = async (data: ProductSchema) => {
        try {
            // const productData = createFormData(data);

            // if (isEditMode && productId) {
            //     await updateProduct({ id: productId, data: productData }).unwrap();
            // } else {
            //     await createProduct(productData).unwrap();
            // }
            console.log(data);
            await createProduct(data);
            toast.success("Product created successfully!");

            if (onSuccess) onSuccess();
        } catch (error) {
            toast.error(`Failed to save product: ${error}`);
        }
    };

    const handleTabChange = (_event: SyntheticEvent, newValue: number): void => {
        setActiveTab(newValue);
    };

    return (
        <Card>
            <CardContent>
                <Box component='form' onSubmit={handleSubmit(onSubmit)}>
                    <Typography variant="h5" gutterBottom>
                        {/* {isEditMode ? "Edit Product" : "Add New Product"} */}
                        Add New Product
                    </Typography>

                    <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 2 }}>
                        <Tabs value={activeTab} onChange={handleTabChange} aria-label="product form tabs">
                            <Tab label="Basic information" />
                            <Tab label="Attributes" />
                            <Tab label="Images" />
                            <Tab label="Variants" />
                        </Tabs>
                    </Box>

                    <TabPanel value={activeTab} index={0}>
                        <Grid container spacing={3}>
                            <Grid size={{ xs: 12, md: 6 }}>
                                <Controller
                                    name="name"
                                    control={control}
                                    render={({ field }) => (
                                        <TextField
                                            {...field}
                                            label="Product Name"
                                            fullWidth
                                            variant="outlined"
                                            error={!!errors.name}
                                            helperText={errors.name?.message}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid size={{ xs: 12, md: 6 }}>
                                <Controller
                                    name="categoryId"
                                    control={control}
                                    render={({ field }) => (
                                        <FormControl fullWidth error={!!errors.categoryId}>
                                            <InputLabel>Category</InputLabel>
                                            <Select
                                                {...field}
                                                label="Category"
                                                disabled={isLoadingCategories}
                                            >
                                                {isLoadingCategories ? (
                                                    <MenuItem value={1}>Loading categories...</MenuItem>
                                                ) : (
                                                    categories?.map((category) => (
                                                        <MenuItem key={category.id} value={category.id}>
                                                            {category.name}
                                                        </MenuItem>
                                                    ))
                                                )}
                                            </Select>
                                            {errors.categoryId && (
                                                <FormHelperText>{errors.categoryId.message}</FormHelperText>
                                            )}
                                        </FormControl>
                                    )}
                                />
                            </Grid>

                            <Grid size={{ xs: 12, md: 6 }}>
                                <Controller
                                    name="basePrice"
                                    control={control}
                                    render={({ field }) => (
                                        <TextField
                                            {...field}
                                            label="Base Price"
                                            fullWidth
                                            variant="outlined"
                                            type="number"
                                            slotProps={{
                                                input: {
                                                    startAdornment: <InputAdornment position="start">$</InputAdornment>
                                                }
                                            }}
                                            error={!!errors.basePrice}
                                            helperText={errors.basePrice?.message}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid size={{ xs: 12, md: 6 }}>
                                {!hasVariants && (
                                    <Controller
                                        name="quantityInStock"
                                        control={control}
                                        render={({ field }) => (
                                            <TextField
                                                {...field}
                                                label="Quantity in Stock"
                                                type="number"
                                                fullWidth
                                                variant="outlined"
                                                error={!!errors.quantityInStock}
                                                helperText={errors.quantityInStock?.message?.toString()}
                                            />
                                        )}
                                    />
                                )}
                            </Grid>

                            <Grid size={{ xs: 12, md: 6 }}>
                                <Controller
                                    name="isActive"
                                    control={control}
                                    render={({ field }) => (
                                        <FormControlLabel
                                            control={
                                                <Switch
                                                    checked={field.value}
                                                    onChange={(e) => field.onChange(e.target.checked)}
                                                    color="primary"
                                                />
                                            }
                                            label="Active (visible to customers)"
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid size={{ xs: 12, md: 6 }}>
                                <Controller
                                    name="hasVariants"
                                    control={control}
                                    render={({ field }) => (
                                        <FormControlLabel
                                            control={
                                                <Switch
                                                    checked={field.value}
                                                    onChange={(e) => field.onChange(e.target.checked)}
                                                    color="primary"
                                                />
                                            }
                                            label="Has Variants (sizes, colors, etc.)"
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid size={12}>
                                <Controller
                                    name="shortDescription"
                                    control={control}
                                    render={({ field }) => (
                                        <TextField
                                            {...field}
                                            label="Short Description"
                                            fullWidth
                                            multiline
                                            rows={2}
                                            variant="outlined"
                                            error={!!errors.shortDescription}
                                            helperText={errors.shortDescription?.message}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid size={12}>
                                <Controller
                                    name="description"
                                    control={control}
                                    render={({ field }) => (
                                        <TextField
                                            {...field}
                                            label="Description"
                                            fullWidth
                                            multiline
                                            rows={4}
                                            variant="outlined"
                                            error={!!errors.description}
                                            helperText={errors.description?.message}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </TabPanel>

                    <Box sx={{ mt: 4, display: 'flex', justifyContent: 'space-between' }}>
                        <Button variant="outlined" onClick={() => handleTabChange(null as any, Math.max(0, activeTab - 1))} disabled={activeTab === 0}>
                            Previous
                        </Button>

                        <Box>
                            {activeTab < 3 && (
                                <Button variant="outlined" onClick={() => handleTabChange(null as any, Math.min(3, activeTab + 1))} sx={{ mr: 2 }}>
                                    Next
                                </Button>
                            )}

                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                startIcon={<Save />}
                                loading={isAddingProduct}
                            >
                                {/* {isEditMode ? 'Update Product' : 'Create Product'} */}
                                Create Product
                            </Button>
                        </Box>
                    </Box>
                </Box>
            </CardContent>
        </Card>
    )
}