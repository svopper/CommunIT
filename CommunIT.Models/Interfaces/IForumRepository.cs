using System.Collections.Generic;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Forum;

namespace CommunIT.Models.Interfaces
{
    public interface IForumRepository
    {
        Task<ForumDetailDto> CreateAsync(ForumCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ForumDetailDto> ReadAsync(int id);
        IEnumerable<ForumDetailDto> ReadByCommunityId(int communityId);
        Task<bool> UpdateAsync(ForumUpdateDto dto);
    }
}