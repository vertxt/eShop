import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MainLayout from '../shared/layouts/MainLayout';
import ProductListView from '../features/products/ProductListView';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={
          <MainLayout>
            <ProductListView />
          </MainLayout>
        } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
