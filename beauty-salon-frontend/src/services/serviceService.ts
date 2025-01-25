import axios from 'axios';

export const getServices = async () => {
  return await axios.get('/api/services');
};

export const getServiceById = async (id: string) => {
  return await axios.get(`/api/services/${id}`);
};

export const createService = async (serviceData: any) => {
  return await axios.post('/api/services', serviceData);
};

export const updateService = async (id: string, serviceData: any) => {
  return await axios.put(`/api/services/${id}`, serviceData);
};
