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
} from '@mui/material';
import { Link, useParams, useNavigate } from 'react-router-dom';
import { ArrowBack } from '@mui/icons-material';
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
                <Paper sx={{ p: 2, mb: 3 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <Box display="flex" alignItems="center" gap={3}>
                            <Box
                                component="img"
                                alt="Product Image"
                                src={ product.mainImageUrl ?? "https://fakeimg.pl/600x400?text=No+Image" }
                                sx={{ width: 64, height: 64, objectFit: 'cover' }}
                            />
                            <Box>
                                <Typography variant="h6">Product Information</Typography>
                                <Typography variant="body1">ID: {product.id}</Typography>
                                <Typography variant="body1">Price: ${product.basePrice?.toFixed(2)}</Typography>
                                <Typography variant="body1">Average Rating: {product.averageRating} ({product.reviewCount} reviews)</Typography>
                            </Box>
                        </Box>
                        <Box>
                            <Button
                                variant="contained"
                                component={Link}
                                to={`/products/edit/${productId}`}
                            >
                                Edit
                            </Button>
                        </Box>
                    </Box>
                </Paper>
            )}

            {/* Reviews Section */}
            <Typography variant="h6" sx={{ mb: 2 }}>
                {noReviews
                    ? "No reviews found for this product"
                    : `Reviews (${reviewsData.metadata.totalCount})`
                }
            </Typography>

            {noReviews ? (
                <Paper sx={{ p: 3, textAlign: 'center' }}>
                    <Typography variant="body1">
                        This product has no customer reviews yet.
                    </Typography>
                </Paper>
            ) : (
                <>
                    {reviewsData.items.map((review) => (
                        <Card key={review.id} sx={{ mb: 2 }}>
                            <CardContent>
                                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                                    <Typography variant="subtitle1" fontWeight="bold">
                                        {review.reviewer || 'Anonymous User'}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        {new Date(review.createdDate).toLocaleDateString()}
                                    </Typography>
                                </Box>
                                <Rating value={review.rating} readOnly precision={0.5} />
                                <Typography variant="body1" sx={{ mt: 1 }}>
                                    {review.body}
                                </Typography>
                            </CardContent>
                        </Card>
                    ))}

                    {/* Pagination */}
                    {reviewsData.metadata.totalPages > 1 && (
                        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                            <Pagination
                                count={reviewsData.metadata.totalPages}
                                page={reviewsData.metadata.currentPage}
                                onChange={handlePageChange}
                                color="primary"
                            />
                        </Box>
                    )}
                </>
            )}
        </Box>
    );
}