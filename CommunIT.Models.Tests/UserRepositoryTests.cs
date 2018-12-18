using System;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Entities;
using CommunIT.Shared.Portable.DTOs.User;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_given_dto_created_User()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {   
                var repository = new UserRepository(context);
                var dto = new UserCreateDto
                {
                    Username = "mail@user.com",
                    DisplayName = "Longjohn1337",
                    Bio = "I like cookies and cake"
                };
                var user = await repository.CreateAsync(dto);

                var entity = await context.Users.FindAsync(user.Username);
                
                Assert.Equal(user.Username, entity.Username);
                Assert.Equal("Longjohn1337", entity.DisplayName);
                Assert.Equal("I like cookies and cake", entity.Bio);
            }
        }

        [Fact]
        public async Task CreateAsync_given_dto_where_username_is_empty_returns_null()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new UserRepository(context);
                var dto = new UserCreateDto
                {
                    Bio = "I like cookies and cake"
                };
                var user = await repository.CreateAsync(dto);
                
                Assert.Null(user);
            }
        }
        
        [Fact]
        public async Task CreateAsync_given_dto_where_bio_is_whitespace_returns_null()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new UserRepository(context);
                var dto = new UserCreateDto
                {
                    Username = "   ",
                    Bio = "I like cookies and cake"
                };
                var user = await repository.CreateAsync(dto);
                
                Assert.Null(user);
            }
        }

        [Fact]
        public async Task DeleteAsync_given_id_deletes_user()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = new User
                {
                    Username = "mail@user.com",
                    DisplayName = "Longjohn1337"
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var id = user.Username;
                var repository = new UserRepository(context);
                var deleted = await repository.DeleteAsync(id);
                
                Assert.True(deleted);

            }
        }
        
        [Fact]
        public async Task DeleteAsync_given_id_that_does_not_exits_returns_false()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new UserRepository(context);
                
                var deleted = await repository.DeleteAsync("test");
                
                Assert.False(deleted);
            }
        }

        [Fact]
        public async Task ReadAsync_given_valid_id_returns_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var entity = new User
                {
                    Username = "mail@user.com",
                    DisplayName = "Longjohn1337",
                    Bio = "I like cookies",
                    Created = new DateTime(2018, 11, 13)
                };
                var u = context.Users.Add(entity);
                await context.SaveChangesAsync();
                
                var repository = new UserRepository(context);
                var user = await repository.ReadAsync(u.Entity.Username);

                Assert.Equal("mail@user.com", user.Username);
                Assert.Equal("Longjohn1337", user.DisplayName);
                Assert.Equal("I like cookies", user.Bio);
                Assert.Equal(new DateTime(2018, 11, 13), user.Created);
            }
        }

        [Fact]
        public async Task ReadAsync_given_non_existing_id_returns_null()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new UserRepository(context);
                var user = await repository.ReadAsync("test");
                
                Assert.Null(user);                
            }
        }

        [Fact]
        public async Task Read_returns_list_of_users()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var entity = new User
                {
                    Username = "mail@user.com",
                    DisplayName = "Longjohn1337",
                    Bio = "I like cookies",
                    Created = new DateTime(2018, 11, 13)
                };
                
                var u = context.Users.Add(entity);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var characters = repository.Read();

                var user = characters.Last();

                Assert.Equal(u.Entity.Username, user.Username);
                Assert.Equal("Longjohn1337", user.DisplayName);
                Assert.Equal("I like cookies", user.Bio);
                Assert.Equal(new DateTime(2018, 11, 13), user.Created);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_existing_userid_updates_entity()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var u = context.Users.Add(new User
                {
                    Username = "mail@user.com",
                    DisplayName = "Longjohn1337",
                    Bio = "I like cake"
                });

                await context.SaveChangesAsync();
                
                var repository = new UserRepository(context);
                var dto = new UserCreateDto
                {
                    Bio = "I like cake cookies",
                    DisplayName = "Longjohn69",
                    University = "IT Univerity of Copenhagen"
                };

                var updated = await repository.UpdateAsync(u.Entity.Username, dto);
                
                Assert.True(updated);

                var entity = await context.Users.FindAsync(u.Entity.Username);
                
                Assert.Equal("I like cake cookies", entity.Bio);
                Assert.Equal("Longjohn69", entity.DisplayName);
                Assert.Equal("IT Univerity of Copenhagen", entity.University);
            }
        }

        [Fact]
        public async Task UpdateAsync_given_non_existing_userid_returns_false()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new UserRepository(context);
                var dto = new UserCreateDto
                {
                    Bio = "I like cake cookies"
                };
                
                var updated = await repository.UpdateAsync("mail@domain.com", dto);

                Assert.False(updated);
            }
        }

        [Fact]
        public async Task ReadUsersInEvent_returns_list_of_users_in_event()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var commuity = await context.Communities.AddAsync(new Community { Name = "test", Description = "test"});
                var @event = await context.Events.AddAsync(new Event { Title = "test", Description = "test", Date = DateTime.Today, CommunityId = commuity.Entity.Id });
                var eventUser = await context.EventUsers.AddAsync(new EventUser { EventId = @event.Entity.Id, UserId = "kols@itu.dk" });

                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var users = repository.ReadUsersInEvent(@event.Entity.Id);

                Assert.Single(users);
                Assert.Equal(eventUser.Entity.UserId, users.Single().Username);
            }
        }

        [Fact]
        public async Task IsUserAdminInCommunity_returns_true_when_user_is_admin()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var commuity = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityUser = new CommunityUser { CommunityId = commuity.Id, UserId = user.Username, IsAdmin = true };
                await context.CommunityUsers.AddAsync(communityUser);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var result = repository.IsUserAdminInCommunity(user.Username, commuity.Id);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task IsUserAdminInCommunity_returns_false_when_user_is_not_admin()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var commuity = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityUser = new CommunityUser { CommunityId = commuity.Id, UserId = user.Username };
                await context.CommunityUsers.AddAsync(communityUser);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);

                var result = repository.IsUserAdminInCommunity(user.Username, commuity.Id);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsUserAdminInCommunity_returns_false_when_user_does_not_exits()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var commuity = await TestHelper.CreateTestCommunityAsync(context);

                var repository = new UserRepository(context);

                var result = repository.IsUserAdminInCommunity("test@itu.dk", commuity.Id);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsUserAdminInCommunity_returns_false_when_community_does_not_exits()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = await TestHelper.CreateTestUserAsync(context);

                var repository = new UserRepository(context);

                var result = repository.IsUserAdminInCommunity(user.Username, 9998);

                Assert.False(result);
            }
        }
    }
}
