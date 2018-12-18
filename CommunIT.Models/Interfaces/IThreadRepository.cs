using System.Collections.Generic;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.Thread;

namespace CommunIT.Models.Interfaces
{
    public interface IThreadRepository
    {
        Task<ThreadDetailDto> CreateAsync(ThreadCreateDto dto);
        Task<ThreadDetailDto> ReadAsync(int id);
        IEnumerable<ThreadDetailDto> ReadByForumId(int forumId);
        Task<bool> UpdateAsync(ThreadUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}