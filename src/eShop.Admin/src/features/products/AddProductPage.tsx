import { useNavigate } from "react-router-dom";
import ProductForm from "./ProductForm";

export default function AddProductPage() {
    const navigate = useNavigate();

    const onSuccess = () => {
        navigate('/products');
    }

    return (
        <ProductForm onSuccess={onSuccess} />
    )
}