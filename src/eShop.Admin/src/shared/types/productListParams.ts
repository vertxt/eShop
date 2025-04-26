export type StockRangeFilter = 'all' | 'outOfStock' | 'low' | 'medium' | 'high';

export interface ProductListParams {
  searchTerm?: string;
  sortBy?: string;
  categoryIds?: number[];
  minPrice?: number | null;
  maxPrice?: number | null;
  isActive?: boolean | null;
  hasVariants?: boolean | null;
  stockRange?: StockRangeFilter;
  createdBefore?: string | null;
  createdAfter?: string | null;
  pageNumber?: number;
  pageSize?: number;
}
