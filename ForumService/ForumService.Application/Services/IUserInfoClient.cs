using System.Collections.Generic;
using System.Threading.Tasks;
using ForumService.Application.DTO;

namespace ForumService.Application.Services
{
    public interface IUserInfoClient
    {
        Task<UserShortDto?> GetUserShortAsync(string userId);
        Task<IEnumerable<UserShortDto>> GetUsersBatchAsync(IEnumerable<string> userIds);
    }
} 