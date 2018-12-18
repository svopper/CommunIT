using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Thread;

namespace CommunIT.App.Models.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly HttpClient _client;

        public ThreadRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<ThreadDetailDto> CreateAsync(ThreadCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync($"api/threads",dto);

            return await response.Content.ReadAsAsync<ThreadDetailDto>();
        }

        public async Task<bool> DeleteAsync(int threadId)
        {
            var response = await _client.DeleteAsync($"api/threads/{threadId}");

            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<IEnumerable<ThreadDetailDto>> ReadByForumId(int forumId)
        {
            var response = await _client.GetAsync($"api/threads/forum/{forumId}");

            return await response.Content.ReadAsAsync<IEnumerable<ThreadDetailDto>>();
        }
    }
}
