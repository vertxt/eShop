import { PaletteMode } from "@mui/material";
import { createSlice } from "@reduxjs/toolkit";

interface UIState {
  theme: PaletteMode
}

const initialState = {
  theme: 'light'
} satisfies UIState as UIState;

const uiSlice = createSlice({
  name: 'ui',
  initialState,
  reducers: {
    switchTheme: state => {
      const updatedTheme = (state.theme === 'dark') ? 'light' : 'dark';
      state.theme = updatedTheme;
    }
  }
})

export const { switchTheme } = uiSlice.actions;
export const uiReducer = uiSlice.reducer;
