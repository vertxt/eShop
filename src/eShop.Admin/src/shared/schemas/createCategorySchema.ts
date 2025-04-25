import { z } from "zod";

export const createCategorySchema = z.object({
    name: z.string().min(1, 'Category name is required'),
    description: z.string(),
})

export type CreateCategorySchema = z.infer<typeof createCategorySchema>;