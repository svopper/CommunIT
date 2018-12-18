using CommunIT.Shared.Portable.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.Models.Interfaces
{
    public interface ITagRepository
    {
        Task<BaseSubTagDto> ReadTagsForCommunityAsync(int communityId);
        IEnumerable<TagDetailDto> ReadBaseTags();
        IEnumerable<TagDetailDto> ReadSubTags();
    }
}
