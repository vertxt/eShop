import { debounce, InputAdornment, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import SearchIcon from "@mui/icons-material/Search";

type Prop = {
    searchTerm: string,
    onSearchChange: (searchTerm: string) => void,
}

export default function Search({ searchTerm, onSearchChange }: Prop) {
    const [localSearch, setLocalSearch] = useState<string>(searchTerm);
    const debouncedSearch = debounce((event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        onSearchChange(event.target.value);
    }, 500);

    useEffect(() => {
        if (localSearch !== searchTerm) {
            setLocalSearch(searchTerm);
        }
    }, [searchTerm]);

    return (
        <TextField
            fullWidth
            label="Search products"
            variant="outlined"
            value={localSearch}
            slotProps={{
                input: {
                    startAdornment: (
                        <InputAdornment position="start">
                            <SearchIcon />
                        </InputAdornment>
                    ),
                }
            }}
            onChange={(e) => {
                setLocalSearch(e.target.value);
                debouncedSearch(e);
            }}
        />
    )
}
