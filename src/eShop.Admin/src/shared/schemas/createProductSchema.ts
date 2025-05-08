import { z } from "zod";

export const productSchema = z.object({
    name: z.string().min(1, "Product name is required"),
    basePrice: z.coerce.number().min(0, "Price must be 0 or higher"),
    description: z.string().optional(),
    shortDescription: z.string().optional(),
    categoryId: z.coerce.number().positive("Category must be selected"),
    isActive: z.boolean(),
    isFeatured: z.boolean(),
    hasVariants: z.boolean(),
    quantityInStock: z.coerce.number().optional().nullable(),
    attributes: z.array(
        z.object({
            attributeId: z.number(),
            value: z.string().optional()
        })
    ),
    variants: z.array(
        z.object({
            id: z.coerce.number().optional(),
            name: z.string().min(1, "Variant name is required"),
            sku: z.string().optional().nullable(),
            price: z.coerce.number().min(0, "Price must be 0 or higher"),
            quantityInStock: z.coerce.number().optional().nullable(),
            isActive: z.boolean()
        })
    ).optional()
});

export type ProductSchema = z.infer<typeof productSchema>;