import api from '../api';

export const contractsService = {
  getAll: async () => {
    const response = await api.get('/api/v1/Contracts');
    return response.data.data || response.data;
  },
  getById: async (uuid: string) => {
    const response = await api.get(`/api/v1/Contracts/${uuid}`);
    return response.data.data || response.data;
  },
  create: async (data: any) => {
    const response = await api.post('/api/v1/Contracts', data);
    return response.data;
  },
  update: async (data: any) => {
    const response = await api.put('/api/v1/Contracts', data);
    return response.data;
  },
  delete: async (uuid: string) => {
    const response = await api.delete(`/api/v1/Contracts/${uuid}`);
    return response.data;
  },
}; 