import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, Link } from 'react-router-dom';
import { registerUser } from '../store/authSlice';
import { Container, TextField, Button, Typography, Box, Alert } from '@mui/material';

const Register = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { status, error } = useSelector((state) => state.auth);
  const [registrationSuccess, setRegistrationSuccess] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setRegistrationSuccess(false);
    dispatch(registerUser({ 
      username: username.trim(), 
      email: email.trim(), 
      password: password.trim() 
    }));
  };
  
  useEffect(() => {
    if (status === 'succeeded' && !error && !registrationSuccess) {
        setRegistrationSuccess(true);
        setTimeout(() => {
            navigate('/');
        }, 2000);
    }
  }, [status, error, navigate, registrationSuccess]);

  return (
    <Container maxWidth="xs">
      <Box sx={{ mt: 8, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        <Typography component="h1" variant="h5">
          Регистрация
        </Typography>
        {registrationSuccess ? (
            <Alert severity="success" sx={{width: '100%', mt: 2}}>
                Регистрация прошла успешно! Вы будете перенаправлены на главную страницу.
            </Alert>
        ) : (
            <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
            <TextField
                margin="normal"
                required
                fullWidth
                id="username"
                label="Имя пользователя"
                name="username"
                autoComplete="username"
                autoFocus
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <TextField
                margin="normal"
                required
                fullWidth
                id="email"
                label="Email"
                name="email"
                autoComplete="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Пароль"
                type="password"
                id="password"
                autoComplete="new-password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            {status === 'loading' && <Typography>Загрузка...</Typography>}
            {error && <Alert severity="error">{error.message || 'Ошибка регистрации'}</Alert>}
            <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
                disabled={status === 'loading'}
            >
                Зарегистрироваться
            </Button>
            <Link to="/login">
                Уже есть аккаунт? Войти
            </Link>
            </Box>
        )}
      </Box>
    </Container>
  );
};

export default Register;
 