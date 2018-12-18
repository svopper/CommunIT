using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Event;

namespace CommunIT.App.Models.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly HttpClient _client;

        public EventRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> AddUserToEventAsync(int eventId, string userId)
        {
            var response = await _client.PostAsync($"api/events/{eventId}/adduser/{userId}", null);

            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public async Task<EventUpdateDetailDto> CreateAsync(EventCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync("api/events", dto);

            return await response.Content.ReadAsAsync<EventUpdateDetailDto>();
        }

        public async Task<IEnumerable<EventUpdateDetailDto>> ReadByCommunityIdAsync(int communityId)
        {
            var response = await _client.GetAsync($"api/events/community/{communityId}");

            return await response.Content.ReadAsAsync<IEnumerable<EventUpdateDetailDto>>();
        }

        public async Task<IEnumerable<EventUpdateDetailDto>> ReadByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"api/events/users/{userId}");

            return await response.Content.ReadAsAsync<IEnumerable<EventUpdateDetailDto>>();
        }

        public async Task<bool> RemoveUserFromEventAsync(int eventId, string userId)
        {
            var response = await _client.DeleteAsync($"api/events/{eventId}/removeuser/{userId}");
            
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
