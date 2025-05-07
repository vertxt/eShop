import { Warning } from '@mui/icons-material';
import { Box, Button, Container, Typography, Paper } from '@mui/material';
import { useAuth } from 'react-oidc-context';

export default function AccessDenied() {
    const { signoutRedirect } = useAuth();

    const backToHome = () => {
        signoutRedirect();
    }

    return (
        <Container maxWidth="md" sx={{ mt: 8 }}>
            <Paper
                elevation={3}
                sx={{
                    p: 5,
                    borderRadius: 2,
                    textAlign: 'center',
                    border: '1px solid #f44336',
                    bgcolor: 'rgba(244, 67, 54, 0.05)'
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        mb: 3
                    }}
                >
                    <Warning sx={{ fontSize: 64, color: "#f44336" }} />
                </Box>

                <Typography variant="h4" component="h1" gutterBottom sx={{ fontWeight: 600 }}>
                    Access Denied
                </Typography>

                <Typography variant="body1" sx={{ mb: 4, maxWidth: '80%', mx: 'auto' }}>
                    You don't have permission to access this admin area.
                    Only users with the role "Admin" can access this site.
                </Typography>

                <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center', gap: 2 }}>
                    <Button
                        variant="outlined"
                        color="primary"
                        onClick={backToHome}
                    >
                        Sign out
                    </Button>
                    <Button
                        variant="contained"
                        color="primary"
                    >
                        Contact admin
                    </Button>
                </Box>
            </Paper>
        </Container>
    );
}