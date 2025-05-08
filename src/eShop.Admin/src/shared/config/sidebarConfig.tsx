import { Apps, AttachMoney, Bookmark, Category, Dashboard, Error, People } from "@mui/icons-material";
import { NavigationItem } from "../types/navigation";

export const SIDEBAR_ITEMS: NavigationItem[] = [
  {
    id: 'dashboard',
    title: 'Dashboard',
    icon: <Dashboard />,
    path: '/dashboard',
  },
  {
    id: 'users',
    title: 'Users',
    icon: <People />,
    path: '/users',
  },
  {
    id: 'catalog',
    title: 'Catalog',
    icon: <Apps />,
    items: [
      {
        id: 'products',
        title: 'Products',
        icon: <Category />,
        path: '/products',
      },
      {
        id: 'categories',
        title: 'Categories',
        icon: <Bookmark />,
        path: '/categories',
      }
    ]
  },
  {
    id: 'orders',
    title: 'Orders',
    icon: <AttachMoney />,
    path: '/orders',
  },
  {
    id: 'errors',
    title: 'Errors',
    icon: <Error />,
    path: '/errors',
  }
]
