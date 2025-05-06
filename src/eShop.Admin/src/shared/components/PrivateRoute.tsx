import { Typography } from "@mui/material";
import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { Navigate, Outlet } from "react-router-dom";

export default function PrivateRoute() {
    const auth = useAuth();

    useEffect(() => {
        if (!auth.isLoading && !auth.isAuthenticated) {
            auth.signinRedirect();
        }
    }, [auth.isLoading, auth.isAuthenticated, auth.signinRedirect]);

    if (auth.isLoading) return <Typography>Loading...</Typography>;
    if (!auth.isAuthenticated) return <Typography>Redirecting to login...</Typography>;

    const role = auth.user?.profile.role;
    if (role && role === "Admin") {
        return <Outlet />
    }
    else {
        return <Navigate to="/access-denied" />
    }
}