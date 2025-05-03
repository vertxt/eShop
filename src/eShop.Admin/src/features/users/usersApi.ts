import { createApi } from "@reduxjs/toolkit/query/react";
import { customQueryWithErrorHandling } from "../../app/api/baseApi";
import { UserDto } from "../../shared/types/user";

export const usersApi = createApi({
    reducerPath: 'usersApi',
    baseQuery: customQueryWithErrorHandling,
    endpoints: builder => ({
        fetchUsers: builder.query<UserDto[], void>({
            query: () => 'users'
        }),
    }),
});

export const { useFetchUsersQuery } = usersApi;