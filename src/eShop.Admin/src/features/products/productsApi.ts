import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { Product } from "../../shared/types/product";
import { PaginationMetadata } from "../../shared/types/pagination";
import { ProductListParams } from "../../shared/types/productListParams";
import { cleanParams } from "../../shared/utils/queryUtils";

export const productsApi = createApi({
    reducerPath: 'productsApi',
    baseQuery: fetchBaseQuery({ baseUrl: 'https://localhost:5000/api' }),
    endpoints: builder => ({
        fetchProducts: builder.query<{ items: Product[], metadata: PaginationMetadata }, ProductListParams>({
            query: (params) => {
                return {
                    url: '/products',
                    params: cleanParams(params),
                }
            }
        })
    })
})

export const { useFetchProductsQuery } = productsApi;
