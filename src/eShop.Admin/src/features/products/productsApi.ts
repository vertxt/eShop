import { createApi } from "@reduxjs/toolkit/query/react";
import { Product, ProductDetail } from "../../shared/types/product";
import { PaginationMetadata } from "../../shared/types/pagination";
import { ProductListParams } from "../../shared/types/productListParams";
import { cleanParams } from "../../shared/utils/queryUtils";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";

export const productsApi = createApi({
    reducerPath: 'productsApi',
    baseQuery: customQueryWithErrorHandling,
    endpoints: builder => ({
        fetchProducts: builder.query<{ items: Product[], metadata: PaginationMetadata }, ProductListParams>({
            query: (params) => {
                return {
                    url: '/products',
                    params: cleanParams(params),
                };
            }
        }),

        fetchProductDetails: builder.query<ProductDetail, number>({
            query: (params) => `/products/details/${params}`
        }),

        createProduct: builder.mutation<Product, FormData>({
            query: (data) => {
                return {
                    url: '/products',
                    method: 'POST',
                    body: data,
                };
            }
        }),

        updateProduct: builder.mutation<void, { id: number, data: FormData }>({
            query: ({ id, data }) => {
                data.append('id', id.toString());

                return {
                    url: `/products/${id}`,
                    method: 'PUT',
                    body: data,
                };
            }
        })
    })
})

export const {
    useFetchProductsQuery,
    useFetchProductDetailsQuery,
    useCreateProductMutation,
    useUpdateProductMutation,
} = productsApi;