import { PaletteOptions } from '@mui/material/styles';

export const getLightPalette = (): PaletteOptions => ({
  mode: 'light',
  primary: {
    main: '#1a73e8',
    light: '#4791db',
    dark: '#115293',
    contrastText: '#fff',
  },
  secondary: {
    main: '#ff6f00',
    light: '#ffa040',
    dark: '#c43e00',
    contrastText: '#fff',
  },
  background: {
    default: '#f8f9fa',
    paper: '#ffffff',
  },
  text: {
    primary: 'rgba(0, 0, 0, 0.87)',
    secondary: 'rgba(0, 0, 0, 0.6)',
    disabled: 'rgba(0, 0, 0, 0.38)',
  },
  success: { main: '#34a853', light: '#4caf50', dark: '#2e7d32' },
  error: { main: '#ea4335', light: '#ef5350', dark: '#c62828' },
  warning: { main: '#fbbc04', light: '#ffd54f', dark: '#f9a825' },
  info: { main: '#4285f4', light: '#64b5f6', dark: '#1976d2' },
  divider: 'rgba(0, 0, 0, 0.12)',
});

export const getDarkPalette = (): PaletteOptions => ({
  mode: 'dark',
  primary: {
    main: '#90caf9',
    light: '#e3f2fd',
    dark: '#42a5f5',
    contrastText: '#000'
  },
  secondary: {
    main: '#ffb74d',
    light: '#ffe9d4',
    dark: '#f57c00',
    contrastText: '#000'
  },
  background: {
    default: '#121212',
    paper: '#1e1e1e'
  },
  text: {
    primary: 'rgba(255, 255, 255, 0.87)',
    secondary: 'rgba(255, 255, 255, 0.6)',
    disabled: 'rgba(255, 255, 255, 0.38)',
  },
  success: { main: '#66bb6a', light: '#81c784', dark: '#388e3c' },
  error: { main: '#f44336', light: '#e57373', dark: '#d32f2f' },
  warning: { main: '#ffa726', light: '#ffb74d', dark: '#f57c00' },
  info: { main: '#29b6f6', light: '#4fc3f7', dark: '#0288d1' },
  divider: 'rgba(255, 255, 255, 0.12)',
});