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
  },
});

export const { setSortBy } = productsSlice.actions;
export const productsReducer = productsSlice.reducer;
