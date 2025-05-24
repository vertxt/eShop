import { createApi } from "@reduxjs/toolkit/query/react";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";
import { CategoryProductCount, DashboardSummary, RatingDistribution } from "../../shared/types/dashboard";
import { Review } from "../../shared/types/review";
import { Product } from "../../shared/types/product";

export const dashboardApi = createApi({
    reducerPath: 'dashboardApi',
    baseQuery: customQueryWithErrorHandling,
    endpoints: builder => ({
        fetchDashboardSummary: builder.query<DashboardSummary, void>({
            query: () => '/dashboard/summary',
        }),
        fetchProductsByCategory: builder.query<CategoryProductCount[], number | void>({
            query: (limit) => ({
                url: '/dashboard/products-by-category',
                params: limit != null ? { limit } : {},
            }),
        }),
        fetchRatingDistribution: builder.query<RatingDistribution[], void>({
            query: () => '/dashboard/rating-distribution',
        }),
        fetchRecentReviews: builder.query<Review[], number | void>({
            query: (count) => ({
                url: '/dashboard/recent-reviews',
                params: count != null ? { count } : {},
            }),
        }),
        fetchLowStockProducts: builder.query<Product[], { threshold: number | void, count: number | void } | void>({
            query: (params) => ({
                url: '/dashboard/low-stock-products',
                params: params ? params : {}
            }),
        }),
        fetchLatestFeaturedProducts: builder.query<Product[], number | void>({
            query: (count) => ({
                url: '/dashboard/latest-featured-products',
                params: count != null ? { count } : {},
            }),
        }),
    })
});

export const {
    useFetchDashboardSummaryQuery,
    useFetchProductsByCategoryQuery,
    useFetchRecentReviewsQuery,
    useFetchLatestFeaturedProductsQuery,
    useFetchLowStockProductsQuery,
    useFetchRatingDistributionQuery,
} = dashboardApi;