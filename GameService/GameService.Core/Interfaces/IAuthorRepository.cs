using GameService.Core.Models;

namespace GameService.Core.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(string id);
        Task<Author> GetByUserIdAsync(string userId);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<IEnumerable<Author>> GetAllAsync(int page, int pagecount);
        Task<Author> AddAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task DeleteAsync(Author author);
    }
} 