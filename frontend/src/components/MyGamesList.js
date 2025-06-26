import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Box, Typography, Button, List, ListItem, ListItemText, CircularProgress, Alert } from '@mui/material';
import UploadIcon from '@mui/icons-material/Upload';
import UploadGameFileDialog from './UploadGameFileDialog';
import { getGamesByAuthor, deleteGame, getMyGames } from '../api/gamesApi';
import FilterDialog from './FilterDialog';

const MyGamesList = () => {
  const dispatch = useDispatch();
  const { user } = useSelector((state) => state.auth);
  
  const [myGames, setMyGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [selectedGame, setSelectedGame] = useState(null);
  const [uploadDialogOpen, setUploadDialogOpen] = useState(false);
  const [deletingId, setDeletingId] = useState(null);
  const [filterDialogOpen, setFilterDialogOpen] = useState(false);
  const [filterGame, setFilterGame] = useState(null);

  const fetchMyGames = async () => {
    setLoading(true);
    try {
      const games = await getMyGames();
      setMyGames(games);
    } catch (err) {
      setError('Не удалось загрузить список ваших игр.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMyGames();
  }, [user]);

  const handleOpenUploadDialog = (game) => {
    setSelectedGame(game);
    setUploadDialogOpen(true);
  };
  
  const handleCloseUploadDialog = () => {
    setUploadDialogOpen(false);
    setSelectedGame(null);
  };

  const handleUploadSuccess = () => {
    fetchMyGames();
    handleCloseUploadDialog();
  };

  const handleDeleteGame = async (gameId) => {
    if (!window.confirm('Вы уверены, что хотите удалить эту игру?')) return;
    setDeletingId(gameId);
    try {
      await deleteGame(gameId);
      fetchMyGames();
    } catch (err) {
      setError('Не удалось удалить игру.');
      console.error(err);
    } finally {
      setDeletingId(null);
    }
  };

  const handleOpenFilterDialog = (game) => {
    setFilterGame(game);
    setFilterDialogOpen(true);
  };
  
  const handleCloseFilterDialog = () => {
    setFilterDialogOpen(false);
    setFilterGame(null);
  };

  const handleFiltersUpdated = () => {
    fetchMyGames();
    handleCloseFilterDialog();
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error && myGames.length > 0) {
    return <Alert severity="error">{error}</Alert>;
  }

  return (
    <Box sx={{ mt: 4 }}>
      <Typography variant="h5" gutterBottom>
        Мои игры
      </Typography>
      {myGames.length === 0 ? (
        <Typography>Вы еще не создали ни одной игры.</Typography>
      ) : (
        <List>
          {myGames.map((game) => (
            <ListItem 
              key={game.id} 
              divider
              alignItems="flex-start"
            >
              <Box sx={{ display: 'flex', width: '100%', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <Box sx={{ flex: 1 }}>
                  <Typography variant="h6">{game.name}</Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ whiteSpace: 'pre-line' }}>
                    {game.description || 'Нет описания'}
                  </Typography>
                </Box>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1, ml: 2 }}>
                  <Button
                    variant="outlined"
                    color="error"
                    sx={{ mb: 1 }}
                    disabled={deletingId === game.id}
                    onClick={() => handleDeleteGame(game.id)}
                  >
                    {deletingId === game.id ? 'Удаление...' : 'Удалить'}
                  </Button>
                  <Button
                    variant="outlined"
                    startIcon={<UploadIcon />}
                    sx={{ mb: 1 }}
                    onClick={() => handleOpenUploadDialog(game)}
                  >
                    {game.gameFileId ? 'Заменить' : 'Загрузить'}
                  </Button>
                  <Button
                    variant="outlined"
                    color="primary"
                    onClick={() => handleOpenFilterDialog(game)}
                  >
                    Фильтры
                  </Button>
                </Box>
              </Box>
            </ListItem>
          ))}
        </List>
      )}

      {selectedGame && (
        <UploadGameFileDialog 
          open={uploadDialogOpen}
          onClose={handleCloseUploadDialog}
          onSuccess={handleUploadSuccess}
          gameId={selectedGame.id}
          gameFileId={selectedGame.gameFileId}
        />
      )}

      {filterGame && (
        <FilterDialog
          open={filterDialogOpen}
          onClose={handleCloseFilterDialog}
          onSuccess={handleFiltersUpdated}
          game={filterGame}
        />
      )}
    </Box>
  );
};

export default MyGamesList; 