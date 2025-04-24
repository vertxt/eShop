import { Outlet } from 'react-router-dom';
import MainLayout from '../shared/layouts/MainLayout';
import { createTheme, CssBaseline, ThemeProvider } from '@mui/material';
import { getDarkPalette, getLightPalette } from '../shared/config/themes/palette';
import { ToastContainer } from 'react-toastify';
import { useAppSelector } from './store/store';

function App() {
  const mode = useAppSelector(state => state.ui.theme);
  const darkTheme = createTheme({
    palette: {
      ...(mode === 'dark' ? getDarkPalette() : getLightPalette())
    }
  })

  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <MainLayout>
        <Outlet />
      </MainLayout>
      <ToastContainer
        position='bottom-right'
        theme='colored'
      />
    </ThemeProvider>
  );
}

export default App;
