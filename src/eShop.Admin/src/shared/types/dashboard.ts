export type DashboardSummary = {
    totalProducts: number;
    totalCategories: number;
    totalUsers: number;
    averageRating: number;
    totalInventoryValue: number;
    averageProductPrice: number;
    lowStockProductsCount: number;
};

export type RatingDistribution = {
    rating: number;
    count: number;
}

export type CategoryProductCount = {
    categoryName: string;
    productCount: number;
    percentageOfTotal: number;
}