using System;

namespace CommunIT.Shared.Portable.DTOs.Comment
{
    public class CommentDetailDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public int ThreadId { get; set; }
    }
}