import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from "react-redux";
import { uiReducer } from "./uiSlice";
import { productsReducer } from "../../features/products/productsSlice";
import { productsApi } from "../../features/products/productsApi";
import { categoriesApi } from "../../features/categories/categoriesApi";
import { errorsApi } from "../../features/errors/errorsApi";
import { loadingMiddleware } from "../../features/loading/loadingMiddleware";
import { usersApi } from "../../features/users/usersApi";

export const store = configureStore({
  reducer: {
    products: productsReducer,
    ui: uiReducer,
    [productsApi.reducerPath]: productsApi.reducer,
    [categoriesApi.reducerPath]: categoriesApi.reducer,
    [usersApi.reducerPath]: usersApi.reducer,
    [errorsApi.reducerPath]: errorsApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(
      productsApi.middleware,
      categoriesApi.middleware,
      usersApi.middleware,
      errorsApi.middleware,
      loadingMiddleware
    ),
});

export type AppStore = typeof store;
export type RootState = ReturnType<AppStore["getState"]>;
export type AppDispatch = AppStore["dispatch"];

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();