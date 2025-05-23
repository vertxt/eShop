import { createApi } from "@reduxjs/toolkit/query/react";
import { Category, CategoryAttribute, CategoryDetail } from "../../shared/types/category";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";
import { CreateCategorySchema } from "../../shared/schemas/createCategorySchema";

export const categoriesApi = createApi({
    reducerPath: 'categoriesApi',
    baseQuery: customQueryWithErrorHandling,
    tagTypes: ['Category'],
    endpoints: builder => ({
        fetchCategories: builder.query<Category[], void>({
            query: () => '/categories',
            providesTags: ['Category'],
        }),

        fetchCategory: builder.query<Category, number>({
            query: (id) => `/categories/${id}`
        }),

        fetchCategoryDetails: builder.query<CategoryDetail, number>({
            query: (id) => `/categories/details/${id}`
        }),

        fetchCategoryAttributes: builder.query<CategoryAttribute[], number>({
            query: (id) => `/categories/attributes/${id}`
        }),

        createCategory: builder.mutation<Category, CreateCategorySchema>({
            query: (params) => ({
                url: '/categories',
                method: 'POST',
                body: params
            }),
            invalidatesTags: ['Category'],
        }),

        updateCategory: builder.mutation<void, { id: number, data: CreateCategorySchema }>({
            query: ({ id, data }) => ({
                url: `categories/${id}`,
                method: 'PUT',
                body: data
            }),
            invalidatesTags: ['Category'],
        }),

        deleteCategory: builder.mutation<void, number>({
            query: (id) => ({
                url: `categories/${id}`,
                method: 'DELETE',
            }),
            invalidatesTags: ['Category'],
        })
    })
})

export const {
    useFetchCategoriesQuery,
    useCreateCategoryMutation,
    useUpdateCategoryMutation,
    useDeleteCategoryMutation,
    useFetchCategoryAttributesQuery,
    useFetchCategoryQuery,
    useFetchCategoryDetailsQuery,
} = categoriesApi;