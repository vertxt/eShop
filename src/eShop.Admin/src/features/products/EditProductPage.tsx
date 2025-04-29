import { useNavigate, useParams } from "react-router-dom";
import ProductForm from "./ProductForm";

export default function EditProductPage()
{
    const navigate = useNavigate();
    const { id } = useParams();

    const onSuccess = () => {
        navigate('/products');
    }
    return (
        <ProductForm onSuccess={onSuccess} productId={Number(id)} />
    )
}