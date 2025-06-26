import React, { useState } from 'react';
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  CircularProgress,
  Alert,
  Box,
  LinearProgress, 
  Typography
} from '@mui/material';
import { uploadGameFile } from '../api/fileApi';

const UploadGameFileDialog = ({ open, onClose, onSuccess, gameId, gameFileId }) => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [uploadProgress, setUploadProgress] = useState(0);

  const handleFileChange = (event) => {
    setSelectedFile(event.target.files[0]);
    setError('');
    setSuccess('');
    setUploadProgress(0);
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      setError('Пожалуйста, выберите файл.');
      return;
    }

    setIsLoading(true);
    setError('');
    setSuccess('');
    setUploadProgress(0);

    try {
      const onUploadProgress = (progressEvent) => {
        const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
        setUploadProgress(percentCompleted);
      };

      if (gameFileId) {
        await uploadGameFile(gameId, selectedFile, onUploadProgress);
      } else {
        await uploadGameFile(gameId, selectedFile, onUploadProgress);
      }
      setSuccess('Файл успешно загружен!');
      setTimeout(() => {
        onSuccess();
        handleClose();
      }, 1500);
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Произошла ошибка при загрузке файла.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    setSelectedFile(null);
    setError('');
    setSuccess('');
    setIsLoading(false);
    setUploadProgress(0);
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
      <DialogTitle>Загрузить файл игры</DialogTitle>
      <DialogContent>
        <Box sx={{ my: 2 }}>
          <TextField
            type="file"
            fullWidth
            onChange={handleFileChange}
            variant="outlined"
            helperText={selectedFile ? `Выбран файл: ${selectedFile.name}` : "Выберите файл для загрузки"}
            disabled={isLoading}
          />
        </Box>
        {isLoading && (
          <Box sx={{ width: '100%', my: 2 }}>
            <LinearProgress variant="determinate" value={uploadProgress} />
            <Typography variant="body2" color="text.secondary" align="center" sx={{ mt: 1 }}>
              {`Загрузка... ${uploadProgress}%`}
            </Typography>
          </Box>
        )}
        {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        {success && <Alert severity="success" sx={{ mt: 2 }}>{success}</Alert>}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isLoading}>Отмена</Button>
        <Button 
          onClick={handleUpload} 
          variant="contained" 
          disabled={!selectedFile || isLoading}
        >
          Загрузить
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default UploadGameFileDialog; 