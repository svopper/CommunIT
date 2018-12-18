using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.User;

namespace CommunIT.App.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _client;

        public UserRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> IsUserAdminInCommunity(string userId, int communityId)
        {
            var response = await _client.GetAsync($"api/users/{userId}/isadmin/{communityId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserDetailDto>> ReadAsync()
        {
            var response = await _client.GetAsync("api/users");

            return await response.Content.ReadAsAsync<IEnumerable<UserDetailDto>>();
        }

        public async Task<IEnumerable<UserDetailDto>> ReadByEventId(int eventId)
        {
            var response = await _client.GetAsync($"api/users/event/{eventId}");

            return await response.Content.ReadAsAsync<IEnumerable<UserDetailDto>>();
        }

        public async Task<bool> UpdateAsync(UserDetailDto dto)
        {
            var response = await _client.PutAsJsonAsync("api/users", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<UserDetailDto> ReadAsync(UserSearchDto user)
        {
            var response = await _client.PostAsJsonAsync($"api/users/search", user);
            return await response.Content.ReadAsAsync<UserDetailDto>();
        }

        public async Task<IEnumerable<UserDetailDto>> ReadByCommunityId(int communityId)
        {
            var response = await _client.GetAsync($"api/users/community/{communityId}");
            return await response.Content.ReadAsAsync<IEnumerable<UserDetailDto>>();
        }

        public async Task<UserDetailDto> CreateAsync(UserCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync("api/users" ,dto);
            return await response.Content.ReadAsAsync<UserDetailDto>();
        }
    }
}
