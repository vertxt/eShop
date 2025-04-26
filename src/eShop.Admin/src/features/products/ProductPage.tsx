import { Grid } from "@mui/material";
import ProductFilters from "./ProductFilters";
import ProductListView from "./ProductListView";

export default function ProductPage() {
    return (
        <Grid container>
            <Grid size={{ xs: 12, md: 2 }}>
                <ProductFilters />
            </Grid>
            <Grid size={{ xs: 12, md: 10 }}>
                <ProductListView />
            </Grid>
        </Grid>
    )
}