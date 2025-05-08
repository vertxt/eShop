import { createApi } from "@reduxjs/toolkit/query/react";
import { Product, ProductDetail } from "../../shared/types/product";
import { PaginationMetadata } from "../../shared/types/pagination";
import { ProductListParams } from "../../shared/types/productListParams";
import { cleanParams } from "../../shared/utils/queryUtils";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";
import { Review } from "../../shared/types/review";
import { ReviewListParams } from "../../shared/types/reviewListParams";

export const productsApi = createApi({
    reducerPath: 'productsApi',
    baseQuery: customQueryWithErrorHandling,
    tagTypes: ['Products', 'Product'],
    endpoints: builder => ({
        fetchProducts: builder.query<{ items: Product[], metadata: PaginationMetadata }, ProductListParams>({
            query: (params) => {
                return {
                    url: '/products',
                    params: cleanParams(params),
                };
            },
            providesTags: ['Products'],
        }),

        fetchProduct: builder.query<Product, number>({
            query: (id) => `/products/${id}`,
            providesTags: ['Product'],
        }),

        fetchProductDetails: builder.query<ProductDetail, number>({
            query: (params) => `/products/details/${params}`,
            providesTags: ['Product'],
        }),

        createProduct: builder.mutation<Product, FormData>({
            query: (data) => {
                return {
                    url: '/products',
                    method: 'POST',
                    body: data,
                };
            },
            invalidatesTags: ['Products', 'Product'],
        }),

        updateProduct: builder.mutation<void, { id: number, data: FormData }>({
            query: ({ id, data }) => {
                data.append('id', id.toString());

                return {
                    url: `/products/${id}`,
                    method: 'PUT',
                    body: data,
                };
            },
            invalidatesTags: ['Products', 'Product'],
        }),

        deleteProduct: builder.mutation<void, number>({
            query: (id) => {
                return {
                    url: `/products/${id}`,
                    method: 'DELETE',
                };
            },
            invalidatesTags: ['Products', 'Product'],
        }),

        fetchProductReviews: builder.query<{ items: Review[], metadata: PaginationMetadata }, { productId: number, params: ReviewListParams }>({
            query: ({ productId, params }) => {
                return {
                    url: `/products/${productId}/reviews`,
                    params: cleanParams(params),
                };
            },
        })
    })
})

export const {
    useFetchProductsQuery,
    useFetchProductDetailsQuery,
    useFetchProductQuery,
    useCreateProductMutation,
    useUpdateProductMutation,
    useDeleteProductMutation,
    useFetchProductReviewsQuery,
} = productsApi;