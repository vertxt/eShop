import { Drawer as MuiDrawer, List, Divider, IconButton } from '@mui/material';
import { styled, Theme, CSSObject, useTheme } from '@mui/material/styles';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';
import React, { useEffect } from 'react';
import DrawerHeader from './DrawerHeader';
import SidebarItem from './SidebarItem';
import { SIDEBAR_ITEMS } from '../config/sidebarConfig';
import SidebarGroup from './SidebarGroup';
import { NavigationItem } from '../types/navigation';

interface SidebarProps {
  open: boolean;
  handleDrawerClose: () => void;
  drawerWidth: number;
};

const openedMixin = (theme: Theme, width: number): CSSObject => ({
  width,
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: 'hidden',
});

const closedMixin = (theme: Theme): CSSObject => ({
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: 'hidden',
  width: `calc(${theme.spacing(7)} + 1px)`,
  [theme.breakpoints.up('sm')]: {
    width: `calc(${theme.spacing(8)} + 1px)`,
  },
});

const Drawer = styled(MuiDrawer, { shouldForwardProp: (prop) => prop !== 'open' && prop !== 'drawerWidth' })<{
  open?: boolean;
  drawerWidth: number;
}>(({ theme, open, drawerWidth }) => ({
  width: drawerWidth,
  flexShrink: 0,
  whiteSpace: 'nowrap',
  boxSizing: 'border-box',
  ...(open
    ? {
      ...openedMixin(theme, drawerWidth),
      '& .MuiDrawer-paper': openedMixin(theme, drawerWidth),
    }
    : {
      ...closedMixin(theme),
      '& .MuiDrawer-paper': closedMixin(theme),
    }),
}));

export default function Sidebar({
  open,
  handleDrawerClose,
  drawerWidth,
}: SidebarProps) {
  const theme = useTheme();
  const [openItems, setOpenItems] = React.useState<{ [key: string]: boolean }>({});

  // Initialize open state for items that have child routes
  useEffect(() => {
    const initialOpenState: { [key: string]: boolean } = {};
    SIDEBAR_ITEMS.forEach((item: NavigationItem) => {
      if (item.items && item.items.length > 0) {
        initialOpenState[item.id] = false;
      }
    });
    setOpenItems(initialOpenState);
  }, []);

  const toggle = (id: string) => {
    setOpenItems((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  return (
    <Drawer variant="permanent" open={open} drawerWidth={drawerWidth}>
      <DrawerHeader>
        <IconButton onClick={handleDrawerClose}>
          {theme.direction === 'rtl' ? <ChevronRight /> : <ChevronLeft />}
        </IconButton>
      </DrawerHeader>
      <Divider />
      <List>
        {SIDEBAR_ITEMS.map((item: NavigationItem) =>
          item.items ? (
            <SidebarGroup
              key={item.id}
              item={item}
              open={open}
              isExpanded={openItems[item.id] || false}
              toggle={() => toggle(item.id)}
            />
          ) : (
            <SidebarItem key={item.id} item={item} open={open} />
          )
        )}
      </List>
    </Drawer>
  );
}
