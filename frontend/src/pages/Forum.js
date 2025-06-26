import React, { useEffect, useState } from 'react';
import { Container, Typography, CircularProgress, Alert, Box, Button } from '@mui/material';
import ForumCard from '../components/ForumCard';
import { getAllForums } from '../api/forumApi';
import { useNavigate } from 'react-router-dom';

const Forum = () => {
  const [forums, setForums] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    getAllForums()
      .then(setForums)
      .catch(() => setError('Не удалось загрузить обсуждения'))
      .finally(() => setLoading(false));
  }, []);

  const handleCreate = () => {
    navigate('/forum/create');
  };

  const handleCardClick = (id) => {
    navigate(`/forum/${id}`);
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" gutterBottom>Форум</Typography>
        <Button variant="contained" onClick={handleCreate}>Создать обсуждение</Button>
      </Box>
      {loading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <CircularProgress />
        </Box>
      ) : error ? (
        <Alert severity="error">{error}</Alert>
      ) : forums.length === 0 ? (
        <Alert severity="info">Нет обсуждений</Alert>
      ) : (
        forums.map((forum) => (
          <Box key={forum.id} onClick={() => handleCardClick(forum.id)} sx={{ cursor: 'pointer' }}>
            <ForumCard forum={forum} />
          </Box>
        ))
      )}
    </Container>
  );
};

export default Forum;
 