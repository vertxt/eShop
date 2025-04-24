import { createApi } from "@reduxjs/toolkit/query/react";
import { Category } from "../../shared/types/category";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";

export const categoriesApi = createApi({
    reducerPath: 'categoriesApi',
    baseQuery: customQueryWithErrorHandling,
    endpoints: builder => ({
        fetchCategories: builder.query<Category[], void>({
            query: () => {
                return {
                    url: '/categories',
                };
            }
        })
    })
})

export const { useFetchCategoriesQuery } = categoriesApi;