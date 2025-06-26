import React, { useState, useEffect } from 'react';
import { 
  Container, 
  Typography, 
  TextField, 
  Button, 
  Box, 
  Grid, 
  CircularProgress,
  Alert,
  InputAdornment,
  Paper,
  Checkbox,
  FormGroup,
  FormControlLabel,
  Divider
} from '@mui/material';
import { Search as SearchIcon, Clear as ClearIcon, Add as AddIcon } from '@mui/icons-material';
import { useDispatch, useSelector } from 'react-redux';
import { fetchAllGames, searchGames, clearSearch, setSearchTerm } from '../store/gamesSlice';
import GameCard from '../components/GameCard';
import AddGameForm from '../components/AddGameForm';
import { getAllFilters } from '../api/filterApi';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from '@mui/material/styles';
import { grey } from '@mui/material/colors';

const Games = () => {
  const [searchInput, setSearchInput] = useState('');
  const [addGameDialogOpen, setAddGameDialogOpen] = useState(false);
  const dispatch = useDispatch();
  const { games, searchResults, isLoading, error, searchTerm } = useSelector((state) => state.games);
  const [allFilters, setAllFilters] = useState([]);
  const [selectedFilters, setSelectedFilters] = useState([]);
  const theme = useTheme();
  const isDesktop = useMediaQuery(theme.breakpoints.up('md'));

  useEffect(() => {
    dispatch(fetchAllGames());
    getAllFilters().then(setAllFilters).catch(() => setAllFilters([]));
  }, [dispatch]);

  const handleSearch = () => {
    if (searchInput.trim()) {
      dispatch(setSearchTerm(searchInput.trim()));
      dispatch(searchGames(searchInput.trim()));
    }
  };

  const handleClearSearch = () => {
    setSearchInput('');
    dispatch(clearSearch());
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      handleSearch();
    }
  };

  const handleAddGameSuccess = () => {
    dispatch(fetchAllGames());
  };

  const gamesToShow = searchTerm ? searchResults : games;
  const isSearchActive = searchTerm.length > 0;

  const filtersByType = allFilters.reduce((acc, filter) => {
    if (!acc[filter.filterType]) acc[filter.filterType] = [];
    acc[filter.filterType].push(filter);
    return acc;
  }, {});

  const filteredGames = selectedFilters.length === 0
    ? gamesToShow
    : gamesToShow.filter(game =>
        game.filters && selectedFilters.every(fId => game.filters.some(f => f.id === fId))
      );

  const handleFilterChange = (filterId) => {
    setSelectedFilters((prev) =>
      prev.includes(filterId)
        ? prev.filter((id) => id !== filterId)
        : [...prev, filterId]
    );
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4, position: 'relative' }}>
      {/* Заголовок */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4 }}>
        <Typography variant="h3" component="h1" gutterBottom>
          Поиск игр
        </Typography>
      </Box>

      {/* Поисковик и карточки на всю ширину */}
      <Box>
        {/* Поисковая панель */}
        <Paper elevation={3} sx={{ p: 3, mb: 4 }}>
          <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
            <TextField
              fullWidth
              variant="outlined"
              placeholder="Введите название игры для поиска..."
              value={searchInput}
              onChange={(e) => setSearchInput(e.target.value)}
              onKeyPress={handleKeyPress}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
            />
            <Button
              variant="contained"
              onClick={handleSearch}
              disabled={!searchInput.trim() || isLoading}
              sx={{ minWidth: 120 }}
            >
              {isLoading ? <CircularProgress size={24} /> : 'Поиск'}
            </Button>
            {isSearchActive && (
              <Button
                variant="outlined"
                onClick={handleClearSearch}
                startIcon={<ClearIcon />}
              >
                Очистить
              </Button>
            )}
          </Box>
        </Paper>

        {/* Результаты поиска */}
        {isSearchActive && (
          <Box sx={{ mb: 3 }}>
            <Typography variant="h5" gutterBottom>
              Результаты поиска по "{searchTerm}"
            </Typography>
            {searchResults.length === 0 && !isLoading && (
              <Alert severity="info">
                По вашему запросу ничего не найдено. Попробуйте изменить поисковый запрос.
              </Alert>
            )}
          </Box>
        )}

        {/* Ошибка */}
        {error && (
          <Alert severity="error" sx={{ mb: 3 }}>
            {error}
          </Alert>
        )}

        {/* Список игр */}
        {isLoading ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        ) : (
          <Box>
            {filteredGames.map((game) => (
              <GameCard game={game} key={game.id} />
            ))}
          </Box>
        )}

        {/* Сообщение, если нет игр */}
        {!isLoading && filteredGames.length === 0 && !isSearchActive && (
          <Box sx={{ textAlign: 'center', py: 4 }}>
            <Typography variant="h6" color="text.secondary" sx={{ mb: 2 }}>
              Игры не найдены
            </Typography>
          </Box>
        )}

        {/* Информация о количестве результатов */}
        {!isLoading && filteredGames.length > 0 && (
          <Box sx={{ mt: 3, textAlign: 'center' }}>
            <Typography variant="body2" color="text.secondary">
              Найдено игр: {filteredGames.length}
            </Typography>
          </Box>
        )}
      </Box>

      {/* Фильтры: фиксированы справа на desktop, под контентом на mobile */}
      {isDesktop ? (
        <Box sx={{
          position: 'fixed',
          top: 100,
          right: 32,
          width: 320,
          zIndex: 1200,
          maxHeight: '80vh',
          overflowY: 'auto',
          bgcolor: grey[900],
          boxShadow: 3,
          borderRadius: 2,
          p: 2
        }}>
          <Typography variant="h6" gutterBottom sx={{ color: '#fff' }}>Фильтры по жанрам и категориям</Typography>
          {Object.keys(filtersByType).length === 0 && (
            <Typography sx={{ color: grey[300] }}>Нет фильтров</Typography>
          )}
          {Object.entries(filtersByType).map(([type, filters]) => (
            <Box key={type} sx={{ mb: 2 }}>
              <Typography variant="subtitle2" sx={{ mb: 1, color: grey[200] }}>{type}</Typography>
              <FormGroup>
                {filters.map((filter) => (
                  <FormControlLabel
                    key={filter.id}
                    control={
                      <Checkbox
                        checked={selectedFilters.includes(filter.id)}
                        onChange={() => handleFilterChange(filter.id)}
                        sx={{ color: '#fff', '&.Mui-checked': { color: grey[300] } }}
                      />
                    }
                    label={<span style={{ color: '#fff' }}>{filter.name}</span>}
                  />
                ))}
              </FormGroup>
              <Divider sx={{ mt: 1, bgcolor: grey[800] }} />
            </Box>
          ))}
        </Box>
      ) : (
        <Box sx={{ mt: 4 }}>
          <Paper elevation={2} sx={{ p: 2, bgcolor: grey[900] }}>
            <Typography variant="h6" gutterBottom sx={{ color: '#fff' }}>Фильтры по жанрам и категориям</Typography>
            {Object.keys(filtersByType).length === 0 && (
              <Typography sx={{ color: grey[300] }}>Нет фильтров</Typography>
            )}
            {Object.entries(filtersByType).map(([type, filters]) => (
              <Box key={type} sx={{ mb: 2 }}>
                <Typography variant="subtitle2" sx={{ mb: 1, color: grey[200] }}>{type}</Typography>
                <FormGroup>
                  {filters.map((filter) => (
                    <FormControlLabel
                      key={filter.id}
                      control={
                        <Checkbox
                          checked={selectedFilters.includes(filter.id)}
                          onChange={() => handleFilterChange(filter.id)}
                          sx={{ color: '#fff', '&.Mui-checked': { color: grey[300] } }}
                        />
                      }
                      label={<span style={{ color: '#fff' }}>{filter.name}</span>}
                    />
                  ))}
                </FormGroup>
                <Divider sx={{ mt: 1, bgcolor: grey[800] }} />
              </Box>
            ))}
          </Paper>
        </Box>
      )}

      {/* Диалог добавления игры */}
    </Container>
  );
};

export default Games;
 