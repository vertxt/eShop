import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ProductListParams } from "../../shared/types/productListParams";

const initialState: ProductListParams = {
  searchTerm: "",
  categoryIds: [],
  maxPrice: null,
  minPrice: null,
  inStock: null,
  sortBy: "name",
  pageNumber: 1,
  pageSize: 12,
};

export const productsSlice = createSlice({
  name: "products",
  initialState,
  reducers: {
    setSortBy(state, action: PayloadAction<string>) {
      state.sortBy = action.payload;
    },
    
    setSearchTerm(state, action: PayloadAction<string>) {
      state.searchTerm = action.payload;
      state.pageNumber = 1;
    },
    
    setPageNumber(state, action: PayloadAction<number>) {
      state.pageNumber = action.payload;
    },
    
    setPageSize(state, action: PayloadAction<number>) {
      state.pageSize = action.payload;
      state.pageNumber = 1;
    }
  },
});

export const { setSortBy, setSearchTerm, setPageNumber, setPageSize } = productsSlice.actions;
export const productsReducer = productsSlice.reducer;
