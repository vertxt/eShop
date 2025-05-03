import { useState } from "react";
import { useAuth } from "react-oidc-context";
import { useAppDispatch, useAppSelector } from "../../app/store/store";
import { Avatar, Button, IconButton, ListItemIcon, Menu, MenuItem, Toolbar, Typography } from "@mui/material";
import GlobalLoadingIndicator from "../../features/loading/GlobalLoadingIndicator";
import { DarkMode, LightMode, AccountCircle, Logout, Login } from "@mui/icons-material";
import MenuIcon from "@mui/icons-material/Menu";
import { switchTheme } from "../../app/store/uiSlice";
import { AppBar } from "./AppBar";

type Props = {
    open: boolean,
    drawerWidth: number,
    onDrawerOpen: () => void,
}

export default function AppTopBar({ open, drawerWidth, onDrawerOpen }: Props) {
    const { user, isAuthenticated, signinRedirect, signoutRedirect } = useAuth();
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const theme = useAppSelector(state => state.ui.theme)
    const dispatch = useAppDispatch();

    const handleLogin = () => {
        signinRedirect();
    }
    const handleLogout = () => {
        signoutRedirect();
        setAnchorEl(null);
    }

    const openAccountMenu = (event: React.MouseEvent<HTMLElement>) => setAnchorEl(event.currentTarget);
    const closeAccountMenu = () => setAnchorEl(null);

    return (
        <AppBar position="fixed" open={open} drawerWidth={drawerWidth}>
            <GlobalLoadingIndicator />
            <Toolbar>
                <IconButton
                    color="inherit"
                    aria-label="open drawer"
                    onClick={onDrawerOpen}
                    edge="start"
                    sx={{
                        marginRight: 5,
                        ...(open && { display: "none" }),
                    }}
                >
                    <MenuIcon />
                </IconButton>

                <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
                    eShop
                </Typography>

                <IconButton onClick={() => dispatch(switchTheme())} sx={{ mr: 1 }}>
                    {theme === 'dark' ? <DarkMode /> : <LightMode />}
                </IconButton>

                {isAuthenticated ? (
                    <>
                        <IconButton
                            onClick={openAccountMenu}
                            color="inherit"
                            aria-label="account"
                            sx={{ ml: 1 }}
                        >
                            {user?.profile.name
                                ? (<Avatar>{user.profile.name[0]}</Avatar>)
                                : (<AccountCircle fontSize="medium" />)}
                        </IconButton>

                        <Menu
                            anchorEl={anchorEl}
                            open={Boolean(anchorEl)}
                            onClose={closeAccountMenu}
                            keepMounted
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'right',
                            }}
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'right',
                            }}
                        >
                            <MenuItem
                                key="logout"
                                onClick={handleLogout}
                            >
                                <ListItemIcon>
                                    <Logout fontSize="small" />
                                </ListItemIcon>
                                <Typography>Logout</Typography>
                            </MenuItem>
                        </Menu>
                    </>
                ) : (
                    <Button
                        color="inherit"
                        startIcon={<Login />}
                        size="medium"
                        onClick={handleLogin}
                        sx={{ ml: 1 }}
                    >
                        Login
                    </Button>
                )}
            </Toolbar>
        </AppBar>
    )
}