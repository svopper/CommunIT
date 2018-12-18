using CommunIT.Entities;
using CommunIT.Shared.Portable.DTOs.Forum;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class ForumRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_creates_and_returns_a_forum_when_given_a_valid_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var repository = new ForumRepository(context);

                var forumDto = new ForumCreateDto
                {
                    Name = "Bunnies are cute",
                    Description ="OMG they are so cute!",
                    CommunityId = community.Id
                };

                var result = await repository.CreateAsync(forumDto);

                Assert.NotNull(result);
                Assert.Equal(forumDto.Name, result.Name);
                Assert.Equal(forumDto.Description, result.Description);
                Assert.True(result.Id > 0);
            }
        }

        [Fact]
        public async Task CreateAsync_returns_null_when_the_given_forum_does_not_have_a_valid_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ForumRepository(context);

                var forumDto = new ForumCreateDto
                {
                    Name = "Bunnies are cute",
                    Description = "OMG they are so cute!",
                    CommunityId = 123
                };

                var result = await repository.CreateAsync(forumDto);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_a_forum_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var repository = new ForumRepository(context);

                var forumDto = new ForumCreateDto
                {
                    Name = "Bunnies are cute",
                    Description = "OMG they are so cute!",
                    CommunityId = community.Id
                };

                var foundForum = await repository.CreateAsync(forumDto);
                var result = await repository.ReadAsync(foundForum.Id);

                Assert.NotNull(result);
                Assert.Equal(forumDto.Name, result.Name);
                Assert.Equal(forumDto.Description, result.Description);
                Assert.True(result.Id > 0);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_null_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ForumRepository(context);

                var result = await repository.ReadAsync(123);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_true_when_given_an_existing_forum()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var repository = new ForumRepository(context);

                var forumCreateDto = new ForumCreateDto
                {
                    Name = "Bunnies are cute",
                    Description = "OMG they are so cute!",
                    CommunityId = community.Id
                };
                var foundForum = await repository.CreateAsync(forumCreateDto);

                var forumUpdateDto = new ForumUpdateDto
                {
                    Id = foundForum.Id,
                    Name = "New name",
                    Description = "New description"
                };
                var result = await repository.UpdateAsync(forumUpdateDto);

                var updatedForum = await repository.ReadAsync(forumUpdateDto.Id);
                Assert.True(result);
                Assert.Equal(forumUpdateDto.Name, updatedForum.Name);
                Assert.Equal(forumUpdateDto.Description, updatedForum.Description);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_false_when_given_an_non_existing_forum()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ForumRepository(context);

                var forumUpdateDto = new ForumUpdateDto
                {
                    Id = 123,
                    Name = "New name",
                    Description = "New description"
                };
                var result = await repository.UpdateAsync(forumUpdateDto);

                Assert.False(result);
            }
        }
        
        [Fact]
        public async Task DeleteAsync_returns_true_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var repository = new ForumRepository(context);

                var forumCreateDto = new ForumCreateDto
                {
                    Name = "Bunnies are cute",
                    Description = "OMG they are so cute!",
                    CommunityId = community.Id
                };

                var foundForum = await repository.CreateAsync(forumCreateDto);
                var result = await repository.DeleteAsync(foundForum.Id);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_false_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ForumRepository(context);

                var result = await repository.DeleteAsync(123);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReadByCommunityId_returns_forums_in_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await context.Communities.AddAsync(new Community { Name = "test", Description = "test" });
                var forum = await context.Forums.AddAsync(new Forum { Name = "test", Created = DateTime.Now, Description = "test", CommunityId = community.Entity.Id });

                await context.SaveChangesAsync();

                var repository = new ForumRepository(context);

                var forums = repository.ReadByCommunityId(community.Entity.Id);

                Assert.Single(forums);
                Assert.Equal(forum.Entity.Name, forums.Single().Name);
            }
        }

        [Fact]
        public async Task ReadByCommunityId_returns_empty_list_given_invalid_community_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new ForumRepository(context);

                var forums = repository.ReadByCommunityId(654);

                Assert.Empty(forums);
            }
        }
    }
}
