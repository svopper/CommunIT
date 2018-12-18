using CommunIT.Entities;
using CommunIT.Models.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Entities.Context;
using CommunIT.Shared.Portable.DTOs.User;
using System.Collections.Generic;

namespace CommunIT.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly ICommunITContext _context;

        public UserRepository(ICommunITContext context)
        {
            _context = context;
        }

        public async Task<UserDetailDto> CreateAsync(UserCreateDto dto)
        {
            if (string.IsNullOrEmpty(dto.DisplayName) || string.IsNullOrWhiteSpace(dto.DisplayName))
            {
                return null;
            }
            
            var user = new User
            {
                Username = dto.Username,
                Bio = dto.Bio,
                DisplayName = dto.DisplayName,
                University = dto.University,
                Created = DateTime.Now
            };

            var created = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return await ReadAsync(created.Entity.Username);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDetailDto> ReadAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return null;
            }

            var userDto = new UserDetailDto
            {
                Username = user.Username,
                DisplayName = user.DisplayName,
                University = user.University,
                Bio = user.Bio,
                Created = user.Created
            };

            return userDto;
        }
        
        public IQueryable<UserDetailDto> Read()
        {
            var users = _context.Users;
            
            return users.Select(u => new UserDetailDto
            {
                Username = u.Username,
                DisplayName = u.DisplayName,
                University = u.University,
                Bio = u.Bio,
                Created = u.Created
            });
        }

        public async Task<bool> UpdateAsync(string id, UserCreateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return false;
            }

            user.DisplayName = string.IsNullOrEmpty(dto.DisplayName) ? user.Username : dto.DisplayName;
            user.University = string.IsNullOrEmpty(dto.University) ? user.University : dto.University;
            user.Bio = string.IsNullOrEmpty(dto.Bio) ? user.Bio : dto.Bio;

            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<UserDetailDto> ReadUsersInEvent(int eventId)
        {
            var users = _context.EventUsers.Where(e => e.EventId == eventId).Select(eu => eu.User);

            return users.Select(u => new UserDetailDto
            {
                Username = u.Username,
                DisplayName = u.DisplayName,
                University = u.University,
                Bio = u.Bio,
                Created = u.Created
            });
        }

        public bool IsUserAdminInCommunity(string userId, int communityId)
        {
            var communityUser = _context.CommunityUsers.Where(cu => cu.UserId == userId && cu.CommunityId == communityId).FirstOrDefault();

            if (communityUser is null)
            {
                return false;
            }

            return communityUser.IsAdmin;
        }

        public IQueryable<UserDetailDto> ReadUsersInCommunity(int communityId)
        {
            var userList = _context.CommunityUsers.Where(u => u.CommunityId == communityId).Select(c => c.User);
            return userList.Select(u => new UserDetailDto
            {
                Username = u.Username,
                DisplayName = u.DisplayName
            });
        }
    }
}
