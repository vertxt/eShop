import { ListItem, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import { NavigationItem } from '../types/navigation';

export default function SidebarItem({ item, open }: { item: NavigationItem; open: boolean }) {
  const navigate = useNavigate();
  const location = useLocation();
  const isSelected = location.pathname === item.path;
  
  return (
    <ListItem disablePadding sx={{ display: 'block' }}>
      <ListItemButton
        onClick={() => navigate(item.path || '/')}
        sx={{
          minHeight: 48,
          justifyContent: open ? 'initial' : 'center',
          px: 2.5,
          backgroundColor: isSelected ? 'rgba(0,0,0,0.08)' : 'inherit',
          '&:hover': {
            backgroundColor: 'rgba(0,0,0,0.04)',
          },
          transition: 'background-color 0.2s',
        }}
      >
        <ListItemIcon sx={{ minWidth: 0, mr: open ? 3 : 'auto', justifyContent: 'center' }}>
          {item.icon}
        </ListItemIcon>
        <ListItemText primary={item.title} sx={{ opacity: open ? 1 : 0 }} />
      </ListItemButton>
    </ListItem>
  );
}
