using CommunIT.API.Controllers;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Event;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunIT.API.Tests
{
    public class EventsControllerTests
    {
        [Fact]
        public async Task Get_returns_dtos()
        {
            var dto = new EventUpdateDetailDto();
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.ReadAsync(123)).ReturnsAsync(dto);
            var controller = new EventsController(repository.Object);

            var get = await controller.Get(123);

            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IEventRepository>();
            var controller = new EventsController(repository.Object);

            var get = await controller.Get(123);

            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Post_given_dto_creates_event()
        {
            var repository = new Mock<IEventRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<EventCreateDto>())).ReturnsAsync(new EventUpdateDetailDto());
            var controller = new EventsController(repository.Object);
            var dto = new EventCreateDto();

            await controller.Post(dto);

            repository.Verify(s => s.CreateAsync(dto));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new EventCreateDto();
            var output = new EventUpdateDetailDto() { Id = 321 };
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.CreateAsync(input)).ReturnsAsync(output);
            var controller = new EventsController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal(321, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }

        [Fact]
        public async Task Put_given_dto_updates_event()
        {
            var repository = new Mock<IEventRepository>();
            var controller = new EventsController(repository.Object);
            var dto = new EventUpdateDetailDto();

            await controller.Put(dto);

            repository.Verify(r => r.UpdateAsync(dto));
        }

        [Fact]
        public async Task Put_returns_NoContent_on_success()
        {
            var dto = new EventUpdateDetailDto();
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.UpdateAsync(dto)).ReturnsAsync(true);
            var controller = new EventsController(repository.Object);

            var put = await controller.Put(dto);

            Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_non_existing_event_returns_NotFound()
        {
            var repository = new Mock<IEventRepository>();
            var controller = new EventsController(repository.Object);
            var dto = new EventUpdateDetailDto();

            var put = await controller.Put(dto);

            Assert.IsType<NotFoundResult>(put);
        }

        [Fact]
        public async Task Delete_given_id_deletes_event()
        {
            var repository = new Mock<IEventRepository>();
            var controller = new EventsController(repository.Object);

            await controller.Delete(123);

            repository.Verify(r => r.DeleteAsync(123));
        }

        [Fact]
        public async Task Delete_returns_NoContent_on_success()
        {
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.DeleteAsync(123)).ReturnsAsync(true);
            var controller = new EventsController(repository.Object);

            var delete = await controller.Delete(123);

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_non_existing_event_returns_NotFound()
        {
            var repository = new Mock<IEventRepository>();
            var controller = new EventsController(repository.Object);

            var delete = await controller.Delete(123);

            Assert.IsType<NotFoundResult>(delete);
        }

        [Fact]
        public async Task PostUserToEvent_given_valid_eventid_and_userid_returns_NoContent()
        {
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.AddUserToEventAsync(11, "mail@user.com")).ReturnsAsync(true);
            var controller = new EventsController(repository.Object);

            var post = await controller.PostUserToEvent(11, "mail@user.com");
            
            Assert.IsType<NoContentResult>(post);
        }

        [Fact]
        public async Task PostUserToEvent_given_invalid_eventid_and_userid_returns_BadRequest()
        {
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.AddUserToEventAsync(11, "mail@user.com")).ReturnsAsync(false);
            var controller = new EventsController(repository.Object);

            var post = await controller.PostUserToEvent(11, "mail@user.com");

            Assert.IsType<BadRequestResult>(post);
        }

        [Fact]
        public async Task DeleteUserFromEvent_given_valid_eventid_and_userid_returns_NoContent()
        {
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.RemoveUserFromEventAsync(11, "mail@user.com")).ReturnsAsync(true);
            var controller = new EventsController(repository.Object);

            var delete = await controller.DeleteUserFromEvent(11, "mail@user.com");

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task DeleteUserFromEvent_given_invalid_eventid_and_userid_returns_BadRequest()
        {
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.RemoveUserFromEventAsync(11, "mail@user.com")).ReturnsAsync(false);
            var controller = new EventsController(repository.Object);

            var delete = await controller.DeleteUserFromEvent(11, "mail@user.com");

            Assert.IsType<BadRequestResult>(delete);
        }

        [Fact]
        public void GetByCommunityId_given_community_id_returns_events_in_commuity()
        {
            var dto = new EventUpdateDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IEventRepository>();
            repository.Setup(r => r.ReadByCommunityId(321)).Returns(dtos.Object);

            var controller = new EventsController(repository.Object);
            var get = controller.GetByCommunityId(321);

            Assert.Equal(dto, get.Value.Single());
        }
    }
}
