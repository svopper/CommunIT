using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.Shared.DTOs.Tag
{
    public class BaseSubTagDto
    {
        public IEnumerable<TagDetailDto> BaseTags { get; set; }
        public IEnumerable<TagDetailDto> SubTags { get; set; }
    }
}
