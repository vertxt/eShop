export type Product = {
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

export type ProductImage = {
    id: number;
    url: string;
    isMain: boolean;
    displayOrder: number;
}

export type ProductVariant = {
    id: number;
    name: string;
    price: number;
    quantityInStock: number | null;
}

export type ProductAttribute = {
    id: number;
    name: string;
    value: string;
}
