using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Forum;
using CommunIT.Shared.Portable.DTOs.Thread;
using Microsoft.AspNetCore.Mvc;

namespace CommunIT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class ThreadsController : Controller
    {
        private readonly IThreadRepository _repository;

        public ThreadsController(IThreadRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadDetailDto>> Get(int id)
        {
            var thread = await _repository.ReadAsync(id);
            if (thread is null)
            {
                return NotFound();
            }

            return thread;
        }

        [HttpGet("{forumId}")]
        [Route("~/api/[controller]/forum/{forumId}")]
        public ActionResult<IEnumerable<ThreadDetailDto>> GetByForumId(int forumId)
        {
            return _repository.ReadByForumId(forumId).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<ThreadDetailDto>> Post([FromBody] ThreadCreateDto dto)
        {
            var created = await _repository.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ThreadUpdateDto dto)
        {
            var successful = await _repository.UpdateAsync(dto);
            if (successful)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var successful = await _repository.DeleteAsync(id);
            if (successful)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}