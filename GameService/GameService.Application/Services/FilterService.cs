using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameService.Core.DTOs;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Services
{
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;
        private readonly ILogger<FilterService> _logger;

        public FilterService(
            IFilterRepository filterRepository,
            ILogger<FilterService> logger)
        {
            _filterRepository = filterRepository;
            _logger = logger;
        }

        public async Task<FilterDto> GetByIdAsync(string id)
        {
            var filter = await _filterRepository.GetByIdAsync(id);
            return filter != null ? MapToDto(filter) : null;
        }

        public async Task<IEnumerable<FilterDto>> GetAllAsync()
        {
            var filters = await _filterRepository.GetAllAsync();
            return filters.Select(MapToDto);
        }

        public async Task<IEnumerable<FilterDto>> GetByTypeAsync(string filterType)
        {
            var filters = await _filterRepository.GetByTypeAsync(filterType);
            return filters.Select(MapToDto);
        }

        public async Task<FilterDto> CreateAsync(CreateFilterDto createFilterDto)
        {
            var filter = new Filter
            {
                Id = Guid.NewGuid().ToString(),
                Name = createFilterDto.Name,
                FilterType = createFilterDto.FilterType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _filterRepository.AddAsync(filter);
            return MapToDto(filter);
        }

        public async Task<FilterDto> UpdateAsync(string id, UpdateFilterDto updateFilterDto)
        {
            var filter = await _filterRepository.GetByIdAsync(id);
            if (filter == null)
            {
                throw new ArgumentException($"Filter with id {id} not found");
            }

            filter.Name = updateFilterDto.Name;
            filter.FilterType = updateFilterDto.FilterType;
            filter.UpdatedAt = DateTime.UtcNow;

            await _filterRepository.UpdateAsync(filter);
            return MapToDto(filter);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = await _filterRepository.GetByIdAsync(id);
            if (filter == null)
            {
                throw new ArgumentException($"Filter with id {id} not found");
            }

            await _filterRepository.DeleteAsync(filter);
        }

        private static FilterDto MapToDto(Filter filter)
        {
            return new FilterDto
            {
                Id = filter.Id,
                Name = filter.Name,
                FilterType = filter.FilterType,
                CreatedAt = filter.CreatedAt,
                UpdatedAt = filter.UpdatedAt
            };
        }
    }
} 