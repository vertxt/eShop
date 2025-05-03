import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import CategoryListView from "../../features/categories/CategoryListView";
import UserListView from "../../features/users/UserListView";
import Dashboard from "../../features/dashboard/Dashboard";
import OrderListView from "../../features/orders/OrderListView";
import ReviewListView from "../../features/reviews/ReviewListView";
import ErrorsControl from "../../features/errors/ErrorsControl";
import ServerError from "../../features/errors/ServerError";
import NotFound from "../../features/errors/NotFound";
import CategoryEditWrapper from "../../features/categories/CategoryEditWrapper";
import CategoryCreateWrapper from "../../features/categories/CategoryCreateWrapper";
import ProductPage from "../../features/products/ProductPage";
import AddProductPage from "../../features/products/AddProductPage";
import EditProductPage from "../../features/products/EditProductPage";
import PrivateRoute from "../../shared/components/PrivateRoute";
import LoginCallback from "../../features/auth/LoginCallback";
import LogoutCallback from "../../features/auth/LogoutCallback";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            {
                path: 'signin-callback',
                element: <LoginCallback />
            },
            {
                path: 'signout-callback',
                element: <LogoutCallback />
            },
            {
                element: <PrivateRoute />,
                children: [
                    {
                        path: 'dashboard',
                        element: <Dashboard />
                    },
                    {
                        path: 'products',
                        element: <ProductPage />
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
                    {
                        path: 'products/create',
                        element: <AddProductPage />
                    },
                    {
                        path: 'products/edit/:id',
                        element: <EditProductPage />
                    },
                    {
                        path: 'categories/create',
                        element: <CategoryCreateWrapper />
                    },
                    {
                        path: 'categories/edit/:id',
                        element: <CategoryEditWrapper />
                    },
                ]
            },
        ]
    }
])