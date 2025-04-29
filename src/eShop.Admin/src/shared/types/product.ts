export interface Product {
    id: number;
    uuid: string;
    name: string;
    basePrice: number;
    shortDescription: string;
    mainImageUrl: string;
    isActive: boolean;
    categoryName: string;
    hasVariants: boolean;
    quantityInStock: number | null;
    createdDate: string;
    updatedDate: string | null;
    reviewCount: number;
    averageRating: number;
}

export interface ProductDetail {
    id: number;
    uuid: string;
    name: string;
    basePrice: number;
    description: string;
    shortDescription: string;
    isActive: boolean;
    categoryId: number;
    categoryName: string;
    hasVariants: boolean;
    quantityInStock: number | null;
    createdDate: string;
    updatedDate: string | null;
    images: ProductImage[];
    variants: ProductVariant[];
    attributes: ProductAttribute[];
    reviews: ProductReview[];
    averageRating: number;
}

export interface ProductImage {
    id: number;
    url: string;
    altText: string;
    isMain: boolean;
    displayOrder: number;
}

export interface ProductVariant {
    id: number;
    name: string;
    price: number;
    quantityInStock: number | null;
    sku: string;
    isActive: boolean;
}

export interface ProductAttribute {
    id: number;
    attributeId: number;
    name: string;
    displayName: string;
    value: string;
}

export interface ProductReview {
    id: number;
    reviewerName: string;
    rating: number;
    title: string;
    body: string;
    createdDate: string;
}