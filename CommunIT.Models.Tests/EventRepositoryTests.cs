using CommunIT.Entities;
using CommunIT.Shared.Portable.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class EventRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_creates_and_returns_an_event_when_given_a_valid_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };

                var result = await repository.CreateAsync(eventCreateDto);

                Assert.NotNull(result);
                Assert.Equal(eventCreateDto.Title, result.Title);
                Assert.Equal(eventCreateDto.Date, result.Date);
                Assert.Equal(eventCreateDto.Description, result.Description);
            }
        }

        [Fact]
        public async Task CreateAsync_returns_null_when_given_event_with_invalid_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = 123
                };

                var result = await repository.CreateAsync(eventCreateDto);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_an_event_when_given_an_existing_event_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };
                var foundEvent = await repository.CreateAsync(eventCreateDto);

                var result = await repository.ReadAsync(foundEvent.Id);

                Assert.NotNull(result);
                Assert.Equal(eventCreateDto.Title, result.Title);
                Assert.Equal(eventCreateDto.Date, result.Date);
                Assert.Equal(eventCreateDto.Description, result.Description);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_null_when_given_a_non_existing_event_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var result = await repository.ReadAsync(123);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_true_when_given_an_existing_event()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };
                var foundEvent = await repository.CreateAsync(eventCreateDto);

                var eventUpdateDto = new EventUpdateDetailDto
                {
                    Id = foundEvent.Id,
                    Title = "New title",
                    Date = DateTime.Now.AddDays(2),
                    Description = "New description"
                };

                var result = await repository.UpdateAsync(eventUpdateDto);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_false_when_given_an_non_existing_event()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var eventUpdateDto = new EventUpdateDetailDto
                {
                    Id = 123,
                    Title = "New title",
                    Date = DateTime.Now.AddDays(2),
                    Description = "New description"
                };

                var result = await repository.UpdateAsync(eventUpdateDto);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_true_when_given_an_existing_event_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };
                var foundEvent = await repository.CreateAsync(eventCreateDto);

                var result = await repository.DeleteAsync(foundEvent.Id);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_false_when_given_an_non_existing_event_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var result = await repository.DeleteAsync(123);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task AddUserToEventAsync_returns_true_when_given_an_existing_event_and_user_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };
                var foundEvent = await repository.CreateAsync(eventCreateDto);

                var result = await repository.AddUserToEventAsync(foundEvent.Id, user.Username);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task AddUserToEventAsync_returns_false_when_given_an_non_existing_event_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);
                var user = await TestHelper.CreateTestUserAsync(context);

                var result = await repository.AddUserToEventAsync(123, user.Username);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task AddUserToEventAsync_returns_false_when_given_an_non_existing_user_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };
                var foundEvent = await repository.CreateAsync(eventCreateDto);

                var result = await repository.AddUserToEventAsync(foundEvent.Id, "Not@Real.com");

                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReadByCommunityId_returns_all_events_in_community_when_given_a_existing_community_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new EventRepository(context);

                var community = await TestHelper.CreateTestCommunityAsync(context);
                var eventCreateDto = new EventCreateDto
                {
                    Title = "Celebration of bunnies",
                    Date = DateTime.Now.AddDays(1),
                    Description = "zup wup!",
                    CommunityId = community.Id
                };
                await repository.CreateAsync(eventCreateDto);

                var result = repository.ReadByCommunityId(community.Id);

                Assert.NotNull(result);
                Assert.Single(result);
            }
        }

        //[Fact]
        //public async Task RemoveUserFromEventAsync()
        //{
        //    using (var context = await TestHelper.CreateContextWithConnectionAsync())
        //    {
        //        var community = await TestHelper.CreateTestCommunityAsync(context);
        //        var user = await TestHelper.CreateTestUserAsync(context);
        //        var @event = new Event {CommunityId = community.Id};
        //        await context.Events.AddAsync(@event);
        //        var eventUser = new EventUser
        //        {
        //            EventId = @event.Id,
        //            UserId = user.Username
        //        };
        //        await context.EventUsers.AddAsync(eventUser);
        //        await context.SaveChangesAsync();

        //        var repo = new EventRepository(context);

        //        var result = await repo.RemoveUserFromEventAsync(@event.Id, user.Username);
        //        var updatedEventUser = context.EventUsers.Where(eu => eu.EventId == @event.Id && eu.UserId == user.Username).SingleOrDefault();

        //        Assert.True(result);
        //        Assert.Null(updatedEventUser);
        //    }
        //}
    }
}
