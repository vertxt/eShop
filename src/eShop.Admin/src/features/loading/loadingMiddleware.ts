import { isFulfilled, isPending, isRejectedWithValue, Middleware } from "@reduxjs/toolkit";
import { startLoading, stopLoading } from "../../app/store/uiSlice";

export const loadingMiddleware: Middleware = (store) => (next) => (action) => {
    if (isPending(action)) {
        store.dispatch(startLoading());
    }

    if (isFulfilled(action) || isRejectedWithValue(action)) {
        store.dispatch(stopLoading());
    }

    return next(action);
};