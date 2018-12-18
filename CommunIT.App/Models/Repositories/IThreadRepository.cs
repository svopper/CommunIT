using CommunIT.Shared.Portable.DTOs.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunIT.App.Models.Repositories
{
    public interface IThreadRepository
    {
        Task<ThreadDetailDto> CreateAsync(ThreadCreateDto dto);
        Task<IEnumerable<ThreadDetailDto>> ReadByForumId(int forumId);
        Task<bool> DeleteAsync(int threadId);
    }
}
