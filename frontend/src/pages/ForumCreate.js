import React, { useState } from 'react';
import { Container, Typography, TextField, Button, Paper, Alert, Box, Autocomplete, CircularProgress } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { createForum } from '../api/forumApi';
import { useSelector } from 'react-redux';
import { searchGames } from '../api/gamesApi';
import { jwtDecode } from 'jwt-decode';

const ForumCreate = () => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [gameQuery, setGameQuery] = useState('');
  const [gameOptions, setGameOptions] = useState([]);
  const [selectedGame, setSelectedGame] = useState(null);
  const [loadingGames, setLoadingGames] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const navigate = useNavigate();
  const { isAuthenticated, token } = useSelector(state => state.auth);

  const handleGameInputChange = async (event, value) => {
    setGameQuery(value);
    if (value.length < 2) {
      setGameOptions([]);
      return;
    }
    setLoadingGames(true);
    try {
      const games = await searchGames(value);
      setGameOptions(games);
    } catch {
      setGameOptions([]);
    } finally {
      setLoadingGames(false);
    }
  };

  if (!isAuthenticated) {
    return (
      <Container maxWidth="sm" sx={{ py: 4 }}>
        <Paper sx={{ p: 3 }}>
          <Alert severity="warning" sx={{ mb: 2 }}>
            Только авторизованные пользователи могут создавать обсуждения.
          </Alert>
          <Button variant="contained" onClick={() => navigate('/login')}>Войти</Button>
        </Paper>
      </Container>
    );
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const decoded = jwtDecode(token);
      const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      
      await createForum({ 
        Title: title, 
        Description: description, 
        GameId: selectedGame ? selectedGame.id : '',
        AuthorId: userId
      });
      setSuccess(true);
      setTimeout(() => navigate('/forum'), 1200);
    } catch {
      setError('Не удалось создать обсуждение');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Paper sx={{ p: 3 }}>
        <Typography variant="h5" gutterBottom>Создать обсуждение</Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            label="Название обсуждения"
            fullWidth
            required
            value={title}
            onChange={e => setTitle(e.target.value)}
            sx={{ mb: 2 }}
          />
          <TextField
            label="Текст обсуждения"
            fullWidth
            required
            multiline
            minRows={3}
            value={description}
            onChange={e => setDescription(e.target.value)}
            sx={{ mb: 2 }}
          />
          <Autocomplete
            freeSolo
            filterOptions={x => x}
            options={gameOptions}
            getOptionLabel={option => option.name || ''}
            value={selectedGame}
            onChange={(e, value) => setSelectedGame(value)}
            onInputChange={handleGameInputChange}
            loading={loadingGames}
            renderInput={(params) => (
              <TextField
                {...params}
                label="Игра (поиск по названию)"
                placeholder="Начните вводить название..."
                sx={{ mb: 2 }}
                InputProps={{
                  ...params.InputProps,
                  endAdornment: (
                    <>
                      {loadingGames ? <CircularProgress color="inherit" size={20} /> : null}
                      {params.InputProps.endAdornment}
                    </>
                  ),
                }}
              />
            )}
            isOptionEqualToValue={(option, value) => option.id === value.id}
          />
          <Box sx={{ display: 'flex', gap: 2 }}>
            <Button type="submit" variant="contained" disabled={loading || !title.trim() || !description.trim()}>
              {loading ? 'Создание...' : 'Создать'}
            </Button>
            <Button variant="outlined" onClick={() => navigate('/forum')} disabled={loading}>
              Отмена
            </Button>
          </Box>
        </form>
        {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        {success && <Alert severity="success" sx={{ mt: 2 }}>Обсуждение создано!</Alert>}
      </Paper>
    </Container>
  );
};

export default ForumCreate; 