using System.Collections.Generic;
using System.Threading.Tasks;
using GameService.Core.Models;

namespace GameService.Core.Interfaces
{
    public interface IFilterRepository
    {
        Task<Filter> GetByIdAsync(string id);
        Task<IEnumerable<Filter>> GetAllAsync();
        Task<IEnumerable<Filter>> GetByTypeAsync(string filterType);
        Task<Filter> AddAsync(Filter filter);
        Task<Filter> UpdateAsync(Filter filter);
        Task DeleteAsync(Filter filter);
    }
} 