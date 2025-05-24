import React from 'react';
import {
    Box,
    Typography,
    Paper,
    Card,
    CardContent,
    Rating,
    CircularProgress,
    Alert,
    Button,
    Breadcrumbs,
    Link as MuiLink,
    Pagination,
    Chip,
    Avatar,
    IconButton,
} from '@mui/material';
import { Link, useParams, useNavigate } from 'react-router-dom';
import { ArrowBack, ThumbDown, ThumbUp } from '@mui/icons-material';
import { useFetchProductQuery, useFetchProductReviewsQuery } from '../products/productsApi';
import { useAppDispatch, useAppSelector } from '../../app/store/store';
import { setPageNumber } from '../products/reviewsSlice';

export default function ReviewListView() {
    const { productId } = useParams<{ productId: string }>();
    const reviewListParams = useAppSelector(state => state.reviews);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const {
        data: product,
        isLoading: isLoadingProduct,
        error: productError
    } = useFetchProductQuery(Number(productId));

    const {
        data: reviewsData,
        isLoading: isLoadingReviews,
        error: reviewsError,
        refetch
    } = useFetchProductReviewsQuery({
        productId: Number(productId),
        params: reviewListParams,
    }, { skip: !productId });

    const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
        dispatch(setPageNumber(value));
    }

    if (isLoadingProduct || isLoadingReviews) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
                <CircularProgress />
            </Box>
        );
    }

    if (productError) {
        return (
            <Box p={3}>
                <Alert severity="error">
                    Error loading product information.
                    <Button sx={{ ml: 2 }} variant="outlined" onClick={() => navigate('/products')}>
                        Return to Products
                    </Button>
                </Alert>
            </Box>
        );
    }

    if (reviewsError) {
        return (
            <Box p={3}>
                <Alert severity="error">
                    Error loading reviews.
                    <Button sx={{ ml: 2 }} variant="outlined" onClick={() => refetch()}>
                        Retry
                    </Button>
                </Alert>
            </Box>
        );
    }

    const noReviews = !reviewsData || reviewsData.items.length === 0;

    return (
        <Box sx={{ p: 3 }}>
            {/* Breadcrumbs Navigation */}
            <Breadcrumbs sx={{ mb: 2 }}>
                <MuiLink component={Link} to="/products" underline="hover" color="inherit">
                    Products
                </MuiLink>
                {product && (
                    <MuiLink
                        component={Link}
                        to={`/products/edit/${productId}`}
                        underline="hover"
                        color="inherit"
                    >
                        {product.name}
                    </MuiLink>
                )}
                <Typography color="text.primary">Reviews</Typography>
            </Breadcrumbs>

            {/* Header Section */}
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                <Button
                    startIcon={<ArrowBack />}
                    onClick={() => navigate('/products')}
                    variant="outlined"
                    sx={{ mr: 2 }}
                >
                    Back to Products
                </Button>

                <Typography variant="h5" component="h1">
                    Reviews for {' '}
                    <Typography color='primary' component="span" variant='h5'>
                        {product?.name || 'Product'}
                    </Typography>
                </Typography>
            </Box>


            {/* Summary Card */}
            {product && (
                <Paper
                    sx={{
                        p: 1.5,
                        mb: 2,
                        borderRadius: 2,
                        boxShadow: '0 2px 8px rgba(0,0,0,0.1)'
                    }}
                >
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <Box display="flex" alignItems="center" gap={3}>
                            <Box
                                component="img"
                                alt="Product Image"
                                src={product.mainImageUrl ?? "https://fakeimg.pl/600x400?text=No+Image"}
                                sx={{
                                    width: 80,
                                    height: 80,
                                    objectFit: 'cover',
                                    borderRadius: 2,
                                    boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
                                }}
                            />
                            <Box>
                                <Typography variant="h6" sx={{ mb: 1, fontWeight: 600 }}>
                                    Product Information
                                </Typography>
                                <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                                    ID: {product.id}
                                </Typography>
                                <Typography variant="h6" color="primary" sx={{ mb: 0.5 }}>
                                    ${product.basePrice?.toFixed(2)}
                                </Typography>
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                    <Rating value={product.averageRating} readOnly precision={0.1} size="small" />
                                    <Typography variant="body2" color="text.secondary">
                                        {product.averageRating} ({product.reviewCount} reviews)
                                    </Typography>
                                </Box>
                            </Box>
                        </Box>
                        <Box>
                            <Button
                                variant="contained"
                                component={Link}
                                to={`/products/edit/${productId}`}
                                sx={{
                                    borderRadius: 2,
                                    textTransform: 'none',
                                    px: 3,
                                    py: 1
                                }}
                            >
                                Edit Product
                            </Button>
                        </Box>
                    </Box>
                </Paper>
            )}

            {/* Reviews Section Header */}
            <Box sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                mb: 3
            }}>
                <Typography variant="h5" sx={{ fontWeight: 600 }}>
                    {noReviews
                        ? "Customer Reviews"
                        : `Customer Reviews (${reviewsData.metadata.totalCount})`
                    }
                </Typography>
                {!noReviews && (
                    <Chip
                        label={`${reviewsData.metadata.totalCount} reviews`}
                        color="primary"
                        variant="outlined"
                        size="small"
                    />
                )}
            </Box>

            {noReviews ? (
                <Paper sx={{
                    p: 2,
                    textAlign: 'center',
                    borderRadius: 2,
                    border: '2px dashed',
                    borderColor: 'divider',
                    bgcolor: 'background.default'
                }}>
                    <Box sx={{ mb: 2 }}>
                        <Typography variant="h6" color="text.secondary" sx={{ mb: 1 }}>
                            No Reviews Yet
                        </Typography>
                        <Typography variant="body1" color="text.secondary">
                            This product hasn't received any customer reviews yet.
                        </Typography>
                    </Box>
                </Paper>
            ) : (
                <>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        {reviewsData.items.map((review) => (
                            <Card
                                key={review.id}
                                sx={{
                                    borderRadius: 2,
                                    border: '1px solid',
                                    borderColor: 'divider',
                                    transition: 'all 0.2s ease-in-out',
                                    '&:hover': {
                                        boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
                                        borderColor: 'primary.light'
                                    }
                                }}
                            >
                                <CardContent sx={{ p: 3 }}>
                                    {/* Review Header */}
                                    <Box sx={{
                                        display: 'flex',
                                        justifyContent: 'space-between',
                                        alignItems: 'flex-start',
                                        mb: 1.5
                                    }}>
                                        <Box sx={{ flex: 1 }}>
                                            <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 1 }}>
                                                <Avatar sx={{ width: 40, height: 40, bgcolor: 'primary.main' }}>
                                                    {(review.reviewer || 'A')[0].toUpperCase()}
                                                </Avatar>
                                                <Box>
                                                    <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
                                                        {review.reviewer || 'Anonymous User'}
                                                    </Typography>
                                                    <Typography variant="caption" color="text.secondary">
                                                        {new Date(review.createdDate).toLocaleDateString('en-US', {
                                                            year: 'numeric',
                                                            month: 'long',
                                                            day: 'numeric'
                                                        })}
                                                    </Typography>
                                                </Box>
                                            </Box>
                                        </Box>
                                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                            <Rating value={review.rating} readOnly precision={0.5} size="small" />
                                            <Chip
                                                label={review.rating}
                                                size="small"
                                                color="primary"
                                                sx={{ minWidth: 40 }}
                                            />
                                        </Box>
                                    </Box>

                                    {/* Review Title */}
                                    {review.title && (
                                        <Typography
                                            variant="h6"
                                            sx={{
                                                mb: 1,
                                                fontWeight: 600,
                                                color: 'text.primary'
                                            }}
                                        >
                                            "{review.title}"
                                        </Typography>
                                    )}

                                    {/* Review Body */}
                                    <Typography
                                        variant="body1"
                                        sx={{
                                            lineHeight: 1.6,
                                            color: 'text.secondary'
                                        }}
                                    >
                                        {review.body}
                                    </Typography>

                                    {/* Review Footer */}
                                    <Box sx={{
                                        display: 'flex',
                                        justifyContent: 'space-between',
                                        alignItems: 'center',
                                        pt: 1,
                                        mt: 1,
                                        borderTop: '1px solid',
                                        borderColor: 'divider'
                                    }}>
                                        <Typography variant="caption" color="text.secondary">
                                            Review #{review.id}
                                        </Typography>
                                        <Box sx={{ display: 'flex', gap: 1 }}>
                                            <IconButton size="small" color="primary">
                                                <ThumbUp fontSize="small" />
                                            </IconButton>
                                            <IconButton size="small">
                                                <ThumbDown fontSize="small" />
                                            </IconButton>
                                        </Box>
                                    </Box>
                                </CardContent>
                            </Card>
                        ))}
                    </Box>

                    {/* Enhanced Pagination */}
                    {reviewsData.metadata.totalPages > 1 && (
                        <Box sx={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            p: 2,
                            mt: 3,
                            bgcolor: 'background.paper',
                            borderRadius: 2,
                            border: '1px solid',
                            borderColor: 'divider'
                        }}>
                            <Pagination
                                count={reviewsData.metadata.totalPages}
                                page={reviewsData.metadata.currentPage}
                                onChange={handlePageChange}
                                color="primary"
                                showFirstButton
                                showLastButton
                                sx={{
                                    '& .MuiPaginationItem-root': {
                                        borderRadius: 2
                                    }
                                }}
                            />
                        </Box>
                    )}
                </>
            )}
        </Box>
    );
}