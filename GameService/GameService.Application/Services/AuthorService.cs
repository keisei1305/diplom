using GameService.Core.DTOs;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(
            IAuthorRepository authorRepository,
            ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<AuthorDto> GetByIdAsync(string id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            return author != null ? MapToDto(author) : null;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return authors.Select(MapToDto);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto createAuthorDto, string userId)
        {
            var author = new Author
            {
                Id = Guid.NewGuid().ToString(),
                Name = createAuthorDto.Name,
                Description = createAuthorDto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _authorRepository.AddAsync(author);
            return MapToDto(author);
        }

        public async Task<AuthorDto> UpdateAsync(string id, UpdateAuthorDto updateAuthorDto)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                throw new ArgumentException($"Author with id {id} not found");
            }

            author.Name = updateAuthorDto.Name;
            author.Description = updateAuthorDto.Description;
            author.UpdatedAt = DateTime.UtcNow;

            await _authorRepository.UpdateAsync(author);
            return MapToDto(author);
        }

        public async Task DeleteAsync(string id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                throw new ArgumentException($"Author with id {id} not found");
            }

            await _authorRepository.DeleteAsync(author);
        }

        private static AuthorDto MapToDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
                UserId = author.UserId,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            };
        }
    }
} 