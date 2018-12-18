using CommunIT.Shared.Portable.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface ITagRepository
    {
        Task<BaseSubTagDto> ReadTagsForCommunityAsync(int communityId);
        Task<IEnumerable<TagDetailDto>> ReadBaseTags();
        Task<IEnumerable<TagDetailDto>> ReadSubTags();
    }
}
