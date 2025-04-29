export interface Category {
    id: number;
    name: string;
    description: string | null;
    attributes: CategoryAttribute[],
}

export type CategoryAttribute = {
    id: number;
    name: string;
    displayName: string;
}