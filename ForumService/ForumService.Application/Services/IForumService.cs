using System.Collections.Generic;
using System.Threading.Tasks;
using ForumService.Application.DTO;

namespace ForumService.Application.Services
{
    public interface IForumService
    {
        Task<IEnumerable<ForumDto>> GetAllAsync();
        Task<IEnumerable<ForumWithAuthorDto>> GetAllWithAuthorsAsync();
        Task<ForumDto?> GetByIdAsync(int id);
        Task<ForumWithAuthorDto?> GetByIdWithAuthorAsync(int id);
        Task<ForumDto> CreateAsync(CreateForumRequest request);
        Task<bool> UpdateAsync(UpdateForumRequest request);
        Task<bool> DeleteAsync(int id);
    }
} 