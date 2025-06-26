import api from './index';

export const getAllGames = async (page = 1, pageSize = 10) => {
  const response = await api.get(`/games?page=${page}&pageSize=${pageSize}`);
  return response.data.Games || response.data;
};

export const searchGames = async (searchTerm) => {
  const response = await api.get(`/games/search?name=${encodeURIComponent(searchTerm)}`);
  return response.data;
};

export const getGameById = async (id) => {
  const response = await api.get(`/games/${id}`);
  return response.data;
};

export const createGame = async (gameData) => {
  const response = await api.post('/games', gameData);
  return response.data;
};

export const getGamesByAuthor = async (authorId) => {
  const response = await api.get(`/games/author/${authorId}`);
  return response.data;
};

export const deleteGame = async (id) => {
  await api.delete(`/games/${id}`);
};

export const updateGameFilters = async (gameId, filterIds) => {
  await api.put(`/games/${gameId}/filters`, filterIds);
};

export const getMyGames = async () => {
  const response = await api.get('/games/mine');
  return response.data;
}; 