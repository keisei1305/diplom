using System.Security.Cryptography;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Services
{
    public class GameFileService : IGameFileService
    {
        private readonly IGameFileRepository _gameFileRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GameFileService> _logger;
        private readonly string _uploadDirectory;

        public GameFileService(
            IGameFileRepository gameFileRepository,
            IGameRepository gameRepository,
            ILogger<GameFileService> logger)
        {
            _gameFileRepository = gameFileRepository;
            _gameRepository = gameRepository;
            _logger = logger;
            _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<GameFile> UploadGameFileAsync(string gameId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }

            if (!file.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only ZIP archives are allowed");
            }

            var fileName = $"{Guid.NewGuid()}.zip";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileInfo = new FileInfo(filePath);
            var fileHash = await CalculateFileHashAsync(filePath);

            var gameFile = new GameFile
            {
                Id = Guid.NewGuid().ToString(),
                GameId = gameId,
                OriginalName = file.FileName,
                Path = filePath,
                Weight = fileInfo.Length,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _gameFileRepository.AddAsync(gameFile);
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game != null)
            {
                game.GameFileId = gameFile.Id;
                await _gameRepository.UpdateAsync(game);
            }
            else
            {
                _logger.LogWarning("Could not find game with ID {GameId} to update GameFileId.", gameId);
            }

            return gameFile;
        }

        public async Task<(Stream FileStream, string FileName)> DownloadGameFileAsync(string gameId)
        {
            var gameFile = await _gameFileRepository.GetByGameIdAsync(gameId);
            if (gameFile == null || !File.Exists(gameFile.Path))
            {
                throw new FileNotFoundException($"Game file not found for game {gameId}");
            }

            var fileStream = new FileStream(gameFile.Path, FileMode.Open, FileAccess.Read);
            return (fileStream, gameFile.OriginalName ?? Path.GetFileName(gameFile.Path));
        }

        public async Task DeleteGameFileAsync(string gameId)
        {
            var gameFile = await _gameFileRepository.GetByGameIdAsync(gameId);
            if (gameFile == null)
            {
                throw new FileNotFoundException($"Game file not found for game {gameId}");
            }

            if (File.Exists(gameFile.Path))
            {
                File.Delete(gameFile.Path);
            }

            await _gameFileRepository.DeleteAsync(gameFile);
        }

        public async Task<bool> VerifyFileHashAsync(string gameId, string expectedHash)
        {
            var gameFile = await _gameFileRepository.GetByGameIdAsync(gameId);
            if (gameFile == null || !File.Exists(gameFile.Path))
            {
                throw new FileNotFoundException($"Game file not found for game {gameId}");
            }

            var actualHash = await CalculateFileHashAsync(gameFile.Path);
            return actualHash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<GameFile> UpdateGameFileAsync(string gameId, IFormFile file)
        {
            var oldFile = await _gameFileRepository.GetByGameIdAsync(gameId);
            if (oldFile != null)
            {
                if (File.Exists(oldFile.Path))
                    File.Delete(oldFile.Path);
                await _gameFileRepository.DeleteAsync(oldFile);
            }

            var fileName = $"{Guid.NewGuid()}.zip";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileInfo = new FileInfo(filePath);

            var gameFile = new GameFile
            {
                Id = Guid.NewGuid().ToString(),
                GameId = gameId,
                OriginalName = file.FileName,
                Path = filePath,
                Weight = fileInfo.Length,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _gameFileRepository.AddAsync(gameFile);

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game != null)
            {
                game.GameFileId = gameFile.Id;
                await _gameRepository.UpdateAsync(game);
            }

            return gameFile;
        }

        private static async Task<string> CalculateFileHashAsync(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hash = await Task.Run(() => md5.ComputeHash(stream));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
} 