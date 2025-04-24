export interface ProductListParams {
  searchTerm?: string;
  categoryIds?: number[];
  minPrice?: number | null;
  maxPrice?: number | null;
  inStock?: boolean | null;
  sortBy?: string;
  pageNumber?: number;
  pageSize?: number;
}
