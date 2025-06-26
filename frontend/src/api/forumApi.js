import api from './index';

export const getAllForums = async () => {
  const response = await api.get('/api/forums');
  return response.data;
};

export const getForumById = async (id) => {
  const response = await api.get(`/api/forums/${id}`);
  return response.data;
};

export const getForumComments = async (forumId) => {
  const response = await api.get(`/api/forums/${forumId}/comments`);
  return response.data;
};

export const createForum = async (data) => {
  const response = await api.post('/api/forums', data);
  return response.data;
};

export const createForumComment = async (forumId, data) => {
  const response = await api.post(`/api/forums/${forumId}/comments`, data);
  return response.data;
}; 