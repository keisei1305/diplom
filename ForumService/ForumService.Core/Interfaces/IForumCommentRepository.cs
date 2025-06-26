using System.Collections.Generic;
using System.Threading.Tasks;
using ForumService.Core.Entities;

namespace ForumService.Core.Interfaces
{
    public interface IForumCommentRepository
    {
        Task<IEnumerable<ForumComment>> GetByForumIdAsync(int forumId);
        Task<ForumComment?> GetByIdAsync(int id);
        Task AddAsync(ForumComment comment);
        Task DeleteAsync(int id);
    }
} 