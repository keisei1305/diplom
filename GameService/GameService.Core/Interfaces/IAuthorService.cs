using GameService.Core.DTOs;

namespace GameService.Core.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetByIdAsync(string id);
        Task<IEnumerable<AuthorDto>> GetAllAsync();
        Task<AuthorDto> CreateAsync(CreateAuthorDto createAuthorDto, string userId);
        Task<AuthorDto> UpdateAsync(string id, UpdateAuthorDto updateAuthorDto);
        Task DeleteAsync(string id);
    }
} 