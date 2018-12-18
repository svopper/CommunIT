using CommunIT.Entities;
using CommunIT.Entities.Context;
using CommunIT.Shared.Portable.DTOs.Community;
using CommunIT.Shared.Portable.DTOs.Event;
using CommunIT.Shared.Portable.DTOs.Forum;
using System;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.Models
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ICommunITContext _context;

        public CommunityRepository(ICommunITContext context)
        {
            _context = context;
        }

        public async Task<CommunityDetailDto> CreateAsync(CommunityCreateDto dto)
        {
            var community = new Community
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var createdCommunity = await _context.Communities.AddAsync(community);
            
            var baseTags = dto.BaseTagIds.Select(b => new CommunityBaseTag
            {
                CommunityId = createdCommunity.Entity.Id,
                BaseTagId = b
            });
            
            var subTags = dto.SubTagIds.Select(s => new CommunitySubTag
            {
                CommunityId = createdCommunity.Entity.Id,
                SubTagId = s
            });

            foreach (var baseTag in baseTags)
            {
                await _context.CommunityBaseTags.AddAsync(baseTag);
            }
            
            foreach (var subTag in subTags)
            {
                await _context.CommunitySubTags.AddAsync(subTag);
            }

            var communityUser = new CommunityUser
            {
                CommunityId = createdCommunity.Entity.Id,
                UserId = dto.CreatedBy,
                IsAdmin = true
            };
            await _context.CommunityUsers.AddAsync(communityUser);
            
            await _context.SaveChangesAsync();
            
            return await ReadAsync(createdCommunity.Entity.Id);
        }

        public async Task<CommunityDetailDto> ReadAsync(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if (community is null)
            {
                return null;
            }

            var events = community.Events?.Select(e => new EventUpdateDetailDto
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Description = e.Description
            });

            var forums = community.Forums?.Select(f => new ForumDetailDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Created = f.Created
            });

            return new CommunityDetailDto
            {
                Id = community.Id,
                Name = community.Name,
                Description = community.Description,
                Events = events,
                Forums = forums
            };
        }

        public async Task<bool> UpdateAsync(CommunityUpdateListViewDto dto)
        {
            var community = await _context.Communities.FindAsync(dto.Id);

            if (community is null)
            {
                return false;
            }

            community.Name = dto.Name;
            community.Description = dto.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if (community is null)
            {
                return false;
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> AddUserToCommunity(int communityId, string userId)
        {
            var community = await _context.Communities.FindAsync(communityId);
            var user = await _context.Users.FindAsync(userId);

            if (community is null || user is null)
            {
                return false;
            }

            var communityUser = new CommunityUser
            {
                UserId = userId,
                CommunityId = communityId,
                DateJoined = DateTime.Now,
                IsAdmin = false,
                IsFavorite = false
            };

            await _context.CommunityUsers.AddAsync(communityUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public IQueryable<CommunityUpdateListViewDto> ReadByUserId(string userId)
        {
            var communities = _context.Communities
                .Where(c => c.CommunityUsers.Any(cu => cu.UserId == userId));

            return communities.Select(c => new CommunityUpdateListViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public IQueryable<CommunityUpdateListViewDto> ReadAdministratedByUserId(string userId)
        {
            var communities = _context.Communities
                .Where(c => c.CommunityUsers.Any(cu => cu.UserId == userId && cu.IsAdmin == true));

            return communities.Select(c => new CommunityUpdateListViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public IQueryable<CommunityUpdateListViewDto> ReadFavoritedByUserId(string userId)
        {
            var communities = _context.Communities
                .Where(c => c.CommunityUsers.Any(cu => cu.UserId == userId && cu.IsFavorite == true));

            return communities.Select(c => new CommunityUpdateListViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public IQueryable<CommunityUpdateListViewDto> ReadMostPopular()
        {
            var communities = _context.Communities
                .OrderByDescending(c => c.CommunityUsers.Count).Take(5);

            return communities.Select(c => new CommunityUpdateListViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public IQueryable<CommunityUpdateListViewDto> Read()
        {
            var communities = _context.Communities;

            return communities.Select(c => new CommunityUpdateListViewDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<bool> IsUserInCommunityAsync(int communityId, string userId)
        {
            var result = await _context.CommunityUsers.Where(cu => cu.CommunityId == communityId && cu.UserId == userId).FirstOrDefaultAsync();

            if (result is null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddCommunityToUserFavorites(int communityId, string userId)
        {
            var community = await _context.Communities.FindAsync(communityId);
            var user = await _context.Users.FindAsync(userId);
            
            if (community is null ||  user is null)
            {
                return false;
            }

            var communityUser = _context.CommunityUsers.Where(cu => cu.CommunityId == communityId && cu.UserId == userId).Single();
            var foundCommunityUser = _context.CommunityUsers.Update(communityUser);

            foundCommunityUser.Entity.IsFavorite = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
