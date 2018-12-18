using CommunIT.Entities;
using CommunIT.Entities.Context;
using CommunIT.Shared.Portable.DTOs.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;

namespace CommunIT.Models
{
    public class ForumRepository : IForumRepository
    {
        private readonly ICommunITContext _context;

        public ForumRepository(ICommunITContext context)
        {
            _context = context;
        }

        public async Task<ForumDetailDto> CreateAsync(ForumCreateDto dto)
        {
            var community = await _context.Communities.FindAsync(dto.CommunityId);

            if (community is null)
            {
                return null;
            }
            
            var forum = new Forum
            {
                Name = dto.Name,
                Description = dto.Description,
                Created = DateTime.Now,
                CommunityId = dto.CommunityId
            };

            var created = await _context.Forums.AddAsync(forum);
            await _context.SaveChangesAsync();

            return await ReadAsync(created.Entity.Id);
        }

        public async Task<ForumDetailDto> ReadAsync(int id)
        {
            var forum = await _context.Forums.FindAsync(id);

            if (forum is null)
            {
                return null;
            }

            return new ForumDetailDto
            {
                Id = forum.Id,
                Name = forum.Name,
                Description = forum.Description,
                Created = forum.Created
            };
        }

        public async Task<bool> UpdateAsync(ForumUpdateDto dto)
        {
            var forum = await _context.Forums.FindAsync(dto.Id);

            if (forum is null)
            {
                return false;
            }

            forum.Name = dto.Name;
            forum.Description = dto.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var forum = await _context.Forums.FindAsync(id);

            if (forum is null)
            {
                return false;
            }

            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();

            return true;
        }

        public IEnumerable<ForumDetailDto> ReadByCommunityId(int communityId)
        {
            var forums = _context.Forums.Where(f => f.CommunityId == communityId);

            return forums.Select(f => new ForumDetailDto()
            {
                Id = f.Id,
                Name = f.Name,
                Created = f.Created,
                Description = f.Description
            });
        }
    }
}
