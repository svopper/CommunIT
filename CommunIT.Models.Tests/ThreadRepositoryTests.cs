using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunIT.Entities;
using CommunIT.Entities.Context;
using CommunIT.Shared.Portable.DTOs.Forum;
using CommunIT.Shared.Portable.DTOs.Thread;
using CommunIT.Shared.Portable.DTOs.User;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class ThreadRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_creates_and_returns_a_thread_when_given_a_valid_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);
                var forum = await CreateTestForumAsync(context);
                var user = await CreateTestUserAsync(context);

                var threadCreateDto = new ThreadCreateDto
                {
                    Title = "Bunnies are fluffy",
                    Content = "OMG the are so fluffy",
                    UserId = user.Username,
                    ForumId = forum.Id
                };

                var result = await repository.CreateAsync(threadCreateDto);

                Assert.NotNull(result);
                Assert.Equal(threadCreateDto.Title, result.Title);
                Assert.Equal(threadCreateDto.Content, result.Content);
            }
        }

        [Fact]
        public async Task CreateAsync_returns_null_when_the_given_forum_does_not_exist()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);
                var user = await CreateTestUserAsync(context);

                var threadCreateDto = new ThreadCreateDto
                {
                    Title = "Bunnies are fluffy",
                    Content = "OMG the are so fluffy",
                    UserId = user.Username,
                    ForumId = 123
                };

                var result = await repository.CreateAsync(threadCreateDto);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task CreateAsync_returns_null_when_the_given_user_does_not_exist()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);
                var forum = await CreateTestForumAsync(context);

                var threadCreateDto = new ThreadCreateDto
                {
                    Title = "Bunnies are fluffy",
                    Content = "OMG the are so fluffy",
                    UserId = "mail@test.com",
                    ForumId = forum.Id
                };

                var result = await repository.CreateAsync(threadCreateDto);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_a_thread_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);
                var forum = await CreateTestForumAsync(context);
                var user = await CreateTestUserAsync(context);

                var threadCreateDto = new ThreadCreateDto
                {
                    Title = "Bunnies are fluffy",
                    Content = "OMG the are so fluffy",
                    UserId = user.Username,
                    ForumId = forum.Id
                };

                var foundForum = await repository.CreateAsync(threadCreateDto);
                var result = await repository.ReadAsync(foundForum.Id);

                Assert.NotNull(result);
                Assert.Equal(threadCreateDto.Title, result.Title);
                Assert.Equal(threadCreateDto.Content, result.Content);
                Assert.True(result.Id > 0);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_null_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);

                var result = await repository.ReadAsync(123);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_true_when_given_an_existing_forum()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);
                var forum = await CreateTestForumAsync(context);
                var user = await CreateTestUserAsync(context);

                var threadCreateDto = new ThreadCreateDto
                {
                    Title = "Bunnies are fluffy",
                    Content = "OMG the are so fluffy",
                    UserId = user.Username,
                    ForumId = forum.Id
                };
                var foundThread = await repository.CreateAsync(threadCreateDto);

                var threadUpdateDto = new ThreadUpdateDto
                {
                    Id = foundThread.Id,
                    Title = "New title",
                    Content = "New content"
                };
                var result = await repository.UpdateAsync(threadUpdateDto);

                var updatedThread = await repository.ReadAsync(threadUpdateDto.Id);
                Assert.True(result);
                Assert.Equal(threadUpdateDto.Title, updatedThread.Title);
                Assert.Equal(threadUpdateDto.Content, updatedThread.Content);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_false_when_given_non_existing_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);

                var threadUpdateDto = new ThreadUpdateDto
                {
                    Id = 123,
                    Title = "New title",
                    Content = "New content"
                };
                var result = await repository.UpdateAsync(threadUpdateDto);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_true_when_given_a_valid_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);
                var forum = await CreateTestForumAsync(context);
                var user = await CreateTestUserAsync(context);

                var threadCreateDto = new ThreadCreateDto
                {
                    Title = "Bunnies are fluffy",
                    Content = "OMG the are so fluffy",
                    UserId = user.Username,
                    ForumId = forum.Id
                };
                var foundThread = await repository.CreateAsync(threadCreateDto);

                var result = await repository.DeleteAsync(foundThread.Id);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_false_when_given_a_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);

                var result = await repository.DeleteAsync(123);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReadByForumId_returns_threads_in_forum()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var commuity = await context.Communities.AddAsync(new Community { Name = "test", Description = "test" });
                var forum = await context.Forums.AddAsync(new Forum { Name = "test", Created = DateTime.Now, Description = "test", CommunityId = commuity.Entity.Id});
                var thread = await context.Threads.AddAsync(new Thread {Title = "test", ForumId = forum.Entity.Id, Created = DateTime.Now, Content ="test", UserId = "kols@itu.dk"});

                await context.SaveChangesAsync();

                var repository = new ThreadRepository(context);

                var threads = repository.ReadByForumId(forum.Entity.Id);

                Assert.Single(threads);
                Assert.Equal(thread.Entity.Title, threads.Single().Title);
            }
        }

        [Fact]
        public async Task ReadByForumId_empty_list_when_given_invalid_forum_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ThreadRepository(context);

                var threads = repository.ReadByForumId(5457);
                
                Assert.Empty(threads);
            }
        }

        #region private methods
        private async Task<ForumDetailDto> CreateTestForumAsync(ICommunITContext context)
        {
            var community = await TestHelper.CreateTestCommunityAsync(context);
            var repository = new ForumRepository(context);

            var forumDto = new ForumCreateDto
            {
                Name = "Bunnies are cute",
                Description = "OMG they are so cute!",
                CommunityId = community.Id
            };

            return await repository.CreateAsync(forumDto);
        }

        private async Task<UserDetailDto> CreateTestUserAsync(ICommunITContext context)
        {
            var userRepository = new UserRepository(context);

            var userDto = new UserCreateDto
            {
                Username = "mail@test.com",
                DisplayName = "longjohn",
                Bio = "I like cookies and cake"
            };

            return await userRepository.CreateAsync(userDto);
        }
        #endregion
    }
}
