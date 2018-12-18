using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Forum
{
    public class ForumDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
