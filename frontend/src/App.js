import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { checkAuth } from './store/authSlice';
import Header from './components/Header';
import Login from './pages/Login';
import Register from './pages/Register';
import Forum from './pages/Forum';
import Games from './pages/Games';
import Profile from './pages/Profile';
import Admin from './pages/Admin';
import PrivateRoute from './routes/PrivateRoute';
import AdminRoute from './routes/AdminRoute';
import ForumDiscussion from './pages/ForumDiscussion';
import ForumCreate from './pages/ForumCreate';

function App() {
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(checkAuth());
  }, [dispatch]);

  return (
    <Router>
      <Header />
      <Routes>
        <Route path="/" element={<Games />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/forum" element={<Forum />} />
        <Route path="/games" element={<Games />} />
        <Route 
          path="/profile" 
          element={
            <PrivateRoute>
              <Profile />
            </PrivateRoute>
          } 
        />
        <Route 
          path="/admin" 
          element={
            <AdminRoute>
              <Admin />
            </AdminRoute>
          } 
        />
        <Route path="/forum/:id" element={<ForumDiscussion />} />
        <Route path="/forum/create" element={<ForumCreate />} />
      </Routes>
    </Router>
  );
}

export default App;
