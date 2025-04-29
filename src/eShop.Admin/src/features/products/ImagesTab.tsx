import { Box, Typography, Button, Grid, Paper, IconButton } from '@mui/material';
import { AddPhotoAlternate, DeleteOutline, ArrowBack, ArrowForward, Check } from '@mui/icons-material';
import { ImageFile } from './ProductForm';

type ImagesTabProps = {
    imageFiles: ImageFile[];
    setImageFiles: React.Dispatch<React.SetStateAction<ImageFile[]>>;
};

export default function ImagesTab({ imageFiles, setImageFiles }: ImagesTabProps) {
    // Add images
    const handleImageUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            const newFiles = Array.from(event.target.files).map((file, index) => ({
                id: crypto.randomUUID(),
                file,
                preview: URL.createObjectURL(file),
                isMain: imageFiles.length === 0 && index === 0, // First image is main by default
                displayOrder: imageFiles.length + index
            }));
            
            setImageFiles(prev => [...prev, ...newFiles]);
        }
    };

    // Remove an image
    const removeImage = (id: string) => {
        setImageFiles(prev => {
            const updatedFiles = prev.filter(img => img.id !== id);
            // If we removed the main image, make the first one the main
            if (prev.find(img => img.id === id)?.isMain && updatedFiles.length > 0) {
                updatedFiles[0].isMain = true;
            }
            return updatedFiles;
        });
    };

    // Set an image as main
    const setMainImage = (id: string) => {
        setImageFiles(prev => prev.map(img => ({
            ...img,
            isMain: img.id === id
        })));
    };

    // Method to move image order
    const moveImage = (id: string, direction: 'up' | 'down') => {
        setImageFiles(prev => {
            const index = prev.findIndex(img => img.id === id);
            if ((direction === 'up' && index === 0) || 
                (direction === 'down' && index === prev.length - 1)) {
                return prev;
            }
            
            const newIndex = direction === 'up' ? index - 1 : index + 1;
            const updatedFiles = [...prev];
            [updatedFiles[index], updatedFiles[newIndex]] = [updatedFiles[newIndex], updatedFiles[index]];
            
            // Update display orders
            return updatedFiles.map((img, idx) => ({
                ...img,
                displayOrder: idx
            }));
        });
    };
    
    return (
        <>
            <Box sx={{ mb: 3 }}>
                <Typography variant="h6" gutterBottom>
                    Product Images
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    Upload product images. The first image will be the main product image.
                </Typography>
            </Box>

            <Box sx={{ mb: 3 }}>
                <input
                    accept="image/*"
                    style={{ display: 'none' }}
                    id="image-upload"
                    type="file"
                    multiple
                    onChange={handleImageUpload}
                />
                <label htmlFor="image-upload">
                    <Button
                        variant="contained"
                        component="span"
                        startIcon={<AddPhotoAlternate />}
                    >
                        Upload Images
                    </Button>
                </label>
            </Box>

            {imageFiles.length > 0 && (
                <Box>
                    <Typography variant="subtitle1" gutterBottom>
                        Uploaded Images ({imageFiles.length})
                    </Typography>
                    <Grid container spacing={2}>
                        {imageFiles.map((img) => (
                            <Grid key={img.id} size={{ xs: 12, sm: 6, md: 4, lg: 3 }}>
                                <Paper elevation={2} sx={{ p: 1, position: 'relative' }}>
                                    <Box
                                        sx={{
                                            height: 200,
                                            backgroundImage: `url(${img.preview})`,
                                            backgroundSize: 'contain',
                                            backgroundPosition: 'center',
                                            backgroundRepeat: 'no-repeat',
                                            mb: 1,
                                            border: img.isMain ? '3px solid #1976d2' : 'none',
                                        }}
                                    />
                                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                        <Typography variant="caption" sx={{ fontWeight: img.isMain ? 'bold' : 'normal' }}>
                                            {img.isMain ? 'Main Image' : `Image ${img.displayOrder + 1}`}
                                        </Typography>
                                        <Box>
                                            {!img.isMain && (
                                                <IconButton
                                                    size="small"
                                                    onClick={() => setMainImage(img.id)}
                                                    title="Set as main image"
                                                >
                                                    <Check fontSize="small" />
                                                </IconButton>
                                            )}
                                            <IconButton
                                                size="small"
                                                onClick={() => moveImage(img.id, 'up')}
                                                disabled={img.displayOrder === 0}
                                                title="Move left (or up)"
                                            >
                                                <ArrowBack fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                size="small"
                                                onClick={() => moveImage(img.id, 'down')}
                                                disabled={img.displayOrder === imageFiles.length - 1}
                                                title="Move right (or down)"
                                            >
                                                <ArrowForward fontSize="small" />
                                            </IconButton>
                                            <IconButton
                                                size="small"
                                                onClick={() => removeImage(img.id)}
                                                title="Remove image"
                                                color="error"
                                            >
                                                <DeleteOutline fontSize="small" />
                                            </IconButton>
                                        </Box>
                                    </Box>
                                </Paper>
                            </Grid>
                        ))}
                    </Grid>
                </Box>
            )}
        </>
    );
}