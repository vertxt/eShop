import { Product } from "./product";

export type Category = {
    id: number;
    name: string;
    description: string;
}

export type CategoryDetail = {
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