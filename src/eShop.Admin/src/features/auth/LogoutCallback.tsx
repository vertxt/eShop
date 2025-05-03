import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";

export default function LogoutCallback() {
    const auth = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!auth.isLoading && !auth.isAuthenticated) {
            navigate('/');
        }
    }, [auth.isLoading, auth.isAuthenticated, navigate]);

    return <div>Completing sign-out, please wait...</div>;
}