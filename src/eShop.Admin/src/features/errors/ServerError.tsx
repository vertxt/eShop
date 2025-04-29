import { Divider, Paper, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";

export default function ServerError() {
    const { state } = useLocation();

    return (
        <Paper sx={{ px: '4rem', py: '2rem' }}>
            {state.error ? (
                <>
                    <Typography variant="h4" gutterBottom color="error" sx={{ fontWeight: '700' }}>{state.error.status}: {state.error.title}</Typography>
                    <Divider />
                    <Typography sx={{ marginTop: '2rem' }}>
                        {state.error.detail}
                    </Typography>
                </>
            ) : (
                <Typography variant="h4" color="error" sx={{ fontWeight: '600' }}>Oops, something went wrong. This is unexpected. Please refresh</Typography>
            )}
        </Paper>
    )
}