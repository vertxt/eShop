import { ReactNode, useState } from "react";
import {
  Box,
  Toolbar,
  IconButton,
  Typography,
} from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import { AppBar } from "../components/AppBar";
import Sidebar from "../components/Sidebar";
import DrawerHeader from "../components/DrawerHeader";
import { switchTheme } from "../../app/store/uiSlice";
import { DarkMode, LightMode } from "@mui/icons-material";
import { useAppDispatch, useAppSelector } from "../../app/store/store";

const drawerWidth = 240;

interface MainLayoutProps {
  children?: ReactNode;
}

export default function MainLayout({ children }: MainLayoutProps) {
  const [open, setOpen] = useState(false);
  const theme = useAppSelector(state => state.ui.theme)
  const dispatch = useAppDispatch();

  const handleDrawerOpen = () => {
    setOpen(true);
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };

  return (
    <Box sx={{ display: "flex", minHeight: '100vh' }}>
      <AppBar position="fixed" open={open} drawerWidth={drawerWidth}>
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={handleDrawerOpen}
            edge="start"
            sx={{
              marginRight: 5,
              ...(open && { display: "none" }),
            }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component="div">
            eShop
          </Typography>
          <IconButton onClick={() => dispatch(switchTheme())}>
            {theme === 'dark' ? <DarkMode /> : <LightMode />}
          </IconButton>
        </Toolbar>
      </AppBar>

      <Sidebar
        open={open}
        handleDrawerClose={handleDrawerClose}
        drawerWidth={drawerWidth}
      />

      <Box component="main" sx={{
        flexGrow: 1,
        p: 3,
        width: '100%',
        backgroundColor: 'background.default',
        color: 'text.primary',
        display: 'flex',
        flexDirection: 'column'
      }}>
        <DrawerHeader />
        {children}
      </Box>
    </Box>
  );
}
