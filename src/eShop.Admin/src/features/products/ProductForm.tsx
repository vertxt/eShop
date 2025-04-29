import { SyntheticEvent, useEffect, useState } from "react";
import { useFetchAttributesByCategoryIdQuery, useFetchCategoriesQuery } from "../categories/categoriesApi";
import { useCreateProductMutation, useFetchProductDetailsQuery, useUpdateProductMutation } from "./productsApi";
import { FieldErrors, useForm } from "react-hook-form";
import { productSchema, ProductSchema } from "../../shared/schemas/createProductSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-toastify";
import { Box, Button, Card, CardContent, Tab, Tabs, Typography } from "@mui/material";
import TabPanel from "../../shared/components/TabPanel";
import { Save } from "@mui/icons-material";
import AttributesTab from "./AttributesTab";
import BasicInfoTab from "./BasicInfoTab";
import ImagesTab from "./ImagesTab";
import VariantsTab from "./VariantsTab";
import { productDetailToForm } from "../../shared/utils/productMappers";

type Props = {
    productId?: number | null,
    onSuccess?: () => void,
}

export interface ImageFile {
    id: string,
    file: File | null,
    preview: string,
    isMain: boolean,
    displayOrder: number,
    existingImageId?: number,
}

export default function ProductForm({ productId, onSuccess }: Props) {
    const [activeTab, setActiveTab] = useState<number>(0);
    const [imageFiles, setImageFiles] = useState<ImageFile[]>([]);
    const isEditMode = Boolean(productId);

    const { control, handleSubmit, watch, reset, formState: { errors } } = useForm<ProductSchema>({
        resolver: zodResolver(productSchema),
        mode: 'onBlur',
        defaultValues: {
            name: '',
            basePrice: 0,
            description: '',
            shortDescription: '',
            categoryId: 1,
            isActive: true,
            hasVariants: false,
            quantityInStock: 0,
            attributes: [],
            variants: []
        }
    });

    const hasVariants = watch('hasVariants');
    const categoryId = watch('categoryId');

    const { data: categories, isLoading: isLoadingCategories } = useFetchCategoriesQuery();
    const { data: existingProduct, isLoading: isLoadingProducts } = useFetchProductDetailsQuery(
        productId!,
        { skip: !isEditMode }
    );
    const { data: attributes, isLoading: isLoadingAttributes } = useFetchAttributesByCategoryIdQuery(categoryId!, { skip: !categoryId });

    const [createProduct, { isLoading: isAddingProduct }] = useCreateProductMutation();
    const [updateProduct, { isLoading: isUpdatingProduct }] = useUpdateProductMutation();

    const isSubmitting = isAddingProduct || isUpdatingProduct;

    const handleTabChange = (_event: SyntheticEvent, newValue: number): void => {
        setActiveTab(newValue);
    };

    useEffect(() => {
        if (categoryId && attributes && !isEditMode) {
            reset((current) => ({
                ...current,
                attributes: attributes.map(attr => ({
                    attributeId: attr.id,
                    value: '',
                }))
            }));
        }
    }, [categoryId, attributes, reset, isEditMode]);

    useEffect(() => {
        if (!existingProduct) return;
        const preloadData = productDetailToForm(existingProduct);
        console.log(JSON.stringify(preloadData));
        reset(preloadData);

        // set images here for ImagesTab
        const preloadImages: ImageFile[] = existingProduct.images.map(img => ({
            id: img.id.toString(),
            preview: img.url,
            isMain: img.isMain,
            displayOrder: img.displayOrder,
            file: null,
            existingImageId: img.id,
        }));
        setImageFiles(preloadImages);
    }, [existingProduct, reset])

    useEffect(() => {
        if (hasVariants) {
            const currentVariants = watch('variants') || [];
            if (currentVariants.length === 0) {
                reset({
                    ...watch(),
                    variants: [{
                        id: undefined,
                        name: '',
                        sku: '',
                        price: watch('basePrice') || 0,
                        quantityInStock: watch('quantityInStock') || 0,
                        isActive: true,
                    }]
                });
            }
        }
    }, [hasVariants, reset, watch]);

    const createFormData = (data: ProductSchema) => {
        const formData = new FormData();

        // Basic information
        Object.keys(data).forEach(key => {
            if (key !== 'attributes' && key !== 'variants') {
                formData.append(key, String(data[key as keyof ProductSchema]));
            }
        });

        // Attributes
        if (data.attributes) {
            data.attributes.forEach((attr, index) => {
                formData.append(`Attributes[${index}].AttributeId`, String(attr.attributeId));
                if (attr.value) {
                    formData.append(`Attributes[${index}].Value`, attr.value);
                }
            });
        }

        // Variants
        if (data.variants && data.hasVariants) {
            data.variants.forEach((variant, vIndex) => {
                if (variant.id) {
                    formData.append(`Variants[${vIndex}].Id`, String(variant.id));
                }
                formData.append(`Variants[${vIndex}].Name`, variant.name);
                formData.append(`Variants[${vIndex}].Price`, String(variant.price));
                formData.append(`Variants[${vIndex}].IsActive`, String(variant.isActive));

                if (variant.sku != null) {
                    formData.append(`Variants[${vIndex}].SKU`, variant.sku);
                }

                if (variant.quantityInStock != null) {
                    formData.append(`Variants[${vIndex}].QuantityInStock`, String(variant.quantityInStock));
                }
            });
        }

        // Images
        // Add existing image IDs and metadata for updates
        if (isEditMode) {
            const existingImages = imageFiles.filter(img => img.existingImageId);

            existingImages.forEach((img, index) => {
                formData.append(`ExistingImageIds[${index}]`, String(img.existingImageId));
            });

            existingImages.forEach((img, index) => {
                formData.append(`ExistingImageMetadata[${index}].IsMain`, String(img.isMain));
                formData.append(`ExistingImageMetadata[${index}].DisplayOrder`, String(img.displayOrder));
            });
        }

        // Add new images
        const newImages = imageFiles.filter(img => img.file);
        newImages.forEach((img) => {
            formData.append(`Images`, img.file as File);
        });

        // Add metadata for new images
        newImages.forEach((img, index) => {
            formData.append(`ImageMetadata[${index}].IsMain`, String(img.isMain));
            formData.append(`ImageMetadata[${index}].DisplayOrder`, String(img.displayOrder));
        });

        return formData;
    };

    const onSubmit = async (data: ProductSchema) => {
        if (Object.keys(errors).length > 0) {
            console.log('Form validation errors:', errors);
            toast.error('Please fix the form errors before submitting.');
            return;
        }

        try {
            const productData = createFormData(data);
            for (let [key, value] of productData.entries()) {
                console.log(key, value);
            }
            if (isEditMode && productId) {
                const payload = await updateProduct({ id: productId, data: productData }).unwrap();
                console.log("Update product", productId, payload);
            } else {
                const payload = await createProduct(productData).unwrap();
                console.log("Create product", payload);
            }

            toast.success("Product saved successfully!");
            if (onSuccess) onSuccess();
        } catch (error) {
            console.error('Submission error:', error);
            toast.error(`Failed to save product: ${error}`);
        }
    };

    const onInvalid = (errors: FieldErrors<ProductSchema>) => {
        console.log('Validation errors:', errors);
        toast.error('Please fix the form errors before submitting.');
        if (errors.variants && activeTab !== 3) {
            setActiveTab(3);
        }
    };

    if (isEditMode && isLoadingProducts) {
        return (
            <Typography>
                Initializing form data...
            </Typography>
        )
    }

    return (
        <Card>
            <CardContent>
                <Box component='form' onSubmit={handleSubmit(onSubmit, onInvalid)}>
                    <Typography variant="h5" gutterBottom>
                        {isEditMode ? "Edit Product" : "Add New Product"}
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
                        <BasicInfoTab
                            control={control}
                            errors={errors}
                            categories={categories}
                            isLoadingCategories={isLoadingCategories}
                            hasVariants={hasVariants}
                        />
                    </TabPanel>

                    <TabPanel value={activeTab} index={1}>
                        <AttributesTab
                            attributes={attributes || []}
                            control={control}
                            errors={errors}
                            isLoading={isLoadingAttributes}
                        />
                    </TabPanel>

                    <TabPanel value={activeTab} index={2}>
                        <ImagesTab
                            imageFiles={imageFiles}
                            setImageFiles={setImageFiles}
                        />
                    </TabPanel>

                    <TabPanel value={activeTab} index={3}>
                        <VariantsTab
                            control={control}
                            errors={errors}
                            hasVariants={hasVariants}
                        />
                    </TabPanel>

                    <Box sx={{ mt: 4, display: 'flex', justifyContent: 'space-between' }}>
                        <Button
                            variant="outlined"
                            onClick={() => handleTabChange(null as any, Math.max(0, activeTab - 1))}
                            disabled={activeTab === 0}
                        >
                            Previous
                        </Button>

                        <Box>
                            {activeTab < 3 && (
                                <Button
                                    variant="outlined"
                                    onClick={() => handleTabChange(null as any, Math.min(3, activeTab + 1))}
                                    sx={{ mr: 2 }}
                                >
                                    Next
                                </Button>
                            )}

                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                startIcon={<Save />}
                                loading={isSubmitting}
                            >
                                {isEditMode ? 'Update Product' : 'Create Product'}
                            </Button>
                        </Box>
                    </Box>
                </Box>
            </CardContent>
        </Card>
    )
}