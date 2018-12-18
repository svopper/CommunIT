using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Tag
{
    public class BaseSubTagDto
    {
        public IEnumerable<TagDetailDto> BaseTags { get; set; }
        public IEnumerable<TagDetailDto> SubTags { get; set; }
    }
}
