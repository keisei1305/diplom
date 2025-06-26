using System.IO;
using System.Threading.Tasks;
using GameService.Core.Models;
using Microsoft.AspNetCore.Http;

namespace GameService.Core.Interfaces
{
    public interface IGameFileService
    {
        Task<GameFile> UploadGameFileAsync(string gameId, IFormFile file);
        Task<(Stream FileStream, string FileName)> DownloadGameFileAsync(string gameId);
        Task DeleteGameFileAsync(string gameId);
        Task<bool> VerifyFileHashAsync(string gameId, string expectedHash);

        public Task<GameFile> UpdateGameFileAsync(string gameId, IFormFile file);
    }
} 