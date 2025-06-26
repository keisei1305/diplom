import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { jwtDecode } from 'jwt-decode';
import * as authApi from '../api/authApi';

const getUserFromToken = (token) => {
  if (!token) return null;
  try {
    const decoded = jwtDecode(token);
    
    console.log("Decoded JWT object:", decoded);
    
    const currentTime = Date.now() / 1000;
    if (decoded.exp && decoded.exp < currentTime) {
      console.warn("Token has expired");
      localStorage.removeItem('token');
      return null;
    }
    
    console.log("Decoded JWT:", decoded);
    
    let roles = [];
    
    const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    if (decoded[roleClaim]) {
      roles = Array.isArray(decoded[roleClaim]) ? decoded[roleClaim] : [decoded[roleClaim]];
    }
    
    if (decoded.role) {
      roles = Array.isArray(decoded.role) ? decoded.role : [decoded.role];
    }
    
    if (decoded.roles) {
      roles = Array.isArray(decoded.roles) ? decoded.roles : [decoded.roles];
    }
    
    if (roles.length === 0) {
      Object.keys(decoded).forEach(key => {
        if (key.toLowerCase().includes('role')) {
          const value = decoded[key];
          if (Array.isArray(value)) {
            roles = roles.concat(value);
          } else if (value) {
            roles.push(value);
          }
        }
      });
    }
    
    console.log("Extracted roles:", roles);
    
    const userId = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || decoded.nameid || decoded.sub;
    const email = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || decoded.email;
    const username = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || decoded.name;

    if (!userId) {
      console.error("User ID could not be found in token. Checked for 'ClaimTypes.NameIdentifier', 'nameid', 'sub'.");
      return null;
    }

    return {
      id: userId,
      email,
      username,
      roles: roles
    };
  } catch (e) {
    console.error("Invalid token", e);
    localStorage.removeItem('token');
    return null;
  }
};

export const loginUser = createAsyncThunk('auth/login', async (credentials, { rejectWithValue }) => {
  try {
    const data = await authApi.login(credentials);
    localStorage.setItem('token', data.token);
    return data;
  } catch (error) {
    return rejectWithValue(error.response.data);
  }
});

export const registerUser = createAsyncThunk('auth/register', async (userData, { rejectWithValue }) => {
  try {
    const data = await authApi.register(userData);
    if (data.token) {
      localStorage.setItem('token', data.token);
    }
    return data;
  } catch (error) {
    return rejectWithValue(error.response.data);
  }
});

export const checkAuth = createAsyncThunk('auth/check', async (_, { getState }) => {
  const token = localStorage.getItem('token');
  if (!token) {
    return null;
  }
  
  const user = getUserFromToken(token);
  if (!user) {
    localStorage.removeItem('token');
    return null;
  }
  
  return { user, token };
});

const authSlice = createSlice({
  name: 'auth',
  initialState: {
    user: getUserFromToken(localStorage.getItem('token')),
    token: localStorage.getItem('token') || null,
    isAuthenticated: !!localStorage.getItem('token'),
    status: 'idle',
    error: null,
  },
  reducers: {
    logout: (state) => {
      state.user = null;
      state.token = null;
      state.isAuthenticated = false;
      state.status = 'idle';
      state.error = null;
      localStorage.removeItem('token');
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(loginUser.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.isAuthenticated = true;
        state.token = action.payload.token;
        state.user = getUserFromToken(action.payload.token);
      })
      .addCase(loginUser.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload;
      })
      .addCase(registerUser.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(registerUser.fulfilled, (state, action) => {
        state.status = 'succeeded';
        if (action.payload.token) {
          state.isAuthenticated = true;
          state.token = action.payload.token;
          state.user = getUserFromToken(action.payload.token);
        }
      })
      .addCase(registerUser.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload;
      })
      .addCase(checkAuth.fulfilled, (state, action) => {
        if (action.payload) {
          state.user = action.payload.user;
          state.token = action.payload.token;
          state.isAuthenticated = true;
          console.log("User successfully authenticated from token:", state.user);
        } else {
          state.user = null;
          state.token = null;
          state.isAuthenticated = false;
        }
      });
  },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer; 