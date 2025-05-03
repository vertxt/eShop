import { useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";

export default function LoginCallback() {
    const auth = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (!auth.isLoading && auth.isAuthenticated) {
            navigate('/dashboard');
        }
        
        if (!auth.isLoading && auth.error) {
            console.error("Authentication error:", auth.error);
            navigate('/');
        }
    }, [auth.isLoading, auth.isAuthenticated, auth.error, navigate]);

    return <div>Processing authentication, please wait...</div>;
}