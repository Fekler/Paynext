import api from '../api';

export const usersService = {
  getAll: async () => {
    const response = await api.get('/api/v1/Users');
    return response.data.data || response.data;
  },
  getById: async (uuid: string) => {
    const response = await api.get(`/api/v1/Users/${uuid}`);
    return response.data.data || response.data;
  },
  create: async (data: any) => {
    const response = await api.post('/api/v1/Users', data);
    return response.data;
  },
  update: async (data: any) => {
    const response = await api.put('/api/v1/Users', data);
    return response.data;
  },
  delete: async (uuid: string) => {
    const response = await api.delete(`/api/v1/Users/${uuid}`);
    return response.data;
  },
}; 