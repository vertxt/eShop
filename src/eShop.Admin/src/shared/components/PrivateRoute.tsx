import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { Outlet } from "react-router-dom";

export default function PrivateRoute() {
    const auth = useAuth();

    if (auth.isLoading) return <div>Loading...</div>;

    useEffect(() => {
        if (!auth.isAuthenticated) {
            auth.signinRedirect();
        }
    }, [auth.isLoading, auth.isAuthenticated, auth.signinRedirect]);

    return auth.isAuthenticated ? <Outlet /> : <div>Redirecting to login...</div>
}