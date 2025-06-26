using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GameService.Core.DTOs;
using GameService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameServiceApi.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetById(string id)
        {
            try
            {
                var game = await _gameService.GetByIdAsync(id);
                if (game == null)
                {
                    return NotFound();
                }
                return Ok(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting game by id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<(IEnumerable<GameDto> Games, int TotalCount)>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _gameService.GetAllAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all games");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetByAuthor(string authorId)
        {
            var games = await _gameService.GetByAuthorIdAsync(authorId);
            return Ok(games);
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetMyGames()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }
            var author = await _gameService.GetAuthorByUserIdAsync(userId);
            if (author == null)
            {
                return NotFound("Author not found for current user");
            }
            var games = await _gameService.GetByAuthorIdAsync(author.Id);
            return Ok(games);
        }

        [HttpGet("filter/{filterId}")]
        public async Task<ActionResult<(IEnumerable<GameDto> Games, int TotalCount)>> GetByFilterId(
            string filterId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _gameService.GetByFilterIdAsync(filterId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting games by filter id {FilterId}", filterId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameDto>>> Search([FromQuery] string name)
        {
            var games = await _gameService.SearchByNameAsync(name);
            return Ok(games);
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> Create([FromBody] CreateGameDto createGameDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in header");
                var game = await _gameService.CreateAsync(createGameDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating game");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GameDto>> Update(string id, [FromBody] UpdateGameDto updateGameDto)
        {
            try
            {
                var game = await _gameService.UpdateAsync(id, updateGameDto);
                return Ok(game);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating game {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _gameService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting game {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 