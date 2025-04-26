import { Box, LinearProgress } from "@mui/material";
import { useAppSelector } from "../../app/store/store";

export default function GlobalLoadingIndicator() {
    const loading = useAppSelector(state => state.ui.loading);

    return (
        loading && <Box width="100%">
            <LinearProgress />
        </Box>
    )
}