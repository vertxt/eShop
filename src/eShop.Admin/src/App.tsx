import { useEffect, useState } from "react";
import { Product } from "./types/product";

function App() {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetch("https://localhost:5000/api/products")
      .then(result => result.json())
      .then(json => setProducts(json));
  }, []);

  return (
    <div>
      {products.map((p, index) => (
        <div key={index}>
          <h3>{p.name}</h3>
          <p>{p.description}</p>
        </div>
      ))}
    </div>
  )
}

export default App
