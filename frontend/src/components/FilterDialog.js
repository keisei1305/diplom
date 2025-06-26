import React, { useEffect, useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, CircularProgress, FormControl, InputLabel, Select, MenuItem, Checkbox, ListItemText, OutlinedInput, Alert } from '@mui/material';
import { getAllFilters } from '../api/filterApi';
import { updateGameFilters } from '../api/gamesApi';

const FilterDialog = ({ open, onClose, onSuccess, game }) => {
  const [filters, setFilters] = useState([]);
  const [selected, setSelected] = useState(game.filters ? game.filters.map(f => f.id) : []);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (open) {
      setLoading(true);
      getAllFilters()
        .then(setFilters)
        .catch(() => setError('Не удалось загрузить фильтры'))
        .finally(() => setLoading(false));
      setSelected(game.filters ? game.filters.map(f => f.id) : []);
    }
  }, [open, game]);

  const handleChange = (event) => {
    setSelected(event.target.value);
  };

  const handleSave = async () => {
    setSaving(true);
    setError('');
    try {
      await updateGameFilters(game.id, selected);
      onSuccess();
    } catch (e) {
      setError('Ошибка при сохранении фильтров');
    } finally {
      setSaving(false);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>Выберите фильтры для игры</DialogTitle>
      <DialogContent>
        {loading ? (
          <CircularProgress />
        ) : error ? (
          <Alert severity="error">{error}</Alert>
        ) : (
          <FormControl fullWidth margin="normal">
            <InputLabel id="filter-multiselect-label">Фильтры</InputLabel>
            <Select
              labelId="filter-multiselect-label"
              multiple
              value={selected}
              onChange={handleChange}
              input={<OutlinedInput label="Фильтры" />}
              renderValue={(selectedIds) =>
                filters
                  .filter(f => selectedIds.includes(f.id))
                  .map(f => f.name)
                  .join(', ')
              }
            >
              {filters.map((filter) => (
                <MenuItem key={filter.id} value={filter.id}>
                  <Checkbox checked={selected.indexOf(filter.id) > -1} />
                  <ListItemText primary={filter.name} />
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={saving}>Отмена</Button>
        <Button onClick={handleSave} disabled={saving || loading} variant="contained">Сохранить</Button>
      </DialogActions>
    </Dialog>
  );
};

export default FilterDialog; 