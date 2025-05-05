import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { Outlet } from "react-router-dom";

export default function PrivateRoute() {
    const auth = useAuth();

    useEffect(() => {
        if (!auth.isLoading && !auth.isAuthenticated) {
            auth.signinRedirect();
        }
    }, [auth.isLoading, auth.isAuthenticated, auth.signinRedirect]);

    if (auth.isLoading) return <div>Loading...</div>;

    if (!auth.isAuthenticated) return <div>Redirecting to login...</div>;

    if (auth.isAuthenticated)
    {
        const roles = auth.user?.profile;
        console.log(roles);
    }

    return <Outlet />
}