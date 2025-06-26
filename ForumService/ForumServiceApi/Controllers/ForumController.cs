using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ForumService.Application.Services;
using ForumService.Application.DTO;

namespace ForumServiceApi.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _service;
        public ForumController(IForumService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllWithAuthorsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var forum = await _service.GetByIdWithAuthorAsync(id);
            if (forum == null) return NotFound();
            return Ok(forum);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateForumRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var forum = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = forum.Id }, forum);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateForumRequest request)
        {
            if (id != request.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateAsync(request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 