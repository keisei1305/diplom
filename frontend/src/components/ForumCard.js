import React from 'react';
import { Card, CardContent, Typography, Box } from '@mui/material';

const ForumCard = ({ forum }) => {
  return (
    <Card sx={{ 
      mb: 2, 
      transition: 'box-shadow 0.2s, opacity 0.2s', 
      '&:hover': { 
        boxShadow: 4, 
        opacity: 0.8 
      }, 
      cursor: 'pointer' 
    }}>
      <CardContent>
        <Typography variant="h6" sx={{ fontWeight: 'bold' }} gutterBottom>
          {forum.title}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
          Автор: {forum.author?.nickname || 'Неизвестно'}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
          Комментариев: {forum.commentsCount || 0}
        </Typography>
        {forum.lastComment ? (
          <Box sx={{ mt: 1 }}>
            <Typography variant="subtitle2" color="text.secondary">
              Последний комментарий:
            </Typography>
            <Typography variant="body2">
              {forum.lastComment.content}
            </Typography>
            <Typography variant="caption" color="text.secondary">
              — {forum.lastComment.author?.nickname || 'Неизвестно'}
            </Typography>
          </Box>
        ) : (
          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            Нет комментариев
          </Typography>
        )}
      </CardContent>
    </Card>
  );
};

export default ForumCard; 