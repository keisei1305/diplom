using System.Collections.Generic;
using System.Threading.Tasks;
using ForumService.Core.Entities;

namespace ForumService.Core.Interfaces
{
    public interface IForumRepository
    {
        Task<IEnumerable<Forum>> GetAllAsync();
        Task<Forum?> GetByIdAsync(int id);
        Task AddAsync(Forum forum);
        Task UpdateAsync(Forum forum);
        Task DeleteAsync(int id);
    }
} 