import api from './index';

export const getAllAuthors = async () => {
  const response = await api.get('/authors');
  return response.data.Authors || response.data;
};

export const getAuthorById = async (id) => {
  const response = await api.get(`/authors/${id}`);
  return response.data;
};

export const createAuthor = async (authorData) => {
  const response = await api.post('/authors', authorData);
  return response.data;
}; 