using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Forum
{
    public class ForumCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CommunityId { get; set; }
    }
}
