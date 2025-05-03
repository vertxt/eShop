import { ReactNode, useState } from "react";
import { Box } from "@mui/material";
import Sidebar from "../components/Sidebar";
import DrawerHeader from "../components/DrawerHeader";
import AppTopBar from "../components/AppTopBar";

const drawerWidth = 240;

interface MainLayoutProps {
  children?: ReactNode;
}

export default function MainLayout({ children }: MainLayoutProps) {
  const [open, setOpen] = useState(false);

  const handleDrawerOpen = () => {
    setOpen(true);
  };
  const handleDrawerClose = () => {
    setOpen(false);
  };

  return (
    <Box sx={{ display: "flex", minHeight: '100vh', minWidth: '100vw' }}>
      <AppTopBar
        open={open}
        drawerWidth={drawerWidth}
        onDrawerOpen={handleDrawerOpen}
      />

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
