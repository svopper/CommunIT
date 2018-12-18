using CommunIT.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CommunIT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class UsersController : Controller
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> Get()
        {
            return await _repository.Read().ToListAsync();
        }

        [HttpPost]
        [Route("~/api/[controller]/search")]
        public async Task<ActionResult<UserDetailDto>> Get(UserSearchDto userDto)
        {
            var user = await _repository.ReadAsync(userDto.Username);
            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<UserDetailDto>> Post([FromBody] UserCreateDto dto)
        {
            var created = await _repository.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { created.Username }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserCreateDto dto)
        {
            var successful = await _repository.UpdateAsync(dto.Username, dto);
            if (successful)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var successful = await _repository.DeleteAsync(id);
            if (successful)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("{eventId}")]
        [Route("~/api/[controller]/event/{eventId}")]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetByEventId(int eventId)
        {
            return await _repository.ReadUsersInEvent(eventId).ToListAsync();
        }

        [HttpGet]
        [Route("~/api/[controller]/{userId}/isadmin/{communityId}")]
        public ActionResult IsUserAdminInCommunity(string userId, int communityId)
        {
            var success =  _repository.IsUserAdminInCommunity(userId, communityId);

            if (success)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("{communityId}")]
        [Route("~/api/[controller]/community/{communityId}")]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetByCommunityId(int communityId)
        {
            return await _repository.ReadUsersInCommunity(communityId).ToListAsync();
        }
    }
}