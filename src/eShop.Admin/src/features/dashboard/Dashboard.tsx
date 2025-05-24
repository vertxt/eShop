import React from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box,
    Card,
    CardContent,
    Typography,
    Grid,
    Paper,
    Avatar,
    Divider,
    List,
    ListItem,
    ListItemAvatar,
    ListItemText,
    Rating,
    Chip,
    CircularProgress,
    useTheme,
} from '@mui/material';
import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
    PieChart,
    Pie,
    Cell
} from 'recharts';
import {
    ShoppingBag,
    Category as CategoryIcon,
    Star,
    People,
    TrendingUp,
    AttachMoney,
    RateReview,
    FeaturedPlayList,
    Warning,
    ArrowForward
} from '@mui/icons-material';
import { useFetchDashboardSummaryQuery, useFetchLatestFeaturedProductsQuery, useFetchLowStockProductsQuery, useFetchProductsByCategoryQuery, useFetchRatingDistributionQuery, useFetchRecentReviewsQuery } from './dashboardApi';


export default function Dashboard() {
    const theme = useTheme();
    const navigate = useNavigate();
    const { data: summary, isLoading: isLoadingSummary } = useFetchDashboardSummaryQuery()
    const { data: productsByCategory, isLoading: isLoadingProductsByCategory } = useFetchProductsByCategoryQuery();
    const { data: ratingDistribution, isLoading: isLoadingRatingDistribution } = useFetchRatingDistributionQuery();
    const { data: lowStockProducts, isLoading: isLoadingLowStockProducts } = useFetchLowStockProductsQuery();
    const { data: latestFeaturedProducts, isLoading: isLoadingLatestFeaturedProducts } = useFetchLatestFeaturedProductsQuery(5);
    const { data: recentReviews, isLoading: isLoadingRecentReviews } = useFetchRecentReviewsQuery();

    const isLoading = isLoadingSummary ||
        isLoadingProductsByCategory ||
        isLoadingRatingDistribution ||
        isLoadingLowStockProducts ||
        isLoadingLatestFeaturedProducts ||
        isLoadingRecentReviews;

    // Colors for charts - compatible with dark mode
    const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884d8'];

    // Transform API data to match chart expectations
    const transformedProductsByCategory = React.useMemo(() => {
        if (!productsByCategory) return [];
        return productsByCategory.map(item => ({
            name: item.categoryName,
            products: item.productCount,
            percentage: item.percentageOfTotal
        }));
    }, [productsByCategory]);

    const transformedRatingDistribution = React.useMemo(() => {
        if (!ratingDistribution) return [];
        return ratingDistribution.map(item => ({
            name: `${item.rating} Stars`,
            value: item.count,
            rating: item.rating
        }));
    }, [ratingDistribution]);

    // Theme-aware colors for metric cards
    const getMetricCardColors = () => ({
        products: {
            bg: theme.palette.mode === 'dark' ? '#1a237e' : '#f3f8ff',
            avatar: '#2196f3'
        },
        categories: {
            bg: theme.palette.mode === 'dark' ? '#1b5e20' : '#f3fff5',
            avatar: '#4caf50'
        },
        rating: {
            bg: theme.palette.mode === 'dark' ? '#e65100' : '#fff8e1',
            avatar: '#ff9800'
        },
        users: {
            bg: theme.palette.mode === 'dark' ? '#880e4f' : '#fdf0f8',
            avatar: '#e91e63'
        }
    });

    const cardColors = getMetricCardColors();

    if (isLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Box sx={{ p: 3 }}>
            <Typography variant="h4" sx={{ mb: 4, fontWeight: 'bold' }}>
                Dashboard
            </Typography>

            {/* Key Metrics */}
            <Grid container spacing={3} sx={{ mb: 4 }}>
                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Card sx={{ height: '100%', bgcolor: cardColors.products.bg, boxShadow: 3 }}>
                        <CardContent>
                            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                <Avatar sx={{ bgcolor: cardColors.products.avatar, mr: 2 }}>
                                    <ShoppingBag sx={{ fontSize: 24 }} />
                                </Avatar>
                                <Typography variant="h6" sx={{ fontWeight: 'medium' }}>Products</Typography>
                            </Box>
                            <Typography variant="h4" sx={{ fontWeight: 'bold' }}>{summary?.totalProducts}</Typography>
                            <Typography variant="body2" color="text.secondary">Total products in inventory</Typography>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Card sx={{ height: '100%', bgcolor: cardColors.categories.bg, boxShadow: 3 }}>
                        <CardContent>
                            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                <Avatar sx={{ bgcolor: cardColors.categories.avatar, mr: 2 }}>
                                    <CategoryIcon sx={{ fontSize: 24 }} />
                                </Avatar>
                                <Typography variant="h6" sx={{ fontWeight: 'medium' }}>Categories</Typography>
                            </Box>
                            <Typography variant="h4" sx={{ fontWeight: 'bold' }}>{summary?.totalCategories}</Typography>
                            <Typography variant="body2" color="text.secondary">Product categories</Typography>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Card sx={{ height: '100%', bgcolor: cardColors.rating.bg, boxShadow: 3 }}>
                        <CardContent>
                            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                <Avatar sx={{ bgcolor: cardColors.rating.avatar, mr: 2 }}>
                                    <Star sx={{ fontSize: 24 }} />
                                </Avatar>
                                <Typography variant="h6" sx={{ fontWeight: 'medium' }}>Rating</Typography>
                            </Box>
                            <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                <Typography variant="h4" sx={{ fontWeight: 'bold', mr: 1 }}>
                                    {summary?.averageRating?.toFixed(1)}
                                </Typography>
                                <Rating value={summary?.averageRating} precision={0.5} readOnly size="small" />
                            </Box>
                            <Typography variant="body2" color="text.secondary">Average product rating</Typography>
                        </CardContent>
                    </Card>
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 3 }}>
                    <Card sx={{ height: '100%', bgcolor: cardColors.users.bg, boxShadow: 3 }}>
                        <CardContent>
                            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                <Avatar sx={{ bgcolor: cardColors.users.avatar, mr: 2 }}>
                                    <People sx={{ fontSize: 24 }} />
                                </Avatar>
                                <Typography variant="h6" sx={{ fontWeight: 'medium' }}>Users</Typography>
                            </Box>
                            <Typography variant="h4" sx={{ fontWeight: 'bold' }}>{summary?.totalUsers}</Typography>
                            <Typography variant="body2" color="text.secondary">Registered users</Typography>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>

            {/* Charts Row */}
            <Grid container spacing={3} sx={{ mb: 4 }}>
                <Grid size={{ xs: 12, md: 8 }}>
                    <Paper sx={{ p: 3, height: '100%', boxShadow: 3 }}>
                        <Typography variant="h6" sx={{ mb: 2 }}>Products by Category</Typography>
                        {transformedProductsByCategory.length > 0 ? (
                            <ResponsiveContainer width="100%" height={300}>
                                <BarChart data={transformedProductsByCategory}>
                                    <CartesianGrid
                                        strokeDasharray="3 3"
                                        stroke={theme.palette.mode === 'dark' ? '#555' : '#ccc'}
                                    />
                                    <XAxis
                                        dataKey="name"
                                        tick={{ fill: theme.palette.text.primary, fontSize: 12 }}
                                        angle={-45}
                                        textAnchor="end"
                                        height={80}
                                    />
                                    <YAxis tick={{ fill: theme.palette.text.primary }} />
                                    <Tooltip
                                        contentStyle={{
                                            backgroundColor: theme.palette.background.paper,
                                            border: `1px solid ${theme.palette.divider}`,
                                            borderRadius: 8,
                                            color: theme.palette.text.primary
                                        }}
                                    />
                                    <Bar dataKey="products" fill="#8884d8" />
                                </BarChart>
                            </ResponsiveContainer>
                        ) : (
                            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 300 }}>
                                <Typography variant="body2" color="text.secondary">No category data available</Typography>
                            </Box>
                        )}
                    </Paper>
                </Grid>

                <Grid size={{ xs: 12, md: 4 }}>
                    <Paper sx={{ p: 3, height: '100%', boxShadow: 3 }}>
                        <Typography variant="h6" sx={{ mb: 2 }}>Rating Distribution</Typography>
                        {transformedRatingDistribution.length > 0 ? (
                            <ResponsiveContainer width="100%" height={300}>
                                <PieChart>
                                    <Pie
                                        data={transformedRatingDistribution}
                                        cx="50%"
                                        cy="50%"
                                        innerRadius={60}
                                        outerRadius={80}
                                        fill="#8884d8"
                                        paddingAngle={5}
                                        dataKey="value"
                                        label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                                    >
                                        {transformedRatingDistribution.map((_entry, index) => (
                                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                                        ))}
                                    </Pie>
                                    <Tooltip
                                        contentStyle={{
                                            backgroundColor: theme.palette.background.paper,
                                            border: `1px solid ${theme.palette.divider}`,
                                            borderRadius: 8,
                                        }}
                                        labelStyle={{ color: theme.palette.text.primary }}
                                        itemStyle={{ color: theme.palette.text.primary }}
                                    />
                                </PieChart>
                            </ResponsiveContainer>
                        ) : (
                            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: 300 }}>
                                <Typography variant="body2" color="text.secondary">No rating data available</Typography>
                            </Box>
                        )}
                    </Paper>
                </Grid>
            </Grid>

            {/* Additional Info Row */}
            <Grid container spacing={3}>
                <Grid size={{ xs: 12, md: 7 }}>
                    <Paper sx={{ p: 3, height: '100%', boxShadow: 3 }}>
                        <Typography variant="h6" sx={{ mb: 2 }}>Recent Reviews</Typography>
                        {recentReviews && recentReviews.length > 0 ? (
                            <List>
                                {recentReviews.map((review, index) => (
                                    <React.Fragment key={review.id}>
                                        <ListItem
                                            alignItems="flex-start"
                                            sx={{
                                                cursor: 'pointer',
                                                '&:hover': {
                                                    backgroundColor: theme.palette.action.hover,
                                                    borderRadius: 1
                                                }
                                            }}
                                            onClick={() => navigate(`/products/${review.productId}/reviews`)}
                                        >
                                            <ListItemAvatar>
                                                <Avatar sx={{ bgcolor: COLORS[index % COLORS.length] }}>
                                                    {review.productId}
                                                </Avatar>
                                            </ListItemAvatar>
                                            <ListItemText
                                                primary={
                                                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                                        <Typography variant="subtitle1">{review.title}</Typography>
                                                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                                            <Rating value={review.rating} readOnly size="small" />
                                                            <ArrowForward sx={{ fontSize: 16, color: 'text.secondary' }} />
                                                        </Box>
                                                    </Box>
                                                }
                                                secondary={
                                                    <React.Fragment>
                                                        <Typography component="span" variant="body2" color="text.primary">
                                                            {review.reviewer}
                                                        </Typography>
                                                        <Typography variant="caption" display="block" color="text.secondary" sx={{ mt: 0.5 }}>
                                                            Product ID: {review.productId}
                                                        </Typography>
                                                        <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
                                                            {review.body}
                                                        </Typography>
                                                        <Typography variant="caption" display="block" color="text.secondary" sx={{ mt: 0.5 }}>
                                                            {new Date(review.createdDate).toLocaleDateString()}
                                                        </Typography>
                                                    </React.Fragment>
                                                }
                                            />
                                        </ListItem>
                                        {index < recentReviews.length - 1 && <Divider variant="inset" component="li" />}
                                    </React.Fragment>
                                ))}
                            </List>
                        ) : (
                            <Box sx={{
                                display: 'flex',
                                flexDirection: 'column',
                                alignItems: 'center',
                                justifyContent: 'center',
                                py: 4,
                                textAlign: 'center'
                            }}>
                                <Avatar sx={{
                                    bgcolor: theme.palette.mode === 'dark' ? theme.palette.grey[700] : theme.palette.grey[200],
                                    mb: 2,
                                    width: 56,
                                    height: 56
                                }}>
                                    <RateReview sx={{ fontSize: 24 }} />
                                </Avatar>
                                <Typography variant="h6" color="text.secondary" gutterBottom>
                                    No Recent Reviews
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    Customer reviews will appear here once they start rating your products
                                </Typography>
                            </Box>
                        )}
                    </Paper>
                </Grid>

                <Grid size={{ xs: 12, md: 5 }}>
                    <Paper sx={{ p: 3, height: '100%', boxShadow: 3 }}>
                        <Typography variant="h6" sx={{ mb: 2 }}>Inventory Insights</Typography>

                        <Grid container spacing={2} sx={{ mb: 3 }}>
                            <Grid size={6}>
                                <Card sx={{
                                    bgcolor: theme.palette.mode === 'dark' ? theme.palette.grey[800] : '#eeeeee'
                                }}>
                                    <CardContent>
                                        <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                            <AttachMoney sx={{
                                                color: theme.palette.mode === 'dark' ? theme.palette.grey[400] : "#616161",
                                                fontSize: 20
                                            }} />
                                            <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                                                Inventory Value
                                            </Typography>
                                        </Box>
                                        <Typography variant="h6">
                                            ${summary?.totalInventoryValue?.toLocaleString()}
                                        </Typography>
                                    </CardContent>
                                </Card>
                            </Grid>

                            <Grid size={6}>
                                <Card sx={{
                                    bgcolor: theme.palette.mode === 'dark' ? theme.palette.grey[800] : '#eeeeee'
                                }}>
                                    <CardContent>
                                        <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                            <TrendingUp sx={{
                                                fontSize: 20,
                                                color: theme.palette.mode === 'dark' ? theme.palette.grey[400] : "#616161"
                                            }} />
                                            <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                                                Avg. Price
                                            </Typography>
                                        </Box>
                                        <Typography variant="h6">
                                            ${summary?.averageProductPrice}
                                        </Typography>
                                    </CardContent>
                                </Card>
                            </Grid>
                        </Grid>

                        <Typography variant="subtitle2" sx={{ mb: 1, mt: 2 }}>Latest Featured Products</Typography>
                        {(latestFeaturedProducts && latestFeaturedProducts.length > 0) ? latestFeaturedProducts.map((product, index) => (
                            <Box key={index} sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 1 }}>
                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                    <Avatar sx={{ width: 28, height: 28, mr: 1 }} src={product.mainImageUrl} variant='square' />
                                    <Typography variant="body2">{product.name}</Typography>
                                </Box>
                                <Chip
                                    label={`$${product.basePrice}`}
                                    size="small"
                                    sx={{
                                        bgcolor: COLORS[index % COLORS.length] + '30',
                                        fontWeight: 'medium',
                                        color: theme.palette.mode === 'dark' ? 'white' : 'inherit'
                                    }}
                                />
                            </Box>
                        )) : (
                            <Box sx={{
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                py: 2,
                                backgroundColor: theme.palette.mode === 'dark' ? theme.palette.grey[800] : theme.palette.grey[50],
                                borderRadius: 1,
                                mb: 2
                            }}>
                                <FeaturedPlayList sx={{ mr: 1, color: 'text.secondary' }} />
                                <Typography variant="body2" color="text.secondary">
                                    No featured products available
                                </Typography>
                            </Box>
                        )}

                        <Typography variant="subtitle2" sx={{ mb: 1, mt: 3 }}>Low Stock Products</Typography>
                        {(lowStockProducts && lowStockProducts.length > 0) ? lowStockProducts.map((product, index) => (
                            <Box key={index} sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 1 }}>
                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                    <Avatar sx={{ width: 28, height: 28, mr: 1 }} src={product.mainImageUrl} variant='square' />
                                    <Typography variant="body2" noWrap sx={{ maxWidth: '250px' }}>{product.name}</Typography>
                                </Box>
                                <Chip
                                    label={`${product.quantityInStock} left`}
                                    size="small"
                                    color="error"
                                    variant="outlined"
                                />
                            </Box>
                        )) : (
                            <Box sx={{
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                py: 2,
                                backgroundColor: theme.palette.mode === 'dark' ? theme.palette.grey[800] : theme.palette.grey[50],
                                borderRadius: 1
                            }}>
                                <Warning sx={{ mr: 1, color: 'success.main' }} />
                                <Typography variant="body2" color="success.main">
                                    All products are well stocked
                                </Typography>
                            </Box>
                        )}
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    );
}