using GameService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameServiceApi.Controllers
{
    [ApiController]
    [Route("api/gamefile")]
    public class GameFileController : ControllerBase
    {
        private readonly IGameFileService _gameFileService;
        private readonly ILogger<GameFileController> _logger;

        public GameFileController(IGameFileService gameFileService, ILogger<GameFileController> logger)
        {
            _gameFileService = gameFileService;
            _logger = logger;
        }

        [HttpPost("{gameId}")]
        public async Task<IActionResult> UploadGameFile(string gameId, IFormFile file)
        {
            try
            {
                var gameFile = await _gameFileService.UploadGameFileAsync(gameId, file);
                return Ok(gameFile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading game file for game {GameId}", gameId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> DownloadGameFile(string gameId)
        {
            try
            {
                var (fileStream, fileName) = await _gameFileService.DownloadGameFileAsync(gameId);
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                return File(fileStream, "application/zip", fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading game file for game {GameId}", gameId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{gameId}")]
        public async Task<IActionResult> DeleteGameFile(string gameId)
        {
            try
            {
                await _gameFileService.DeleteGameFileAsync(gameId);
                return NoContent();
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting game file for game {GameId}", gameId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{gameId}/verify")]
        public async Task<IActionResult> VerifyGameFile(string gameId, [FromQuery] string expectedHash)
        {
            try
            {
                var isValid = await _gameFileService.VerifyFileHashAsync(gameId, expectedHash);
                return Ok(new { isValid });
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying game file for game {GameId}", gameId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{gameId}")]
        public async Task<IActionResult> UpdateGameFile(string gameId, IFormFile file)
        {
            try
            {
                var gameFile = await _gameFileService.UpdateGameFileAsync(gameId, file);
                return Ok(gameFile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating game file for game {GameId}", gameId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 