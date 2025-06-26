import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Container, Typography, Paper, Box, Avatar, Button, TextField, Alert } from '@mui/material';
import MyGamesList from '../components/MyGamesList';
import AddGameForm from '../components/AddGameForm';
import { createAuthor } from '../api/authorsApi';

const Profile = () => {
  const { user } = useSelector((state) => state.auth);
  const [addGameDialogOpen, setAddGameDialogOpen] = useState(false);

  const [authorName, setAuthorName] = useState('');
  const [authorDescription, setAuthorDescription] = useState('');
  const [authorLoading, setAuthorLoading] = useState(false);
  const [authorSuccess, setAuthorSuccess] = useState(false);
  const [authorError, setAuthorError] = useState('');
  const [showAuthorForm, setShowAuthorForm] = useState(false);

  if (!user) {
    return (
      <Container>
        <Typography>Пользователь не найден. Пожалуйста, войдите в систему.</Typography>
      </Container>
    );
  }

  const handleAddGameSuccess = () => {
    setAddGameDialogOpen(false);
  };

  const handleBecomeAuthor = async (e) => {
    e.preventDefault();
    setAuthorLoading(true);
    setAuthorError('');
    setAuthorSuccess(false);
    try {
      await createAuthor({ name: authorName, description: authorDescription, userId: user.id });
      setAuthorSuccess(true);
      setAuthorName('');
      setAuthorDescription('');
      setShowAuthorForm(false);
    } catch (err) {
      setAuthorError(err.response?.data?.message || err.message || 'Ошибка при создании автора');
    } finally {
      setAuthorLoading(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
          <Avatar sx={{ width: 80, height: 80, mr: 3 }}>
            {user.username ? user.username.charAt(0).toUpperCase() : 'U'}
          </Avatar>
          <Box>
            <Typography variant="h4" component="h1">
              {user.username}
            </Typography>
            <Typography variant="body1" color="text.secondary">
              {user.email}
            </Typography>
            <Typography variant="caption" color="text.secondary">
              ID: {user.id}
            </Typography>
          </Box>
        </Box>
        <Button variant="contained" sx={{ mb: 3 }} onClick={() => setAddGameDialogOpen(true)}>
          Добавить игру
        </Button>
        {/* Кнопка и форма 'Стать автором' */}
        {!showAuthorForm && (
          <Button variant="outlined" sx={{ mb: 3 }} onClick={() => { setShowAuthorForm(true); setAuthorSuccess(false); setAuthorError(''); }}>
            Стать автором
          </Button>
        )}
        {showAuthorForm && (
          <Box component="form" onSubmit={handleBecomeAuthor} sx={{ mb: 3, p: 2, border: '1px solid #eee', borderRadius: 2 }}>
            <Typography variant="h6" gutterBottom>Стать автором</Typography>
            <TextField
              label="Имя автора"
              value={authorName}
              onChange={e => setAuthorName(e.target.value)}
              fullWidth
              required
              sx={{ mb: 2 }}
            />
            <TextField
              label="Описание автора"
              value={authorDescription}
              onChange={e => setAuthorDescription(e.target.value)}
              fullWidth
              required
              multiline
              minRows={2}
              sx={{ mb: 2 }}
            />
            <Button type="submit" variant="contained" disabled={authorLoading || !authorName.trim() || !authorDescription.trim()}>
              {authorLoading ? 'Отправка...' : 'Стать автором'}
            </Button>
            {authorError && <Alert severity="error" sx={{ mt: 2 }}>{authorError}</Alert>}
          </Box>
        )}
        {authorSuccess && <Alert severity="success" sx={{ mb: 3 }}>Вы стали автором!</Alert>}
        {/* Раздел с играми пользователя */}
        <MyGamesList />
        <AddGameForm
          open={addGameDialogOpen}
          onClose={() => setAddGameDialogOpen(false)}
          onSuccess={handleAddGameSuccess}
        />
      </Paper>
    </Container>
  );
};

export default Profile;
 