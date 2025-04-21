import { Apps, Category, Dashboard, Inventory, People } from "@mui/icons-material";
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
    title: 'User',
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
        icon: <Inventory />,
        path: '/products',
      },
      {
        id: 'categories',
        title: 'Categories',
        icon: <Category />,
        path: '/categories',
      }
    ]
  }
]
