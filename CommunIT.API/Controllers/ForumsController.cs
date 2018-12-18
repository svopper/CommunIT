using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Forum;
using CommunIT.Shared.Portable.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace CommunIT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class ForumsController : Controller
    {
        private readonly IForumRepository _repository;

        public ForumsController(IForumRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ForumDetailDto>> Get(int id)
        {
            var forum = await _repository.ReadAsync(id);
            if (forum is null)
            {
                return NotFound();
            }

            return forum;
        }

        [HttpPost]
        public async Task<ActionResult<ForumDetailDto>> Post([FromBody] ForumCreateDto dto)
        {
            var created = await _repository.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ForumUpdateDto dto)
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

        [HttpGet("{communityId}")]
        [Route("~/api/[controller]/community/{communityId}")]
        public ActionResult<IEnumerable<ForumDetailDto>> GetByCommunityId(int communityId)
        {
            return _repository.ReadByCommunityId(communityId).ToList();
        }

    }
}