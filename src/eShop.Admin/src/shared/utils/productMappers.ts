import { ProductSchema } from "../schemas/createProductSchema";
import { ProductDetail } from "../types/product";

export function productDetailToForm(detail: ProductDetail): ProductSchema {
    return {
        ...detail,
        quantityInStock: detail.quantityInStock ?? 0,
        attributes: (detail.attributes || []).map(a => ({
            attributeId: a.attributeId,
            value: a.value || '',
        })),
        variants: (detail.variants || []).map(v => ({
            id: v.id,
            name: v.name,
            sku: v.sku || '',
            price: v.price,
            quantityInStock: v.quantityInStock != null ? v.quantityInStock : 0,
            isActive: v.isActive ?? false,
        })),
    };
};