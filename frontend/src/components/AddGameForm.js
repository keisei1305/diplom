import React, { useState, useEffect } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Box,
  Typography,
  Alert,
  CircularProgress
} from '@mui/material';
import { useDispatch, useSelector } from 'react-redux';
import { createGame, clearGameError } from '../store/gamesSlice';
import { fetchAllAuthors, createAuthor } from '../store/authorsSlice';

const AddGameForm = ({ open, onClose, onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: ''
  });
  const [errors, setErrors] = useState({});

  const dispatch = useDispatch();
  const { isLoading: gameLoading, error } = useSelector((state) => state.games);

  useEffect(() => {
    if (open) {
      setFormData({ name: '', description: '' });
      setErrors({});
      dispatch(clearGameError && clearGameError());
    }
  }, [open, dispatch]);

  const validateForm = () => {
    const newErrors = {};
    if (!formData.name.trim()) {
      newErrors.name = 'Название игры обязательно';
    }
    if (!formData.description.trim()) {
      newErrors.description = 'Описание игры обязательно';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateForm()) {
      return;
    }
    try {
      await dispatch(createGame({
        name: formData.name,
        description: formData.description,
        filterIds: []
      })).unwrap();
      setFormData({ name: '', description: '' });
      setErrors({});
      dispatch(clearGameError && clearGameError());
      onSuccess?.();
      onClose();
    } catch (error) {
      console.error('Error creating game:', error);
    }
  };

  const handleClose = () => {
    setFormData({ name: '', description: '' });
    setErrors({});
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Добавить новую игру</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <TextField
              label="Название игры"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              error={!!errors.name}
              helperText={errors.name}
              fullWidth
              required
            />
            <TextField
              label="Описание игры"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              error={!!errors.description}
              helperText={errors.description}
              multiline
              rows={4}
              fullWidth
              required
            />
            {gameLoading && <CircularProgress size={24} />}
            {error && (
              <Alert severity="error">
                {typeof error === 'string' && error.includes('Author not found for userId')
                  ? 'Вы пока не являетесь автором'
                  : error}
              </Alert>
            )}
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Отмена</Button>
          <Button type="submit" variant="contained" color="primary" disabled={gameLoading}>
            Создать
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default AddGameForm; 