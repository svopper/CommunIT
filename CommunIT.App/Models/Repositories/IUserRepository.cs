using CommunIT.Shared.Portable.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface IUserRepository
    {
        Task<UserDetailDto> CreateAsync(UserCreateDto dto);
        Task<IEnumerable<UserDetailDto>> ReadAsync();
        Task<IEnumerable<UserDetailDto>> ReadByEventId(int evnetId);
        Task<UserDetailDto> ReadAsync(UserSearchDto userId);
        Task<bool> UpdateAsync(UserDetailDto dto);
        Task<IEnumerable<UserDetailDto>> ReadByCommunityId(int communityId);
        Task<bool> IsUserAdminInCommunity(string userId, int communityId);
    }
}
