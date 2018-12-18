using CommunIT.Shared.Portable.DTOs.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface ICommentRepository
    {
        Task<CommentDetailDto> CreateAsync(CommentCreateUpdateDto dto);
        Task<IEnumerable<CommentDetailDto>> ReadByThreadAsync(int threadId);
    }
}
