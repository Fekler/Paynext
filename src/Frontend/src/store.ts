import { configureStore } from '@reduxjs/toolkit';
import authReducer from './authSlice';

// Aqui futuramente adicionaremos os reducers dos slices
export const store = configureStore({
  reducer: {
    auth: authReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch; 