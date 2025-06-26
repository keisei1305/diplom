import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { logout } from '../store/authSlice';

const Header = () => {
  const { isAuthenticated, user } = useSelector((state) => state.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  console.log("Header - User:", user);
  console.log("Header - User roles:", user?.roles);
  console.log("Header - Is authenticated:", isAuthenticated);

  const isAdmin = user && Array.isArray(user.roles) && user.roles.includes('Admin');
  
  console.log("Header - Is admin:", isAdmin);

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          <Button color="inherit" component={Link} to="/" sx={{textTransform: 'none', fontSize: '1.25rem'}}>
            Мой проект
          </Button>
        </Typography>
        <Button color="inherit" component={Link} to="/forum">Форум</Button>
        <Button color="inherit" component={Link} to="/games">Каталог игр</Button>
        
        {isAuthenticated ? (
          <>
            {isAdmin && (
              <Button color="inherit" component={Link} to="/admin">Админка</Button>
            )}
            <Button color="inherit" onClick={() => navigate('/profile')}>Профиль</Button>
            <Button color="inherit" onClick={handleLogout}>Выход</Button>
          </>
        ) : (
          <>
            <Button color="inherit" component={Link} to="/login">Вход</Button>
            <Button color="inherit" component={Link} to="/register">Регистрация</Button>
          </>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default Header;
 