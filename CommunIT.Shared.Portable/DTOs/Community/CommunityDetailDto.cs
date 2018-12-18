    using CommunIT.Shared.Portable.DTOs.Event;
using CommunIT.Shared.Portable.DTOs.Forum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Community
{
    public class CommunityDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<EventUpdateDetailDto> Events { get; set; }
        public IEnumerable<ForumDetailDto> Forums { get; set; }
    }
}
