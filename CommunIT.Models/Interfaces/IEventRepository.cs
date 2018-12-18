using CommunIT.Shared.Portable.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.Models.Interfaces
{
    public interface IEventRepository
    {
        Task<EventUpdateDetailDto> CreateAsync(EventCreateDto dto);
        Task<EventUpdateDetailDto> ReadAsync(int id);
        IEnumerable<EventUpdateDetailDto> ReadByCommunityId(int communityId);
        IQueryable<EventUpdateDetailDto> ReadByUserId(string userId);
        Task<bool> UpdateAsync(EventUpdateDetailDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddUserToEventAsync(int eventId, string userId);
        Task<bool> RemoveUserFromEventAsync(int eventId, string userId);

    }
}
