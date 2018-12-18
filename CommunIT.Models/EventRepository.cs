using CommunIT.Entities;
using CommunIT.Entities.Context;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Event;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunIT.Models
{
    public class EventRepository : IEventRepository
    {
        private readonly ICommunITContext _context;

        public EventRepository(ICommunITContext context)
        {
            _context = context;
        }

        public async Task<EventUpdateDetailDto> CreateAsync(EventCreateDto dto)
        {
            var community = await _context.Communities.FindAsync(dto.CommunityId);

            if (community is null)
            {
                return null;
            }

            var eventDbo = new Event
            {
                Title = dto.Title,
                Date = dto.Date,
                Description = dto.Description,
                CommunityId = dto.CommunityId
            };

            var created = await _context.Events.AddAsync(eventDbo);
            await _context.SaveChangesAsync();

            return await ReadAsync(created.Entity.Id);
        }

        public async Task<EventUpdateDetailDto> ReadAsync(int id)
        {
            var eventDbo = await _context.Events.FindAsync(id);

            if (eventDbo is null)
            {
                return null;
            }

            return new EventUpdateDetailDto
            {
                Id = eventDbo.Id,
                Title = eventDbo.Title,
                Date = eventDbo.Date,
                Description = eventDbo.Description
            };
        }

        public async Task<bool> UpdateAsync(EventUpdateDetailDto dto)
        {
            var eventDbo = await _context.Events.FindAsync(dto.Id);

            if (eventDbo is null)
            {
                return false;
            }

            eventDbo.Title = dto.Title;
            eventDbo.Date = dto.Date;
            eventDbo.Description = dto.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var eventDbo = await _context.Events.FindAsync(id);

            if (eventDbo is null)
            {
                return false;
            }

            _context.Events.Remove(eventDbo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddUserToEventAsync(int eventId, string userId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var user = await _context.Users.FindAsync(userId);

            if (@event is null || user is null)
            {
                return false;
            }

            var isUserAlreadyInEvent = _context.EventUsers.Where(eu => eu.EventId == eventId).Any(evnt => evnt.UserId == userId);

            if (isUserAlreadyInEvent)
            {
                return false;
            }

            var eventUser = new EventUser
            {
                EventId = eventId,
                UserId = userId
            };

            await _context.EventUsers.AddAsync(eventUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public IEnumerable<EventUpdateDetailDto> ReadByCommunityId(int communityId)
        {
            var events = _context.Events.Where(e => e.CommunityId == communityId);

            return events.Select(e => new EventUpdateDetailDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date
            });
        }

        public IQueryable<EventUpdateDetailDto> ReadByUserId(string userId)
        {
            var events = _context.Events
                .Where(e => e.EventUsers.Any(ev => ev.UserId == userId)).OrderBy(e => e.Date);

            return events.Select(e => new EventUpdateDetailDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date
            });
        }

        public async Task<bool> RemoveUserFromEventAsync(int eventId, string userId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var user = await _context.Users.FindAsync(userId);

            if (@event is null || user is null)
            {
                return false;
            }

            var isUserAlreadyInEvent = _context.EventUsers.Where(eu => eu.EventId == eventId).Any(evnt => evnt.UserId == userId);

            if (isUserAlreadyInEvent)
            {
                var eventUser = new EventUser
                {
                    EventId = eventId,
                    UserId = userId
                };

                _context.EventUsers.Remove(eventUser);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
