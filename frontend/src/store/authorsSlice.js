import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import * as authorsApi from '../api/authorsApi';

export const fetchAllAuthors = createAsyncThunk('authors/fetchAll', async (_, { rejectWithValue }) => {
  try {
    const data = await authorsApi.getAllAuthors();
    return data;
  } catch (error) {
    return rejectWithValue(error.response?.data || 'Failed to fetch authors');
  }
});

export const createAuthor = createAsyncThunk('authors/create', async (authorData, { rejectWithValue }) => {
  try {
    const data = await authorsApi.createAuthor(authorData);
    return data;
  } catch (error) {
    return rejectWithValue(error.response?.data || 'Failed to create author');
  }
});

const authorsSlice = createSlice({
  name: 'authors',
  initialState: {
    authors: [],
    isLoading: false,
    error: null,
  },
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllAuthors.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchAllAuthors.fulfilled, (state, action) => {
        state.isLoading = false;
        state.authors = action.payload;
      })
      .addCase(fetchAllAuthors.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload;
      })
      .addCase(createAuthor.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(createAuthor.fulfilled, (state, action) => {
        state.isLoading = false;
        state.authors.push(action.payload);
      })
      .addCase(createAuthor.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload;
      });
  },
});

export const { clearError } = authorsSlice.actions;
export default authorsSlice.reducer; 