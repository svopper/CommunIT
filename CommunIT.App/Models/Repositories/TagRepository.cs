using CommunIT.Shared.Portable.DTOs.Tag;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly HttpClient _client;
        public TagRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<IEnumerable<TagDetailDto>> ReadBaseTags()
        {
            var response = await _client.GetAsync("api/tags/basetags");

            return await response.Content.ReadAsAsync<IEnumerable<TagDetailDto>>();
        }

        public async Task<IEnumerable<TagDetailDto>> ReadSubTags()
        {
            var response = await _client.GetAsync("api/tags/subtags");

            return await response.Content.ReadAsAsync<IEnumerable<TagDetailDto>>();
        }

        public async Task<BaseSubTagDto> ReadTagsForCommunityAsync(int communityId)
        {
            var response = await _client.GetAsync($"api/tags/community/{communityId}");
            return await response.Content.ReadAsAsync<BaseSubTagDto>();
        }
    }
}
