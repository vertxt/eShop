import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ReviewListParams } from "../../shared/types/reviewListParams";

const initialState: ReviewListParams = {
    pageNumber: 1,
    pageSize: 2,
};

export const reviewsSlice = createSlice({
    name: "reviews",
    initialState,
    reducers: {
        setPageSize(state, action: PayloadAction<number>) {
            state.pageSize = action.payload;
        },

        setPageNumber(state, action: PayloadAction<number>) {
            state.pageNumber = action.payload;
        }
    }
})

export const {
    setPageNumber,
    setPageSize,
} = reviewsSlice.actions;

export const reviewsReducer = reviewsSlice.reducer;