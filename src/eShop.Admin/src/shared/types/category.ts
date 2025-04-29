import { Product } from "./product";

export interface Category {
    id: number;
    name: string;
    description: string;
}

export interface CategoryDetail {
    id: number;
    name: string,
    description: string;
    attributes: CategoryAttribute[],
    products: Product[],
}

export type CategoryAttribute = {
    id: number;
    name: string;
    displayName: string;
}