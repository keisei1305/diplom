import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import * as gamesApi from '../api/gamesApi';

export const fetchAllGames = createAsyncThunk('games/fetchAll', async (_, { rejectWithValue }) => {
  try {
    const data = await gamesApi.getAllGames();
    return data;
  } catch (error) {
    return rejectWithValue(error.response?.data || 'Failed to fetch games');
  }
});

export const searchGames = createAsyncThunk('games/search', async (searchTerm, { rejectWithValue }) => {
  try {
    const data = await gamesApi.searchGames(searchTerm);
    return data;
  } catch (error) {
    return rejectWithValue(error.response?.data || 'Failed to search games');
  }
});

export const createGame = createAsyncThunk('games/create', async (gameData, { rejectWithValue }) => {
  try {
    const data = await gamesApi.createGame(gameData);
    return data;
  } catch (error) {
    return rejectWithValue(error.response?.data || 'Failed to create game');
  }
});

const gamesSlice = createSlice({
  name: 'games',
  initialState: {
    games: [],
    searchResults: [],
    isLoading: false,
    error: null,
    searchTerm: '',
  },
  reducers: {
    clearSearch: (state) => {
      state.searchResults = [];
      state.searchTerm = '';
    },
    setSearchTerm: (state, action) => {
      state.searchTerm = action.payload;
    },
    clearGameError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllGames.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchAllGames.fulfilled, (state, action) => {
        state.isLoading = false;
        state.games = action.payload;
      })
      .addCase(fetchAllGames.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload;
      })
      .addCase(searchGames.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(searchGames.fulfilled, (state, action) => {
        state.isLoading = false;
        state.searchResults = action.payload;
      })
      .addCase(searchGames.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload;
      })
      .addCase(createGame.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(createGame.fulfilled, (state, action) => {
        state.isLoading = false;
        state.games.push(action.payload);
      })
      .addCase(createGame.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload;
      });
  },
});

export const { clearSearch, setSearchTerm, clearGameError } = gamesSlice.actions;
export default gamesSlice.reducer; 