import { createApi } from "@reduxjs/toolkit/query/react";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";

export const errorsApi = createApi({
    reducerPath: 'errorsApi',
    baseQuery: customQueryWithErrorHandling,
    endpoints: builder => ({
        getNotFound: builder.query<void, void>({
            query: () => 'testerrors/not-found',
        }),
        getBadRequest: builder.query<void, void>({
            query: () => 'testerrors/bad-request',
        }),
        getUnauthorized: builder.query<void, void>({
            query: () => 'testerrors/unauthorized',
        }),
        getServerError: builder.query<void, void>({
            query: () => 'testerrors/server-error',
        }),
        getValidationError: builder.query<void, void>({
            query: () => 'testerrors/validation',
        })
    })
});

export const {
    useLazyGetNotFoundQuery,
    useLazyGetBadRequestQuery,
    useLazyGetUnauthorizedQuery,
    useLazyGetServerErrorQuery,
    useLazyGetValidationErrorQuery,
} = errorsApi;