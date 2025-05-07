import { BarChart, Insights, PeopleAlt, Shield, ShoppingCart } from "@mui/icons-material";
import { Box, Button, Card, CardContent, Container, Divider, Grid, Paper, Typography } from "@mui/material";
import { ReactNode, useEffect } from "react";
import { useAuth } from "react-oidc-context";
import { useNavigate } from "react-router-dom";

type FeatureDisplayItem = {
    icon: ReactNode,
    title: string,
    description: string,
}

export default function LandingPage() {
    const auth = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        if (auth.isAuthenticated) {
            navigate('/dashboard');
        }
    }, [auth.isAuthenticated, navigate]);

    const handleLogin = () => {
        auth.signinRedirect();
    }

    const features: FeatureDisplayItem[] = [
        {
            icon: <ShoppingCart fontSize="large" sx={{ color: "#1976d2" }} />,
            title: "Product Management",
            description: "Easily manage your entire product catalog with powerful tools."
        },
        {
            icon: <PeopleAlt fontSize="large" sx={{ color: "#1976d2" }} />,
            title: "User Controls",
            description: "Administrative tools to manage users, roles, and permissions."
        },
        {
            icon: <BarChart fontSize="large" sx={{ color: "#1976d2" }} />,
            title: "Analytics Dashboard",
            description: "Get insights with comprehensive analytics and reporting."
        },
        {
            icon: <Shield fontSize="large" sx={{ color: "#1976d2" }} />,
            title: "Secure Access",
            description: "Enterprise-grade security with role-based access control."
        }
    ];

    return (
        <Box sx={{ minHeight: "100vh", py: 8 }}>
            <Container maxWidth="lg">
                {/* Hero Section */}
                <Paper
                    elevation={0}
                    sx={{
                        mb: 6,
                        p: { xs: 4, md: 6 },
                        borderRadius: 3,
                        background: "linear-gradient(120deg, #1976d2, #42a5f5)",
                        color: "white"
                    }}
                >
                    <Grid container spacing={4} alignItems="center">
                        <Grid size={{ xs: 12, md: 7 }}>
                            <Typography variant="h3" component="h1" gutterBottom sx={{ fontWeight: 700 }}>
                                Admin Portal
                            </Typography>
                            <Typography variant="h6" sx={{ mb: 4, opacity: 0.9 }}>
                                Powerful tools to manage your e-commerce platform with ease
                            </Typography>
                            <Button
                                variant="contained"
                                size="large"
                                onClick={handleLogin}
                                sx={{
                                    bgcolor: "white",
                                    color: "#1976d2",
                                    "&:hover": {
                                        bgcolor: "rgba(255,255,255,0.9)"
                                    },
                                    px: 4,
                                    py: 1.5
                                }}
                            >
                                Sign In To Continue
                            </Button>
                        </Grid>
                        <Grid size={{ xs: 12, md: 5 }} sx={{ display: { xs: "none", md: "block" } }}>
                            {/* You could add an illustration here */}
                            <Box sx={{ textAlign: "center" }}>
                                <Insights sx={{ fontSize: 220, opacity: 0.5 }} />
                            </Box>
                        </Grid>
                    </Grid>
                </Paper>

                {/* Features */}
                <Typography variant="h4" component="h2" sx={{ mb: 4, textAlign: "center", fontWeight: 600 }}>
                    Admin Portal Features
                </Typography>

                <Grid container spacing={3} sx={{ mb: 6 }}>
                    {features.map((feature, index) => (
                        <Grid size={{ xs: 12, sm: 6, md: 3 }} key={index}>
                            <Card
                                sx={{
                                    height: "100%",
                                    display: "flex",
                                    flexDirection: "column",
                                    transition: "transform 0.2s",
                                    "&:hover": {
                                        transform: "translateY(-5px)",
                                        boxShadow: 3
                                    }
                                }}
                            >
                                <CardContent sx={{ flexGrow: 1, textAlign: "center", py: 4 }}>
                                    <Box sx={{ mb: 2 }}>
                                        {feature.icon}
                                    </Box>
                                    <Typography variant="h6" component="h3" gutterBottom>
                                        {feature.title}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        {feature.description}
                                    </Typography>
                                </CardContent>
                            </Card>
                        </Grid>
                    ))}
                </Grid>

                <Divider sx={{ mb: 6 }} />

                {/* Footer */}
                <Box sx={{ textAlign: "center" }}>
                    <Typography variant="body2" color="text.secondary">
                        © {new Date().getFullYear()} Admin Portal. All rights reserved.
                    </Typography>
                </Box>
            </Container>
        </Box>
    );
}