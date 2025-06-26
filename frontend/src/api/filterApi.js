import api from './index';

export const getAllFilters = async () => {
  const response = await api.get('/filter');
  return response.data;
};

export const getFilterById = async (id) => {
  const response = await api.get(`/filter/${id}`);
  return response.data;
};

export const createFilter = async (filterData) => {
  const response = await api.post('/filter', filterData);
  return response.data;
}; 