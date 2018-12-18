using System.Collections.Generic;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _repository;

        public CommentsController(ICommentRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDetailDto>> Get(int id)
        {
            var user = await _repository.ReadAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return user;
        }

        [Route("~/api/[controller]/user/{userId}")]
        [HttpGet]
        public IEnumerable<CommentDetailDto> GetCommentsByUserId(string userId)
        {
            return _repository.ReadByUserId(userId);
        }

        [Route("~/api/[controller]/thread/{threadId}")]
        [HttpGet]
        public IEnumerable<CommentDetailDto> GetCommentsByThreadId(int threadId)
        {
            return _repository.ReadByThreadId(threadId);

        }

        [HttpPost]
        public async Task<ActionResult<CommentDetailDto>> Post([FromBody] CommentCreateUpdateDto dto)
        {
            var created = await _repository.CreateAsync(dto);
            if (created is null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new {created.Id}, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CommentCreateUpdateDto dto)
        {
            var successful = await _repository.UpdateAsync(id, dto);
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