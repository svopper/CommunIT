namespace CommunIT.Shared.Portable.DTOs.Comment
{
    public class CommentCreateUpdateDto
    {
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public string UserId { get; set; }
    }
}