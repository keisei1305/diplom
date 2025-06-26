import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Container, Typography, CircularProgress, Alert, Box, TextField, Button, Paper } from '@mui/material';
import { getForumById, getForumComments, createForumComment } from '../api/forumApi';
import { useSelector } from 'react-redux';
import { jwtDecode } from 'jwt-decode';

const ForumDiscussion = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [forum, setForum] = useState(null);
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [commentText, setCommentText] = useState('');
  const [posting, setPosting] = useState(false);
  const { isAuthenticated, token } = useSelector(state => state.auth);

  useEffect(() => {
    setLoading(true);
    Promise.all([
      getForumById(id),
      getForumComments(id)
    ])
      .then(([forumData, commentsData]) => {
        setForum(forumData);
        setComments(commentsData);
      })
      .catch(() => setError('Не удалось загрузить обсуждение'))
      .finally(() => setLoading(false));
  }, [id]);

  const handleAddComment = async () => {
    setPosting(true);
    try {
      const decoded = jwtDecode(token);
      const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      
      await createForumComment(id, {
        ForumId: Number(id),
        AuthorId: userId,
        Content: commentText
      });
      setCommentText('');
      const updatedComments = await getForumComments(id);
      setComments(updatedComments);
    } catch {
      setError('Не удалось добавить комментарий');
    } finally {
      setPosting(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <CircularProgress />
        </Box>
      ) : error ? (
        <Alert severity="error">{error}</Alert>
      ) : !forum ? (
        <Alert severity="info">Обсуждение не найдено</Alert>
      ) : (
        <>
          <Typography variant="h4" gutterBottom>{forum.title}</Typography>
          <Typography variant="subtitle1" color="text.secondary" gutterBottom>
            Автор: {forum.author?.nickname || 'Неизвестно'}
          </Typography>

          <Paper sx={{ p: 2, mb: 3 }}>
            <Typography variant="h6" gutterBottom>Добавить комментарий</Typography>
            {isAuthenticated ? (
              <>
                <TextField
                  fullWidth
                  multiline
                  minRows={2}
                  maxRows={6}
                  value={commentText}
                  onChange={e => setCommentText(e.target.value)}
                  placeholder="Ваш комментарий..."
                  sx={{ mb: 2 }}
                />
                <Button
                  variant="contained"
                  onClick={handleAddComment}
                  disabled={!commentText.trim() || posting}
                >
                  {posting ? 'Отправка...' : 'Отправить'}
                </Button>
              </>
            ) : (
              <Alert severity="warning" sx={{ mb: 0 }}>
                Только авторизованные пользователи могут оставлять комментарии.{' '}
                <Button color="primary" size="small" onClick={() => navigate('/login')}>Войти</Button>
              </Alert>
            )}
          </Paper>

          <Typography variant="h6" gutterBottom>Комментарии</Typography>
          {comments.length === 0 ? (
            <Alert severity="info">Комментариев пока нет</Alert>
          ) : (
            comments.map((c) => (
              <Paper key={c.id} sx={{ p: 2, mb: 2 }}>
                <Typography variant="body1" sx={{ mb: 1 }}>{c.content}</Typography>
                <Typography variant="caption" color="text.secondary">
                  {c.author?.nickname || 'Неизвестно'} • {new Date(c.createdAt).toLocaleString()}
                </Typography>
              </Paper>
            ))
          )}
        </>
      )}
    </Container>
  );
};

export default ForumDiscussion; 