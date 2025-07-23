import axios from 'axios';
import { store } from './store';

const api = axios.create({
  baseURL: 'https://paynext-api.fekler.tec.br/',
});

// Interceptor para adicionar o token Bearer
api.interceptors.request.use((config) => {
  const state = store.getState() as any;
  const token = state.auth?.token;
  if (token) {
    config.headers = config.headers || {};
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
});

export default api; 