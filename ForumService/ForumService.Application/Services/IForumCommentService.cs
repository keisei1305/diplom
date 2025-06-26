using System.Collections.Generic;
using System.Threading.Tasks;
using ForumService.Application.DTO;

namespace ForumService.Application.Services
{
    public interface IForumCommentService
    {
        Task<IEnumerable<ForumCommentDto>> GetByForumIdAsync(int forumId);
        Task<ForumCommentDto?> GetByIdAsync(int id);
        Task<ForumCommentDto> CreateAsync(CreateForumCommentRequest request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ForumCommentWithAuthorDto>> GetWithAuthorsByForumIdAsync(int forumId);
    }
} 