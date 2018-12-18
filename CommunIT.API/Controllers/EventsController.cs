using CommunIT.Models;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Event;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommunIT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class EventsController : Controller
    {
        private readonly IEventRepository _repository;

        public EventsController(IEventRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventUpdateDetailDto>> Get(int id)
        {
            var @event = await _repository.ReadAsync(id);
            if (@event is null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpGet("{communityId}")]
        [Route("~/api/[controller]/community/{communityId}")]
        public ActionResult<IEnumerable<EventUpdateDetailDto>> GetByCommunityId(int communityId)
        {
            return _repository.ReadByCommunityId(communityId).ToList();
        }

        [HttpGet("{userId}")]
        [Route("~/api/[controller]/users/{userId}")]
        public async Task<ActionResult<IEnumerable<EventUpdateDetailDto>>> GetByUserId(string userId)
        {
            return await _repository.ReadByUserId(userId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<EventUpdateDetailDto>> Post([FromBody] EventCreateDto dto)
        {
            var created = await _repository.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] EventUpdateDetailDto dto)
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

        [HttpPost]
        [Route("~/api/[controller]/{eventId}/adduser/{userId}")]
        public async Task<IActionResult> PostUserToEvent(int eventId, string userId)
        {
            var successful = await _repository.AddUserToEventAsync(eventId, userId);
            if (successful)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("~/api/[controller]/{eventId}/removeuser/{userId}")]
        public async Task<IActionResult> DeleteUserFromEvent(int eventId, string userId)
        {
            var successful = await _repository.RemoveUserFromEventAsync(eventId, userId);
            if (successful)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
