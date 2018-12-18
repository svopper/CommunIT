using System.Collections.Generic;
using CommunIT.Shared.Portable.DTOs.Comment;

namespace CommunIT.Shared.Portable.DTOs.User
{
    public class UserCommentsDto
    {
        public IEnumerable<CommentDetailDto> Comments { get; set; }
    }
}