import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ProductListParams, StockRangeFilter } from "../../shared/types/productListParams";

const initialState: ProductListParams = {
  searchTerm: "",
  sortBy: "name",
  categoryIds: [],
  maxPrice: null,
  minPrice: null,
  isActive: null,
  hasVariants: null,
  stockRange: 'all',
  createdBefore: null,
  createdAfter: null,
  pageNumber: 1,
  pageSize: 12,
};

export const productsSlice = createSlice({
  name: "products",
  initialState,
  reducers: {
    setSearchTerm(state, action: PayloadAction<string>) {
      state.searchTerm = action.payload;
      state.pageNumber = 1;
    },

    setSortBy(state, action: PayloadAction<string>) {
      state.sortBy = action.payload;
    },

    setPageNumber(state, action: PayloadAction<number>) {
      state.pageNumber = action.payload;
    },

    setPageSize(state, action: PayloadAction<number>) {
      state.pageSize = action.payload;
      state.pageNumber = 1;
    },

    setCategoryIds(state, action: PayloadAction<number[]>) {
      state.categoryIds = action.payload;
      state.pageNumber = 1;
    },

    setPriceRange(state, action: PayloadAction<{ min: number | null, max: number | null }>) {
      state.minPrice = action.payload.min;
      state.maxPrice = action.payload.max;
      state.pageNumber = 1;
    },

    setHasVariants(state, action: PayloadAction<boolean | null>) {
      state.hasVariants = action.payload;
      state.pageNumber = 1;
    },

    setIsActive(state, action: PayloadAction<boolean | null>) {
      state.isActive = action.payload;
      state.pageNumber = 1;
    },

    setStockRange(state, action: PayloadAction<StockRangeFilter>) {
      state.stockRange = action.payload;
      state.pageNumber = 1;
    },

    setCreatedDateRange(state, action: PayloadAction<{ after: string | null, before: string | null }>) {
      state.createdAfter = action.payload.after;
      state.createdBefore = action.payload.before;
      state.pageNumber = 1;
    },

    resetFilters(state) {
      return {
        ...initialState,
        searchTerm: state.searchTerm,
        sortBy: state.sortBy,
        pageSize: state.pageSize,
      }
    }
  },
});

export const {
  setSortBy,
  setSearchTerm,
  setPageNumber,
  setPageSize,
  setCategoryIds,
  setPriceRange,
  setHasVariants,
  setIsActive,
  setStockRange,
  setCreatedDateRange,
  resetFilters
} = productsSlice.actions;

export const productsReducer = productsSlice.reducer;
