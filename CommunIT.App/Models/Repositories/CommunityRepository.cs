using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Community;

namespace CommunIT.App.Models.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly HttpClient _client;

        public CommunityRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CommunityDetailDto> CreateAsync(CommunityCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync("api/community", dto);

            return await response.Content.ReadAsAsync<CommunityDetailDto>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/community/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CommunityDetailDto>> ReadAsync()
        {
            var response = await _client.GetAsync("api/community");

            return await response.Content.ReadAsAsync<IEnumerable<CommunityDetailDto>>();
        }

        public async Task<IEnumerable<CommunityUpdateListViewDto>> ReadAsListViewAsync()
        {
            var response = await _client.GetAsync("api/community");

            return await response.Content.ReadAsAsync<IEnumerable<CommunityUpdateListViewDto>>();
        }

        public async Task<CommunityDetailDto> ReadAsync(int id)
        {
            var response = await _client.GetAsync($"api/community/{id}");

            return await response.Content.ReadAsAsync<CommunityDetailDto>();
        }

        public async Task<IEnumerable<CommunityUpdateListViewDto>> ReadByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"api/community/user/{userId}");

            return await response.Content.ReadAsAsync<IEnumerable<CommunityUpdateListViewDto>>();
        }

        public async Task<IEnumerable<CommunityUpdateListViewDto>> ReadAdministratedByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"api/community/user/{userId}/admin");

            return await response.Content.ReadAsAsync<IEnumerable<CommunityUpdateListViewDto>>();
        }

        public async Task<IEnumerable<CommunityUpdateListViewDto>> ReadFavoritedByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"api/community/user/{userId}/favorite");

            return await response.Content.ReadAsAsync<IReadOnlyCollection<CommunityUpdateListViewDto>>();
        }

        public async Task<IEnumerable<CommunityUpdateListViewDto>> ReadMostPopularAsync()
        {
            var response = await _client.GetAsync($"api/community/popular");

            return await response.Content.ReadAsAsync<IReadOnlyCollection<CommunityUpdateListViewDto>>();
        }

        public async Task<bool> UpdateAsync(CommunityUpdateListViewDto dto)
        {
            var response = await _client.PutAsJsonAsync($"api/community", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> IsUserInCommunityAsync(int communityId, string userId)
        {
            var response = await _client.GetAsync($"api/community/{communityId}/user/{userId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> JoinCommunity(int communityId, string userId)
        {
            var response = await _client.PostAsync($"api/community/{communityId}/adduser/{userId}", null);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UserAddCommunityToFavorites(int communityId, string userId)
        {
            var response = await _client.PostAsync($"api/community/{communityId}/addfavorite/{userId}", null);

            return response.IsSuccessStatusCode;
        }
    }
}
