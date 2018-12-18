using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Community;

namespace CommunIT.Models.Interfaces
{
    public interface ICommunityRepository
    {
        Task<CommunityDetailDto> CreateAsync(CommunityCreateDto dto);
        Task<bool> DeleteAsync(int id);
        IQueryable<CommunityUpdateListViewDto> Read();
        IQueryable<CommunityUpdateListViewDto> ReadMostPopular();
        IQueryable<CommunityUpdateListViewDto> ReadByUserId(string userId);
        IQueryable<CommunityUpdateListViewDto> ReadAdministratedByUserId(string userId);
        IQueryable<CommunityUpdateListViewDto> ReadFavoritedByUserId(string userId);
        Task<bool> AddCommunityToUserFavorites(int communityId, string userId);
        Task<CommunityDetailDto> ReadAsync(int id);
        Task<bool> UpdateAsync(CommunityUpdateListViewDto dto);
        Task<bool> AddUserToCommunity(int communityId, string userId);
        Task<bool> IsUserInCommunityAsync(int communityId, string userId);
    }
}