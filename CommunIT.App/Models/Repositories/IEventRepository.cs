using CommunIT.Shared.Portable.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface IEventRepository
    {
        Task<EventUpdateDetailDto> CreateAsync(EventCreateDto dto);
        Task<IEnumerable<EventUpdateDetailDto>> ReadByCommunityIdAsync(int eventId);
        Task<IEnumerable<EventUpdateDetailDto>> ReadByUserIdAsync(string userId);
        Task<bool> AddUserToEventAsync(int eventId, string userId);
        Task<bool> RemoveUserFromEventAsync(int eventId, string userId);
    }
}
