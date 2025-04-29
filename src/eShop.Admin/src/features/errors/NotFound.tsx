import { SearchOff } from "@mui/icons-material";
import { Box, Button, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function NotFound() {
    const navigate = useNavigate();

    return (
        <Box
            sx={{
                position: 'fixed',
                top: 0,
                left: 0,
                height: '100%',
                width: '100%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center'
            }}
        >
            <SearchOff sx={{ fontSize: '10rem' }} />
            <Typography variant="h2" gutterBottom sx={{ fontWeight: '900' }}>Oops! Page not found.</Typography>
            <Button onClick={() => navigate(-1)} size="large" variant="outlined">Go back</Button>
        </Box>
    );
}