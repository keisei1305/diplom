using System;
using System.Threading.Tasks;
using GameService.Core.DTOs;
using GameService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameServiceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;
        private readonly ILogger<FilterController> _logger;

        public FilterController(IFilterService filterService, ILogger<FilterController> logger)
        {
            _filterService = filterService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FilterDto>> GetById(string id)
        {
            try
            {
                var filter = await _filterService.GetByIdAsync(id);
                if (filter == null)
                {
                    return NotFound();
                }
                return Ok(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting filter by id {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilterDto>>> GetAll()
        {
            try
            {
                var result = await _filterService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all filters");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("type/{filterType}")]
        public async Task<ActionResult<IEnumerable<FilterDto>>> GetByType(
            string filterType)
        {
            try
            {
                var result = await _filterService.GetByTypeAsync(filterType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting filters by type {FilterType}", filterType);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FilterDto>> Create([FromBody] CreateFilterDto createFilterDto)
        {
            try
            {
                var filter = await _filterService.CreateAsync(createFilterDto);
                return CreatedAtAction(nameof(GetById), new { id = filter.Id }, filter);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating filter");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FilterDto>> Update(string id, [FromBody] UpdateFilterDto updateFilterDto)
        {
            try
            {
                var filter = await _filterService.UpdateAsync(id, updateFilterDto);
                return Ok(filter);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating filter {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _filterService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting filter {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 