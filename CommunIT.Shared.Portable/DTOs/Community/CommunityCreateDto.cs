using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Community
{
    public class CommunityCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> BaseTagIds { get; set; }
        public IEnumerable<int> SubTagIds { get; set; }
        public string CreatedBy { get; set; }
    }
}
