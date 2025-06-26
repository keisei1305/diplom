using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Application.DTO;

namespace UserService.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetByIdsAsync(string[] ids);
        Task<UserDto> CreateAsync(CreateUserRequest request);
        Task<bool> UpdateAsync(UpdateUserRequest request);
        Task<bool> DeleteAsync(string id);
    }
} 