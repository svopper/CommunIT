using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Thread
{
    public class ThreadDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime Created { get; set; }
    }
}
