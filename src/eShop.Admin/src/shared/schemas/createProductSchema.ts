import { z } from "zod";

export const productSchema = z.object({
    name: z.string(),
    basePrice: z.coerce.number(),
    description: z.string(),
    shortDescription: z.string(),
    isActive: z.boolean(),
    categoryId: z.coerce.number(),
    hasVariants: z.boolean(),
    quantityInStock: z.coerce.number(),
})

export type ProductSchema = z.infer<typeof productSchema>;