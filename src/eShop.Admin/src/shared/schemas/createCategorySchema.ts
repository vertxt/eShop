import { z } from "zod";

export const createCategorySchema = z.object({
    name: z.string().min(1, 'Category name is required'),
    description: z.string({ required_error: 'Category description is required' }).min(10, 'Category description must have at least 10 characters.'),
    attributes: z.array(z.object({
        name: z.string({ required_error: 'Attribute name is required' }),
        displayName: z.string({ required_error: 'Attribute display name is required' }),
    })).optional(),
})

export type CreateCategorySchema = z.infer<typeof createCategorySchema>;