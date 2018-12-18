using CommunIT.Shared.Portable.DTOs.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface IForumRepository
    {
        Task<ForumDetailDto> CreateAsync(ForumCreateDto dto);
        Task<IEnumerable<ForumDetailDto>> ReadByCommunityId(int communityId);
    }
}
