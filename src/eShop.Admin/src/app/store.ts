import { configureStore } from "@reduxjs/toolkit";
import { productsReducer } from "../features/products/productsSlice";
import { productsApi } from "../features/products/productsApi";
import { useDispatch, useSelector } from "react-redux";

export const store = configureStore({
    reducer: {
        products: productsReducer,
        [productsApi.reducerPath]: productsApi.reducer,
    },
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware().concat(
            productsApi.middleware,
        ),
});

export type AppStore = typeof store;
export type RootState = ReturnType<AppStore["getState"]>;
export type AppDispatch = AppStore["dispatch"];

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();