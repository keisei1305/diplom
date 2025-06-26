using System.Collections.Generic;
using System.Threading.Tasks;
using GameService.Core.DTOs;

namespace GameService.Core.Interfaces
{
    public interface IFilterService
    {
        Task<FilterDto> GetByIdAsync(string id);
        Task<IEnumerable<FilterDto>> GetAllAsync();
        Task<IEnumerable<FilterDto>> GetByTypeAsync(string filterType);
        Task<FilterDto> CreateAsync(CreateFilterDto createFilterDto);
        Task<FilterDto> UpdateAsync(string id, UpdateFilterDto updateFilterDto);
        Task DeleteAsync(string id);
    }
} 