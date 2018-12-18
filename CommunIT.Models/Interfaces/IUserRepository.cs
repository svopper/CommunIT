using CommunIT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.User;

namespace CommunIT.Models.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDetailDto> CreateAsync(UserCreateDto dto);
        Task<UserDetailDto> ReadAsync(string id);
        IQueryable<UserDetailDto> Read();
        IQueryable<UserDetailDto> ReadUsersInEvent(int eventId);
        IQueryable<UserDetailDto> ReadUsersInCommunity(int communityId);
        Task<bool> UpdateAsync(string id, UserCreateDto dto);
        Task<bool> DeleteAsync(string id);
        bool IsUserAdminInCommunity(string userId, int communityId);
    }
}