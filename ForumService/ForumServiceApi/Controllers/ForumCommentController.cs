using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ForumService.Application.Services;
using ForumService.Application.DTO;

namespace ForumServiceApi.Controllers
{
    [ApiController]
    [Route("api/forums/{forumId}/comments")]
    public class ForumCommentController : ControllerBase
    {
        private readonly IForumCommentService _service;
        public ForumCommentController(IForumCommentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll(int forumId) => Ok(await _service.GetWithAuthorsByForumIdAsync(forumId));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int forumId, int id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null || comment.ForumId != forumId) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int forumId, [FromBody] CreateForumCommentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (request.ForumId != forumId) return BadRequest();
            var comment = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { forumId = comment.ForumId, id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int forumId, int id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null || comment.ForumId != forumId) return NotFound();
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 