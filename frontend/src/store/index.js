import { configureStore } from '@reduxjs/toolkit';
import authReducer from './authSlice';
import gamesReducer from './gamesSlice';
import authorsReducer from './authorsSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    games: gamesReducer,
    authors: authorsReducer,
  },
}); 