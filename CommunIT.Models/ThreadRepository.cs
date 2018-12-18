using CommunIT.Entities;
using CommunIT.Entities.Context;
using CommunIT.Shared.Portable.DTOs.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Models.Interfaces;

namespace CommunIT.Models
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly ICommunITContext _context;

        public ThreadRepository(ICommunITContext context)
        {
            _context = context;
        }

        public async Task<ThreadDetailDto> CreateAsync(ThreadCreateDto dto)
        {
            var forum = await _context.Forums.FindAsync(dto.ForumId);
            var user = await _context.Users.FindAsync(dto.UserId);

            if (forum is null || user is null)
            {
                return null;
            }

            var thread = new Thread
            {
                Title = dto.Title,
                Content = dto.Content,
                Created = DateTime.Now,
                UserId = dto.UserId,
                ForumId = dto.ForumId
            };

            var created = await _context.Threads.AddAsync(thread);
            await _context.SaveChangesAsync();

            return await ReadAsync(created.Entity.Id);
        }

        public async Task<ThreadDetailDto> ReadAsync(int id)
        {
            var thread = await _context.Threads.FindAsync(id);

            if (thread is null)
            {
                return null;
            }

            return new ThreadDetailDto
            {
                Id = thread.Id,
                Title = thread.Title,
                Content = thread.Content,
                Created = thread.Created
            };
        }

        public async Task<bool> UpdateAsync(ThreadUpdateDto dto)
        {
            var thread = await _context.Threads.FindAsync(dto.Id);

            if (thread is null)
            {
                return false;
            }

            thread.Title = dto.Title;
            thread.Content = dto.Content;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var thread = await _context.Threads.FindAsync(id);

            if (thread is null)
            {
                return false;
            }

            _context.Threads.Remove(thread);
            await _context.SaveChangesAsync();

            return true;
        }

        public IEnumerable<ThreadDetailDto> ReadByForumId(int forumId)
        {
            var threads = _context.Threads.Where(t => t.ForumId == forumId);

            return threads.Select(t => new ThreadDetailDto
            {
                Id = t.Id,
                Title = t.Title,
                Created = t.Created,
                Content = t.Content,
                CreatedByUserId = t.UserId
            });
        }
    }
}
