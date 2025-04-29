import { Add, DeleteOutline } from "@mui/icons-material";
import { Alert, Box, Button, FormControlLabel, Grid, InputAdornment, Paper, Switch, TextField, Typography } from "@mui/material";
import { Control, Controller, FieldErrors, useFieldArray } from "react-hook-form";
import { ProductSchema } from "../../shared/schemas/createProductSchema";

type Props = {
    control: Control<ProductSchema>,
    errors: FieldErrors<ProductSchema>,
    hasVariants: boolean,
}

export default function VariantsTab({ control, errors, hasVariants }: Props) {
    const { fields, append, remove } = useFieldArray({
        control,
        name: "variants",
    });

    return (
        <>
            <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom>
                    Product Variants
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {hasVariants
                        ? "Add product variants such as different sizes, colors, etc."
                        : "Enable 'Has Variants' in the Basic Information tab to add product variants."}
                </Typography>
            </Box>

            {hasVariants ? (
                <>
                    {fields.map((field, index) => (
                        <Paper key={field.id} elevation={1} sx={{ mb: 3, p: 2 }}>
                            <Typography variant="subtitle1" gutterBottom>
                                Variant #{index + 1}
                            </Typography>
                            <Grid container spacing={2}>
                                <Controller
                                    name={`variants.${index}.id`}
                                    control={control}
                                    shouldUnregister={true}
                                    render={({ field }) => (
                                        <input type="hidden" {...field} />
                                    )}
                                />

                                <Grid size={{ xs: 12, md: 6 }}>
                                    <Controller
                                        name={`variants.${index}.name`}
                                        control={control}
                                        defaultValue={field.name}
                                        render={({ field }) => (
                                            <TextField
                                                {...field}
                                                label="Variant Name"
                                                fullWidth
                                                variant="outlined"
                                                error={!!errors.variants?.[index]?.name}
                                                helperText={errors.variants?.[index]?.name?.message}
                                            />
                                        )}
                                    />
                                </Grid>

                                <Grid size={{ xs: 12, md: 6 }}>
                                    <Controller
                                        name={`variants.${index}.sku`}
                                        control={control}
                                        defaultValue={field.sku || ''}
                                        render={({ field }) => (
                                            <TextField
                                                {...field}
                                                label="SKU"
                                                fullWidth
                                                variant="outlined"
                                                error={!!errors.variants?.[index]?.sku}
                                                helperText={errors.variants?.[index]?.sku?.message}
                                            />
                                        )}
                                    />
                                </Grid>

                                <Grid size={{ xs: 12, md: 6 }}>
                                    <Controller
                                        name={`variants.${index}.price`}
                                        control={control}
                                        defaultValue={field.price}
                                        render={({ field }) => (
                                            <TextField
                                                {...field}
                                                label="Price"
                                                fullWidth
                                                variant="outlined"
                                                type="number"
                                                slotProps={{
                                                    input: {
                                                        startAdornment: <InputAdornment position="start">$</InputAdornment>
                                                    }
                                                }}
                                                error={!!errors.variants?.[index]?.price}
                                                helperText={errors.variants?.[index]?.price?.message}
                                            />
                                        )}
                                    />
                                </Grid>

                                <Grid size={{ xs: 12, md: 6 }}>
                                    <Controller
                                        name={`variants.${index}.quantityInStock`}
                                        control={control}
                                        defaultValue={field.quantityInStock}
                                        render={({ field }) => (
                                            <TextField
                                                {...field}
                                                label="Quantity in Stock"
                                                type="number"
                                                fullWidth
                                                variant="outlined"
                                                error={!!errors.variants?.[index]?.quantityInStock}
                                                helperText={errors.variants?.[index]?.quantityInStock?.message?.toString()}
                                            />
                                        )}
                                    />
                                </Grid>

                                <Grid size={12}>
                                    <Controller
                                        name={`variants.${index}.isActive`}
                                        control={control}
                                        defaultValue={field.isActive}
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
                            </Grid>

                            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'flex-end' }}>
                                <Button
                                    variant="outlined"
                                    color="error"
                                    startIcon={<DeleteOutline />}
                                    onClick={() => remove(index)}
                                >
                                    Remove Variant
                                </Button>
                            </Box>
                        </Paper>
                    ))}

                    <Button
                        variant="outlined"
                        startIcon={<Add />}
                        onClick={() => append({
                            id: undefined,
                            name: '',
                            sku: '',
                            price: 0,
                            quantityInStock: 0,
                            isActive: true,
                        })}
                        sx={{ mt: 2 }}
                    >
                        Add Variant
                    </Button>
                </>
            ) : (
                <Alert severity="info" sx={{ mt: 2 }}>
                    To add variants, go to Basic Information tab and enable "Has Variants"
                </Alert>
            )}
        </>
    )
}
