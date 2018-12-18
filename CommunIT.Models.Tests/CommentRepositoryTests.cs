using CommunIT.Entities;
using CommunIT.Shared.Portable.DTOs.Comment;
using CommunIT.Shared.Portable.DTOs.Thread;
using CommunIT.Shared.Portable.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class CommentRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_creates_and_returns_a_comment_when_given_a_valid_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);
                context.Users.Add(new User { Username = "user@hello.dk", DisplayName = "Mememaster"});
                context.Communities.Add(new Community { Id = 99, Name="Test", Description = "Test"});
                context.Forums.Add(new Forum { Id = 100, Created=DateTime.Now, Name="Name", CommunityId = 99});
                context.Threads.Add(new Thread {Id = 1337, Title = "Title", Content="Somethin", ForumId = 100});
                await context.SaveChangesAsync();
                var commentDto = new CommentCreateUpdateDto
                {
                    Content = "Nice find",
                    ThreadId = 1337,
                    UserId = "user@hello.dk",
                };

                var result = await repository.CreateAsync(commentDto);

                Assert.NotNull(result);
                Assert.Equal(commentDto.Content, result.Content);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_a_comment_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);
                context.Users.Add(new User { Username = "user@hello.dk", DisplayName = "Mememaster" });
                context.Communities.Add(new Community { Id = 99, Name = "Test", Description = "Test" });
                context.Forums.Add(new Forum { Id = 100, Created = DateTime.Now, Name = "Name", CommunityId = 99 });
                context.Threads.Add(new Thread { Id = 1337, Title = "Title", Content = "Somethin", ForumId = 100 });
                context.Comments.Add(new Comment {Id = 23, Content = "dank.", ThreadId = 1337, UserId = "user@hello.dk"});
                await context.SaveChangesAsync();
                
                var result = await repository.ReadAsync(23);

                Assert.NotNull(result);
                Assert.Equal("dank.", result.Content);
                Assert.True(result.Id > 0);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_null_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);

                var result = await repository.ReadAsync(123);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_true_when_given_an_existing_comment()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);

                context.Users.Add(new User { Username = "user@hello.dk", DisplayName = "Mememaster" });
                context.Communities.Add(new Community { Id = 99, Name = "Test", Description = "Test" });
                context.Forums.Add(new Forum { Id = 100, Created = DateTime.Now, Name = "Name", CommunityId = 99 });
                context.Threads.Add(new Thread { Id = 1337, Title = "Title", Content = "Somethin", ForumId = 100 });
                context.Comments.Add(new Comment { Id = 23, Content = "dank.", ThreadId = 1337, UserId = "user@hello.dk" });
                await context.SaveChangesAsync();

                var updateDto = new CommentCreateUpdateDto
                {
                    Content = "This is not cute at all!"
                };
                var result = await repository.UpdateAsync(23, updateDto);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_false_when_given_an_non_existing_comment()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);

                var newCommentDto = new CommentCreateUpdateDto
                {
                    Content = "nice"
                };
                var result = await repository.UpdateAsync(123, newCommentDto);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_true_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);

                context.Users.Add(new User { Username = "user@hello.dk", DisplayName = "Mememaster" });
                context.Communities.Add(new Community { Id = 99, Name = "Test", Description = "Test" });
                context.Forums.Add(new Forum { Id = 100, Created = DateTime.Now, Name = "Name", CommunityId = 99 });
                context.Threads.Add(new Thread { Id = 1337, Title = "Title", Content = "Somethin", ForumId = 100 });
                context.Comments.Add(new Comment { Id = 23, Content = "dank.", ThreadId = 1337, UserId = "user@hello.dk" });
                await context.SaveChangesAsync();
                
                var result = await repository.DeleteAsync(23);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_false_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommentRepository(context);

                var result = await repository.DeleteAsync(123);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReadByUserId_returns_all_comments_by_user()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = await context.Users.AddAsync(new User {Username = "test@itu.dk"});
                var commuity = await context.Communities.AddAsync(new Community { Name = "test", Description = "test" });
                var forum = await context.Forums.AddAsync(new Forum { Name = "test", Created = DateTime.Now, Description = "test", CommunityId = commuity.Entity.Id});
                var thread = await context.Threads.AddAsync(new Thread {Title = "test", ForumId = forum.Entity.Id, Created = DateTime.Now, Content ="test", UserId = "kols@itu.dk"});
                var comment = await context.Comments.AddAsync(new Comment { Content = "nice", Created = DateTime.Now, ThreadId = thread.Entity.Id, UserId = "test@itu.dk" });
                await context.SaveChangesAsync();

                var repo = new CommentRepository(context);

                var userComments = repo.ReadByUserId("test@itu.dk");

                Assert.Single(userComments);
                Assert.Equal(comment.Entity.Content, userComments.Single().Content);
            }
        }

        [Fact]
        public async Task ReadByUserId_given_nonexisting_user_returns_empty_list()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repo = new CommentRepository(context);

                var userComments = repo.ReadByUserId("emfi@itu.dk");

                Assert.Empty(userComments);
            }
        }

        [Fact]
        public async Task ReadByThreadId_returns_all_comments_in_thread()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var commuity = await context.Communities.AddAsync(new Community { Name = "test", Description = "test" });
                var forum = await context.Forums.AddAsync(new Forum { Name = "test", Created = DateTime.Now, Description = "test", CommunityId = commuity.Entity.Id });
                var thread = await context.Threads.AddAsync(new Thread { Title = "test", ForumId = forum.Entity.Id, Created = DateTime.Now, Content = "test", UserId = "kols@itu.dk" });
                var comment = await context.Comments.AddAsync(new Comment { Content = "nice", Created = DateTime.Now, ThreadId = thread.Entity.Id, UserId = "kols@itu.dk" });
                await context.SaveChangesAsync();

                var repo = new CommentRepository(context);

                var threadComments = repo.ReadByThreadId(thread.Entity.Id);

                Assert.Single(threadComments);
                Assert.Equal(comment.Entity.Content, threadComments.Single().Content);
            }
        }

        [Fact]
        public async Task ReadByThreadId_given_nonexisting_thread_returns_empty_list()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repo = new CommentRepository(context);

                var userComments = repo.ReadByThreadId(1468);

                Assert.Empty(userComments);
            }
        }
    }
}
