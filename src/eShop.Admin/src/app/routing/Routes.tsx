import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import ProductListView from "../../features/products/ProductListView";
import CategoryListView from "../../features/categories/CategoryListView";
import UserListView from "../../features/users/UserListView";
import Dashboard from "../../features/dashboard/Dashboard";
import OrderListView from "../../features/orders/OrderListView";
import ReviewListView from "../../features/reviews/ReviewListView";
import ErrorsControl from "../../features/errors/ErrorsControl";
import ServerError from "../../features/errors/ServerError";
import NotFound from "../../features/errors/NotFound";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {
                path: 'dashboard',
                element: <Dashboard />
            },
            {
                path: 'products',
                element: <ProductListView />
            },
            {
                path: 'categories',
                element: <CategoryListView />
            },
            {
                path: 'users',
                element: <UserListView />
            },
            {
                path: 'orders',
                element: <OrderListView />
            },
            {
                path: 'reviews',
                element: <ReviewListView />
            },
            {
                path: 'errors',
                element: <ErrorsControl />
            },
            {
                path: 'errors/server',
                element: <ServerError />
            },
            {
                path: 'errors/notfound',
                element: <NotFound />
            },
        ]
    }
])