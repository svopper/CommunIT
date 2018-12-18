using CommunIT.Shared.Portable.DTOs.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface ICommunityRepository
    {

        Task<CommunityDetailDto> CreateAsync(CommunityCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CommunityDetailDto>> ReadAsync();
        Task<IEnumerable<CommunityUpdateListViewDto>> ReadAsListViewAsync();
        Task<IEnumerable<CommunityUpdateListViewDto>> ReadByUserIdAsync(string userId);
        Task<IEnumerable<CommunityUpdateListViewDto>> ReadAdministratedByUserIdAsync(string userId);
        Task<bool> UserAddCommunityToFavorites(int communityId, string userId);
        Task<IEnumerable<CommunityUpdateListViewDto>> ReadFavoritedByUserIdAsync(string userId);
        Task<IEnumerable<CommunityUpdateListViewDto>> ReadMostPopularAsync();
        Task<CommunityDetailDto> ReadAsync(int id);
        Task<bool> UpdateAsync(CommunityUpdateListViewDto dto);
        Task<bool> IsUserInCommunityAsync(int communityId, string userId);
        Task<bool> JoinCommunity(int communityId, string userId);
    }
}
