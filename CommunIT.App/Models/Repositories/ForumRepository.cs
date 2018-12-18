using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Forum;

namespace CommunIT.App.Models.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private readonly HttpClient _client;

        public ForumRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<ForumDetailDto> CreateAsync(ForumCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync($"api/forums", dto);

            return await response.Content.ReadAsAsync<ForumDetailDto>();
        }

        public async Task<IEnumerable<ForumDetailDto>> ReadByCommunityId(int communityId)
        {
            var response = await _client.GetAsync($"api/forums/community/{communityId}");

            return await response.Content.ReadAsAsync<IEnumerable<ForumDetailDto>>();
        }
    }
}
