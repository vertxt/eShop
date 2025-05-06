import { useEffect, useState } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";
import { Box, CircularProgress, Typography, Paper, Container, Button } from "@mui/material";
import { CheckCircle } from "@mui/icons-material";

type LogoutState = "processing" | "success";

export default function LogoutCallback() {
    const auth = useAuth();
    const navigate = useNavigate();
    const [status, setStatus] = useState<LogoutState>("processing");

    useEffect(() => {
        if (!auth.isLoading && !auth.isAuthenticated) {
            setStatus("success");
            // Add small delay
            const timer = setTimeout(() => navigate('/'), 2000);
            return () => clearTimeout(timer);
        }
    }, [auth.isLoading, auth.isAuthenticated, navigate]);

    const handleRedirectNow = () => {
        navigate('/');
    };

    return (
        <Container maxWidth="sm">
            <Box sx={{ mt: 10, display: "flex", justifyContent: "center" }}>
                <Paper
                    elevation={3}
                    sx={{
                        p: 5,
                        width: "100%",
                        textAlign: "center",
                        borderRadius: 2
                    }}
                >
                    {status === "processing" && (
                        <>
                            <CircularProgress size={60} color="primary" sx={{ mb: 3 }} />
                            <Typography variant="h5" gutterBottom>
                                Signing you out...
                            </Typography>
                            <Typography variant="body1" color="text.secondary">
                                Please wait while we complete the sign out process
                            </Typography>
                        </>
                    )}

                    {status === "success" && (
                        <>
                            <CheckCircle sx={{ fontSize: 60, color: "#4caf50", marginBottom: 2 }} />
                            <Typography variant="h5" gutterBottom>
                                You've been signed out
                            </Typography>
                            <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
                                Thank you for using our admin portal
                            </Typography>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={handleRedirectNow}
                            >
                                Return to Login
                            </Button>
                        </>
                    )}
                </Paper>
            </Box>
        </Container>
    );
}