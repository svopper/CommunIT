using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Entities;
using CommunIT.Models;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Community;
using CommunIT.Shared.Portable.DTOs.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class CommunityController : Controller
    {
        private readonly ICommunityRepository _repository;

        public CommunityController(ICommunityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityDetailDto>> Get(int id)
        {
            var community = await _repository.ReadAsync(id);
            if (community is null)
            {
                return NotFound();
            }

            return community;
        }

        [HttpGet("{userId}")]
        [Route("~/api/[controller]/user/{userId}")]
        public async Task<ActionResult<IEnumerable<CommunityUpdateListViewDto>>> GetByUserId(string userId)
        {
            return await _repository.ReadByUserId(userId).ToListAsync();
        }

        [HttpGet("{userId}")]
        [Route("~/api/[controller]/user/{userId}/admin")]
        public async Task<ActionResult<IEnumerable<CommunityUpdateListViewDto>>> GetAdministratedByUserId(string userId)
        {
            return await _repository.ReadAdministratedByUserId(userId).ToListAsync();
        }

        [HttpGet("{userId}")]
        [Route("~/api/[controller]/user/{userId}/favorite")]
        public async Task<ActionResult<IEnumerable<CommunityUpdateListViewDto>>> GetFavoritedByUserId(string userId)
        {
            return await _repository.ReadFavoritedByUserId(userId).ToListAsync();
        }

        [HttpPost]
        [Route("~/api/[controller]/{communityId}/addfavorite/{userId}")]
        public async Task<ActionResult> PostCommunityToUserFavorite(int communityId, string userId)
        {
            var success =  await _repository.AddCommunityToUserFavorites(communityId, userId);
            if (success)
            {
                return NoContent();
            }
            return NotFound();

        }

        [HttpGet]
        [Route("~/api/[controller]/popular")]
        public async Task<ActionResult<IEnumerable<CommunityUpdateListViewDto>>> GetMostPopular()
        {
            return await _repository.ReadMostPopular().ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityUpdateListViewDto>>> Get()
        {
            return await _repository.Read().ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CommunityDetailDto>> Post([FromBody] CommunityCreateDto dto)
        {
            var created = await _repository.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] CommunityUpdateListViewDto dto)
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

        [Route("~/api/[controller]/{communityId}/adduser/{userId}")]
        [HttpPost]
        public async Task<IActionResult> PostUserToCommunity(int communityId, string userId)
        {
            var successful = await _repository.AddUserToCommunity(communityId, userId);

            if (successful)
            {
                return NoContent();
            }

            return NotFound();
        }

        [Route("~/api/[controller]/{communityId}/user/{userid}")]
        [HttpGet]
        public async Task<IActionResult> IsUserInCommunity(int communityId, string userId)
        {
            var successfull = await _repository.IsUserInCommunityAsync(communityId, userId);

            if (successfull)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}