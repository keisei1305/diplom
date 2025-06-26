using GameService.Core.DTOs;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace GameService.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameFileRepository _gameFileRepository;
        private readonly ILogger<GameService> _logger;
        private readonly IAuthorRepository _authorRepository;
        private readonly string _uploadDirectory;

        public GameService(
            IGameRepository gameRepository,
            IGameFileRepository gameFileRepository,
            ILogger<GameService> logger,
            IAuthorRepository authorRepository)
        {
            _gameRepository = gameRepository;
            _gameFileRepository = gameFileRepository;
            _logger = logger;
            _authorRepository = authorRepository;
            _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<GameDto> GetByIdAsync(string id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return game != null ? MapToDto(game) : null;
        }

        public async Task<IEnumerable<GameDto>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var games = await _gameRepository.GetAllAsync(page, pageSize);
            return games.Select(MapToDto);
        }

        public async Task<IEnumerable<GameDto>> GetByAuthorIdAsync(string authorId, int page = 1, int pageSize = 10)
        {
            var games = await _gameRepository.GetByAuthorIdAsync(authorId, page, pageSize);
            return games.Select(MapToDto);
        }

        public async Task<IEnumerable<GameDto>> GetByFilterIdAsync(string filterId, int page = 1, int pageSize = 10)
        {
            var games = await _gameRepository.GetByFilterIdAsync(filterId, page, pageSize);
            return games.Select(MapToDto);
        }

        public async Task<IEnumerable<GameDto>> SearchByNameAsync(string searchTerm)
        {
            var games = await _gameRepository.SearchByNameAsync(searchTerm);
            return games.Select(MapToDto);
        }

        public async Task<GameDto> CreateAsync(CreateGameDto createGameDto, string userId)
        {
            var author = await _authorRepository.GetByUserIdAsync(userId);
            if (author == null)
                throw new ArgumentException($"Author not found for userId {userId}");
            var game = new Game
            {
                Id = Guid.NewGuid().ToString(),
                Name = createGameDto.Name,
                Description = createGameDto.Description,
                AuthorId = author.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _gameRepository.AddAsync(game);
            return MapToDto(game);
        }

        public async Task<GameDto> UpdateAsync(string id, UpdateGameDto updateGameDto)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new ArgumentException($"Game with id {id} not found");
            }

            game.Name = updateGameDto.Name;
            game.Description = updateGameDto.Description;
            game.UpdatedAt = DateTime.UtcNow;

            await _gameRepository.UpdateAsync(game);
            return MapToDto(game);
        }

        public async Task DeleteAsync(string id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new ArgumentException($"Game with id {id} not found");
            }

            if (game.GameFile != null)
            {
                if (File.Exists(game.GameFile.Path))
                {
                    File.Delete(game.GameFile.Path);
                }
                await _gameFileRepository.DeleteAsync(game.GameFile);
            }

            await _gameRepository.DeleteAsync(game);
        }

        public async Task<(byte[] fileBytes, string fileName)> DownloadGameAsync(string id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new ArgumentException($"Game with id {id} not found");
            }

            if (game.GameFile == null || !File.Exists(game.GameFile.Path))
            {
                throw new FileNotFoundException($"Game file not found for game {id}");
            }

            var fileBytes = await File.ReadAllBytesAsync(game.GameFile.Path);
            return (fileBytes, $"{game.Name}.zip");
        }

        private static GameDto MapToDto(Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                AuthorId = game.AuthorId,
                AuthorName = game.Author?.Name,
                GameFileId = game.GameFileId,
                FileSize = game.GameFile?.Weight ?? 0,
                CreatedAt = game.CreatedAt,
                UpdatedAt = game.UpdatedAt,
                Filters = game.Filters?.Select(f => new FilterDto
                {
                    Id = f.Id,
                    FilterType = f.FilterType,
                    Name = f.Name,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                }).ToList()
            };
        }

        public async Task<IEnumerable<GameDto>> GetByAuthorIdAsync(string authorId)
        {
            var games = await _gameRepository.GetByAuthorIdAsync(authorId);
            return games.Select(MapToDto);
        }

        public async Task<AuthorDto> GetAuthorByUserIdAsync(string userId)
        {
            var author = await _authorRepository.GetByUserIdAsync(userId);
            return author != null ? new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
                UserId = author.UserId,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            } : null;
        }
    }
} 