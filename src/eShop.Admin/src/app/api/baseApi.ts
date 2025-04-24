import { BaseQueryApi, FetchArgs, fetchBaseQuery, FetchBaseQueryError } from "@reduxjs/toolkit/query";
import { toast } from "react-toastify";

type ErrorResponse = {
    type: string,
    title: string,
    status: number | string,
    detail?: string | null,
    errors?: object | null,
    traceId?: string | null
}

const isFetchBaseQueryError = (error: unknown) => (
    error && typeof error == 'object' && 'status' in error
);

const handleErrorResponse = (error: unknown) => {
    if (!isFetchBaseQueryError(error)) {
        toast.error("A network or serialization error occurred.");
    }

    const { status, data } = error as FetchBaseQueryError;
    const err = data as ErrorResponse;

    const navigateTo = (path: string, state?: any) => {
        import("../routing/Routes").then(({ router }) => {
            router.navigate(path, state);
        });
    };

    switch (status) {
        case 400:
            if ('errors' in err && err.errors) {
                return { fieldErrors: err.errors || {}, message: err.title };
            } else {
                toast.error(err.detail ?? err.title);
            }
            break;
        case 401:
        case 403:
            toast.error(err.detail ?? err.title);
            break;
        case 404:
            navigateTo('/errors/notfound');
            break;
        case 500:
            navigateTo('/errors/server', { state: { error: err } });
            break
    }
};

const baseQuery = fetchBaseQuery({
    baseUrl: 'https://localhost:5000/api',
    credentials: 'include',
});

export const customQueryWithErrorHandling = async (args: string | FetchArgs, api: BaseQueryApi, extraOptions: object) => {
    const result = await baseQuery(args, api, extraOptions);

    if (result.error) {
        return {
            ...result,
            error: handleErrorResponse(result.error),
        };
    }

    return result;
};