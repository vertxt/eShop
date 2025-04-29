import { useEffect } from 'react';
import { Control, Controller, FieldErrors, useFieldArray } from 'react-hook-form';
import {
    Box, Typography, Grid,
    TextField,
    Alert
} from '@mui/material';
import { ProductSchema } from '../../shared/schemas/createProductSchema';
import { CategoryAttribute } from '../../shared/types/category';

type Props = {
    attributes: CategoryAttribute[],
    control: Control<ProductSchema>,
    errors: FieldErrors<ProductSchema>,
    isLoading: boolean,
}

export default function AttributesTab({ attributes, control, errors, isLoading }: Props) {
    const { fields, replace } = useFieldArray({ control, name: 'attributes' });

    useEffect(() => {
        if (attributes.length && fields.length === 0) {
            replace(
                attributes.map((attr) => ({
                    attributeId: attr.id,
                    value: '',
                }))
            );
        }
    }, [attributes, fields.length, replace]);

    if (isLoading) {
        return (
            <Typography>Loading attributes...</Typography>
        )
    }

    if (!attributes || attributes.length === 0) {
        return (
            <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom>
                    Product Attributes
                </Typography>
                <Alert severity="info" sx={{ mt: 2 }}>
                    No attributes are defined for this category. You can add attributes in the category management section.
                </Alert>
            </Box>
        );
    }

    return (
        <>
            <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom>
                    Product Attributes
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    Fill in information for category-specific attributes.
                </Typography>
            </Box>

            <Grid container spacing={2}>
                {fields.map((field, index) => (
                    <Grid size={{ xs: 12, md: 6 }} key={field.id}>
                        <Controller
                            name={`attributes.${index}.value`}
                            control={control}
                            defaultValue={field.value}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={attributes[index].displayName}
                                    fullWidth
                                    variant="outlined"
                                    multiline
                                    error={!!errors.attributes?.[index]?.value}
                                    helperText={errors.attributes?.[index]?.value?.message}
                                />
                            )}
                        />
                        <Controller
                            name={`attributes.${index}.attributeId`}
                            control={control}
                            defaultValue={field.attributeId}
                            render={({ field }) => <input type="hidden" {...field} />}
                        />
                    </Grid>
                ))}
            </Grid>
        </>
    );
}