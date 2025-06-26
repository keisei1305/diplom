using UserService.Application.DTO;
using UserService.Core.Entities;
using UserService.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Nickname = u.Nickname,
                Email = u.Email
            });
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;
            return new UserDto { Id = u.Id, Nickname = u.Nickname, Email = u.Email };
        }

        public async Task<IEnumerable<UserDto>> GetByIdsAsync(string[] ids)
        {
            var users = await _repo.GetByIdsAsync(ids);
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Nickname = u.Nickname,
                Email = u.Email
            });
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            var user = new User { Id = request.Id, Nickname = request.Nickname, Email = request.Email };
            await _repo.AddAsync(user);
            return new UserDto { Id = user.Id, Nickname = user.Nickname, Email = user.Email };
        }

        public async Task<bool> UpdateAsync(UpdateUserRequest request)
        {
            var user = await _repo.GetByIdAsync(request.Id);
            if (user == null) return false;
            user.Nickname = request.Nickname;
            user.Email = request.Email;
            await _repo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
} 