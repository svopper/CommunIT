using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Thread
{
    public class ThreadCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int ForumId { get; set; }
    }
}
