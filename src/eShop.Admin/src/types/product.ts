export interface Product {
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
}

export interface ProductImage {
    id: number;
    url: string;
    isMain: boolean;
    displayOrder: number;
}

export interface ProductVariant {
    id: number;
    name: string;
    price: number;
    quantityInStock: number | null;
}

export interface ProductAttribute {
    id: number;
    name: string;
    value: string;
}