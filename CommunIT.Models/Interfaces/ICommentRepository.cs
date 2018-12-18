using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Comment;
using CommunIT.Shared.Portable.DTOs.User;

namespace CommunIT.Models.Interfaces
{
    public interface ICommentRepository
    {
        Task<CommentDetailDto> CreateAsync(CommentCreateUpdateDto dto);
        Task<CommentDetailDto> ReadAsync(int id);
        IEnumerable<CommentDetailDto> ReadByUserId(string userId);
        IEnumerable<CommentDetailDto> ReadByThreadId(int threadId);
        Task<bool> UpdateAsync(int id, CommentCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}