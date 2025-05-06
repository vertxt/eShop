import { useEffect, useState } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";
import { Box, CircularProgress, Typography, Paper, Container } from "@mui/material";
import { CheckCircle, Dangerous } from "@mui/icons-material";

type LoginState = "processing" | "success" | "error";

export default function LoginCallback() {
    const auth = useAuth();
    const navigate = useNavigate();
    const [status, setStatus] = useState<LoginState>("processing");

    useEffect(() => {
        if (!auth.isLoading) {
            if (auth.isAuthenticated) {
                setStatus("success");
                // Add small delay
                const timer = setTimeout(() => navigate('/dashboard'), 1500);
                return () => clearTimeout(timer);
            }

            if (auth.error) {
                console.error("Authentication error:", auth.error);
                setStatus("error");
                // Add small delay
                const timer = setTimeout(() => navigate('/'), 3000);
                return () => clearTimeout(timer);
            }
        }
    }, [auth.isLoading, auth.isAuthenticated, auth.error, navigate]);

    return (
        <Container maxWidth="sm">
            <Box sx={{ mt: 10, display: "flex", justifyContent: "center" }}>
                <Paper elevation={3} sx={{ p: 5, width: "100%", textAlign: "center" }}>
                    {status === "processing" && (
                        <>
                            <CircularProgress size={60} color="primary" sx={{ mb: 3 }} />
                            <Typography variant="h5" gutterBottom>
                                Signing you in...
                            </Typography>
                            <Typography variant="body1" color="text.secondary">
                                Please wait while we verify your credentials
                            </Typography>
                        </>
                    )}

                    {status === "success" && (
                        <>
                            <CheckCircle sx={{ fontSize: 60, color: "#4caf50", marginBottom: 2 }} />
                            <Typography variant="h5" gutterBottom>
                                Sign in successful!
                            </Typography>
                            <Typography variant="body1" color="text.secondary">
                                Redirecting you to the dashboard...
                            </Typography>
                        </>
                    )}

                    {status === "error" && (
                        <>
                            <Dangerous sx={{ fontSize: 60, color: "#f44336", marginBottom: 2 }} />
                            <Typography variant="h5" gutterBottom>
                                Authentication Error
                            </Typography>
                            <Typography variant="body1" color="text.secondary">
                                There was a problem signing you in. Please try again.
                            </Typography>
                            <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                                Redirecting to home page...
                            </Typography>
                        </>
                    )}
                </Paper>
            </Box>
        </Container>
    );
}