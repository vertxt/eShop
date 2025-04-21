import {
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Collapse,
  List,
} from "@mui/material";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import { useLocation, useNavigate } from "react-router-dom";
import { NavigationItem } from "../types/navigation";

interface SidebarGroupProps {
  item: NavigationItem;
  open: boolean;
  isExpanded: boolean;
  toggle: () => void;
}

export default function SidebarGroup({
  item,
  open,
  isExpanded,
  toggle,
}: SidebarGroupProps) {
  const navigate = useNavigate();
  const location = useLocation();

  const handleClick = () => {
    if (item.path) {
      navigate(item.path);
    } else {
      toggle();
    }
  };

  // Check if any child item is active
  const hasActiveChild = item.items?.some(
    (subItem) => subItem.path === location.pathname,
  );

  const displayIcon = open ? (
    item.icon
  ) : isExpanded ? (
    <ExpandLess />
  ) : (
    item.icon
  );

  return (
    <>
      <ListItem disablePadding sx={{ display: "block" }}>
        <ListItemButton
          onClick={handleClick}
          sx={{
            minHeight: 48,
            justifyContent: open ? "initial" : "center",
            px: 2.5,
            backgroundColor: hasActiveChild ? "rgba(0,0,0,0.04)" : "inherit",
          }}
        >
          <ListItemIcon
            sx={{
              minWidth: 0,
              mr: open ? 3 : "auto",
              justifyContent: "center",
            }}
          >
            {displayIcon}
          </ListItemIcon>
          <ListItemText primary={item.title} sx={{ opacity: open ? 1 : 0 }} />
          {open && (isExpanded ? <ExpandLess /> : <ExpandMore />)}
        </ListItemButton>
      </ListItem>
      <Collapse in={isExpanded} timeout="auto" unmountOnExit>
        <List component="div" disablePadding>
          {item.items?.map((sub) => (
            <ListItemButton
              key={sub.id}
              sx={{
                pl: open ? 4 : 2.5,
                minHeight: 48,
                justifyContent: open ? "initial" : "center",
                backgroundColor:
                  location.pathname === sub.path
                    ? "rgba(0,0,0,0.08)"
                    : "inherit",
              }}
              onClick={() => navigate(sub.path || '/')}
            >
              <ListItemIcon
                sx={{
                  minWidth: 0,
                  mr: open ? 3 : "auto",
                  justifyContent: "center",
                }}
              >
                {sub.icon}
              </ListItemIcon>
              <ListItemText
                primary={sub.title}
                sx={{ opacity: open ? 1 : 0 }}
              />
            </ListItemButton>
          ))}
        </List>
      </Collapse>
    </>
  );
}
