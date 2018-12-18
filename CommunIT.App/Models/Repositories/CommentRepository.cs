using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Comment;

namespace CommunIT.App.Models.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly HttpClient _client;

        public CommentRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CommentDetailDto> CreateAsync(CommentCreateUpdateDto dto)
        {
            var response = await _client.PostAsJsonAsync("api/comments", dto);

            return await response.Content.ReadAsAsync<CommentDetailDto>();
        }

        public async Task<IEnumerable<CommentDetailDto>> ReadByThreadAsync(int threadId)
        {
            var response = await _client.GetAsync($"api/comments/thread/{threadId}");

            return await response.Content.ReadAsAsync<IEnumerable<CommentDetailDto>>();
        }
    }
}
