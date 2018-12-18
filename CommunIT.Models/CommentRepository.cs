using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using CommunIT.Entities;
using CommunIT.Entities.Context;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Comment;
using CommunIT.Shared.Portable.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.Models
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ICommunITContext _context;

        public CommentRepository(ICommunITContext context)
        {
            _context = context;
        }
        
        public async Task<CommentDetailDto> CreateAsync(CommentCreateUpdateDto dto)
        {
            var comment = new Comment
            {
                Created = DateTime.Now,
                Content = dto.Content,
                ThreadId = dto.ThreadId,
                UserId = dto.UserId
            };

            var created = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return await ReadAsync(created.Entity.Id);

        }

        public async Task<CommentDetailDto> ReadAsync(int id)
        {
            var comment = await _context.Comments.Where(c => c.Id == id).Include(c => c.User).SingleOrDefaultAsync();

            if (comment == null)
            {
                return null;
            }

            var commentDetailDto = new CommentDetailDto
            {
                Id = comment.Id,
                Content = comment.Content,
                Created = comment.Created,
                UserDisplayName = comment.User.DisplayName
            };

            return commentDetailDto;
        }

        public IEnumerable<CommentDetailDto> ReadByUserId(string userId)
        {
            var comments = _context.Comments.Where(c => c.UserId == userId);

            return comments.Select(c => new CommentDetailDto
            {
                Id = c.Id,
                Created = c.Created,
                Content = c.Content
            });
        }

        public async Task<bool> UpdateAsync(int id, CommentCreateUpdateDto dto)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment is null)
            {
                return false;
            }

            comment.Content = dto.Content;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();

            return true;
        }

        public IEnumerable<CommentDetailDto> ReadByThreadId(int threadId)
        {
            var comments = _context.Comments.Where(c => c.ThreadId == threadId);
            
            return comments.Select(c => new CommentDetailDto
            {
                Id = c.Id,
                Created = c.Created,
                Content = c.Content,
                UserId = c.UserId,
                UserDisplayName = c.User.DisplayName
                
            });
        }
    }
}