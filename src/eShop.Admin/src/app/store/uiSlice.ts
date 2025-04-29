import { PaletteMode } from "@mui/material";
import { createSlice } from "@reduxjs/toolkit";

interface UIState {
  theme: PaletteMode,
  loading: boolean,
}

const storedTheme = localStorage.getItem("theme") as PaletteMode | null;

const initialState = {
  theme: storedTheme ?? 'light',
  loading: false
} satisfies UIState as UIState;

const uiSlice = createSlice({
  name: 'ui',
  initialState,
  reducers: {
    switchTheme: state => {
      const updatedTheme = (state.theme === 'dark') ? 'light' : 'dark';
      localStorage.setItem("theme", updatedTheme);
      state.theme = updatedTheme;
    },

    startLoading: state => { state.loading = true },
    stopLoading: state => { state.loading = false },
  }
})

export const { switchTheme, startLoading, stopLoading } = uiSlice.actions;
export const uiReducer = uiSlice.reducer;
