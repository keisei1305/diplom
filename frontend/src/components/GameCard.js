import React, { useState } from 'react';
import { Card, CardContent, Typography, Box, Chip, Button, CircularProgress } from '@mui/material';
import DownloadIcon from '@mui/icons-material/Download';
import { downloadGameFile } from '../api/fileApi';

const GameCard = ({ game }) => {
  const [isDownloading, setIsDownloading] = useState(false);

  const handleDownload = async () => {
    setIsDownloading(true);
    try {
      const { blob, fileName } = await downloadGameFile(game.id);
      
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', fileName);
      
      document.body.appendChild(link);
      link.click();
      
      link.parentNode.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Download failed:', error);
    } finally {
      setIsDownloading(false);
    }
  };

  return (
    <Card sx={{ 
      display: 'flex', 
      width: '100%', 
      mb: 2,
      transition: 'box-shadow 0.3s',
      '&:hover': {
        boxShadow: 3,
      }
    }}>
      <CardContent sx={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center', 
        width: '100%',
        p: 2,
        '&:last-child': {
          pb: 2,
        }
      }}>
        <Box sx={{ flex: 1, pr: 2 }}>
          <Typography variant="h6" component="div" sx={{ fontWeight: 'bold' }}>
            {game.name}
          </Typography>
          
          {game.author && (
            <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
              Автор: {game.author.name}
            </Typography>
          )}
          
          {game.description && (
            <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
              {game.description}
            </Typography>
          )}

          {game.filters && game.filters.length > 0 && (
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5, mt: 1 }}>
              {game.filters.map((filter) => (
                <Chip 
                  key={filter.id} 
                  label={filter.name} 
                  size="small"
                />
              ))}
            </Box>
          )}
        </Box>
        
        {game.gameFileId && (
          <Button
            variant="contained"
            startIcon={isDownloading ? <CircularProgress size={20} /> : <DownloadIcon />}
            onClick={handleDownload}
            disabled={isDownloading}
          >
            {isDownloading ? 'Скачивание...' : 'Скачать'}
          </Button>
        )}
      </CardContent>
    </Card>
  );
};

export default GameCard; 