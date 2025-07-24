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
  advancedRequest: async (installmentId: string) => {
    const response = await api.post('/api/advanced-request', JSON.stringify(installmentId), {
      headers: { 'Content-Type': 'application/json' }
    });
    return response.data;
  },
  getAdvancedRequests: async (pagerNumber = 1, pageSize = 10) => {
    const response = await api.get('/api/advanced-request', {
      params: { pagerNumber, pageSize }
    });
    return response.data;
  },
  approveAdvancedRequests: async (actions: {InstallmentUuid: string, IsAccepted: boolean}[]) => {
    const response = await api.put('/api/advanced-request/approve', actions);
    return response.data;
  },
}; 