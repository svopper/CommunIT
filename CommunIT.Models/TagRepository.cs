using CommunIT.Entities.Context;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.Models
{
    public class TagRepository : ITagRepository
    {
        private readonly ICommunITContext _context;

        public TagRepository(ICommunITContext context)
        {
            _context = context;
        }
        
        public IEnumerable<TagDetailDto> ReadBaseTags()
        {
            var tags = _context.BaseTags;

            var tagDtos = tags.Select(t => new TagDetailDto
            {
                Id = t.Id,
                Name = t.Name
            });

            return tagDtos;
        }
        public IEnumerable<TagDetailDto> ReadSubTags()
        {
            var tags = _context.SubTags;

            var tagDtos = tags.Select(t => new TagDetailDto
            {
                Id = t.Id,
                Name = t.Name
            });

            return tagDtos;
        }

        public async Task<BaseSubTagDto> ReadTagsForCommunityAsync(int communityId)
        {
            var community = await _context.Communities.FindAsync(communityId);
            if (community is null)
            {
                return null;
            }
            
            var baseTags =
                _context.CommunityBaseTags
                .Where(c => c.CommunityId == community.Id)
                .Select(b => b.BaseTag)
                .Select(b => new TagDetailDto
            {
                Id = b.Id,
                Name = b.Name
            });
            var subTags =
                _context.CommunitySubTags
                .Where(c => c.CommunityId == community.Id)
                .Select(s => s.SubTag)
                .Select(b => new TagDetailDto
            {
                Id = b.Id,
                Name = b.Name
            });

            return new BaseSubTagDto()
            {
                BaseTags = baseTags,
                SubTags = subTags
            };
        }
    }
}
