import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/store/store';
import {
    setCategoryIds,
    setIsActive,
    setPriceRange,
    setHasVariants,
    setStockRange,
    resetFilters,
} from './productsSlice';
import {
    Box,
    Button,
    Checkbox,
    Collapse,
    Divider,
    FormControl,
    FormControlLabel,
    FormGroup,
    FormLabel,
    IconButton,
    Paper,
    Radio,
    RadioGroup,
    Slider,
    Stack,
    TextField,
    Typography
} from '@mui/material';
import { ExpandMore, ExpandLess } from '@mui/icons-material';
import { useFetchCategoriesQuery } from '../categories/categoriesApi';
import { StockRangeFilter } from '../../shared/types/productListParams';

export default function ProductFilters() {
    const dispatch = useAppDispatch();
    const filters = useAppSelector(state => state.products);
    const { data: categories } = useFetchCategoriesQuery();

    const [priceRange, setPriceRangeLocal] = useState<[number, number]>([0, 1000]);
    const [showFilters, setShowFilters] = useState(true);

    useEffect(() => {
        if (filters.minPrice != null && filters.maxPrice != null) {
            setPriceRangeLocal([filters.minPrice, filters.maxPrice]);
        }
    }, []);

    const handleCategoryChange = (categoryId: number) => {
        const currentIds = filters.categoryIds ? [...filters.categoryIds] : [];
        const index = currentIds.indexOf(categoryId);

        if (index === -1) {
            currentIds.push(categoryId);
        } else {
            currentIds.splice(index, 1);
        }

        dispatch(setCategoryIds(currentIds));
    };

    const handlePriceRangeChange = (_: Event, newValue: number | number[]) => {
        const [min, max] = newValue as number[];
        setPriceRangeLocal([min, max]);
    };

    const handlePriceRangeCommit = () => {
        dispatch(setPriceRange({ min: priceRange[0], max: priceRange[1] }));
    };

    const handleReset = () => {
        dispatch(resetFilters());
        setPriceRangeLocal([0, 1000]);
    };

    return (
        <Paper sx={{ p: 2, mb: 3 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
                <Typography variant="h6" component="h2">
                    Filters
                </Typography>
                <IconButton onClick={() => setShowFilters(!showFilters)}>
                    {showFilters ? <ExpandLess /> : <ExpandMore />}
                </IconButton>
            </Box>

            <Collapse in={showFilters}>
                <Stack spacing={3} sx={{ mt: 2 }}>
                    {/* Categories */}
                    <Box>
                        <Typography variant="subtitle1" gutterBottom>Categories</Typography>
                        <FormGroup>
                            {categories?.map(category => (
                                <FormControlLabel
                                    key={category.id}
                                    control={
                                        <Checkbox
                                            checked={filters.categoryIds?.includes(category.id)}
                                            onChange={() => handleCategoryChange(category.id)}
                                        />
                                    }
                                    label={category.name}
                                />
                            ))}
                        </FormGroup>
                    </Box>

                    <Divider />

                    {/* Active Status */}
                    <FormControl component="fieldset">
                        <FormLabel component="legend">Status</FormLabel>
                        <RadioGroup
                            value={filters.isActive == null ? 'all' : filters.isActive.toString()}
                            onChange={(e) => {
                                const value = e.target.value;
                                dispatch(setIsActive(
                                    value === 'all' ? null : value === 'true'
                                ));
                            }}
                        >
                            <FormControlLabel value="all" control={<Radio />} label="All" />
                            <FormControlLabel value="true" control={<Radio />} label="Active" />
                            <FormControlLabel value="false" control={<Radio />} label="Inactive" />
                        </RadioGroup>
                    </FormControl>

                    <Divider />

                    {/* Price Range */}
                    <Box>
                        <Typography variant="subtitle1" gutterBottom>Price Range</Typography>
                        <Slider
                            value={priceRange}
                            onChange={handlePriceRangeChange}
                            onChangeCommitted={handlePriceRangeCommit}
                            valueLabelDisplay="auto"
                            min={0}
                            max={1000}
                            step={10}
                        />
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 1 }}>
                            <TextField
                                label="Min"
                                type="number"
                                size="small"
                                value={priceRange[0]}
                                onChange={(e) => {
                                    const value = parseInt(e.target.value);
                                    setPriceRangeLocal([value, priceRange[1]]);
                                }}
                                onBlur={handlePriceRangeCommit}
                            />
                            <TextField
                                label="Max"
                                type="number"
                                size="small"
                                value={priceRange[1]}
                                onChange={(e) => {
                                    const value = parseInt(e.target.value);
                                    setPriceRangeLocal([priceRange[0], value]);
                                }}
                                onBlur={handlePriceRangeCommit}
                            />
                        </Box>
                    </Box>

                    <Divider />

                    {/* Variants */}
                    <FormControl component="fieldset">
                        <FormLabel component="legend">Variants</FormLabel>
                        <RadioGroup
                            value={filters.hasVariants == null ? 'all' : filters.hasVariants.toString()}
                            onChange={(e) => {
                                const value = e.target.value;
                                dispatch(setHasVariants(
                                    value === 'all' ? null : value === 'true'
                                ));
                            }}
                        >
                            <FormControlLabel value="all" control={<Radio />} label="All Products" />
                            <FormControlLabel value="true" control={<Radio />} label="With Variants" />
                            <FormControlLabel value="false" control={<Radio />} label="Without Variants" />
                        </RadioGroup>
                    </FormControl>

                    <Divider />

                    {/* Stock Status */}
                    <FormControl component="fieldset">
                        <FormLabel component="legend">Stock Level</FormLabel>
                        <RadioGroup
                            value={filters.stockRange}
                            onChange={(e) => {
                                const value = e.target.value as StockRangeFilter;
                                dispatch(setStockRange(value));
                            }}
                        >
                            <FormControlLabel value="all" control={<Radio />} label="All Products" />
                            <FormControlLabel value="outOfStock" control={<Radio />} label="Out of Stock (0)" />
                            <FormControlLabel value="low" control={<Radio />} label="Low Stock (1-10)" />
                            <FormControlLabel value="medium" control={<Radio />} label="Medium Stock (11-50)" />
                            <FormControlLabel value="high" control={<Radio />} label="High Stock (50+)" />
                        </RadioGroup>
                    </FormControl>

                    <Divider />
                </Stack>

                <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3 }}>
                    <Button variant="outlined" color="error" onClick={handleReset}>
                        Reset All Filters
                    </Button>
                </Box>
            </Collapse>
        </Paper>
    );
}