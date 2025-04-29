import { Control, Controller, FieldErrors } from 'react-hook-form';
import {
    Grid, TextField, FormControl, InputLabel, Select, MenuItem,
    FormHelperText, Switch, FormControlLabel, InputAdornment
} from '@mui/material';
import { ProductSchema } from '../../shared/schemas/createProductSchema';
import { Category } from '../../shared/types/category';


type BasicInfoTabProps = {
    control: Control<ProductSchema>;
    errors: FieldErrors<ProductSchema>;
    categories?: Category[];
    isLoadingCategories: boolean;
    hasVariants: boolean;
};

export default function BasicInfoTab({
    control,
    errors,
    categories,
    isLoadingCategories,
    hasVariants
}: BasicInfoTabProps) {
    return (
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
    );
}