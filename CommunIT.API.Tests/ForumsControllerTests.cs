using System.Linq;
using System.Threading.Tasks;
using CommunIT.API.Controllers;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Forum;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CommunIT.API.Tests
{
    public class ForumsControllerTests
    {
        [Fact]
        public async Task Get_returns_dtos()
        {
            var dto = new ForumDetailDto();
            var repository = new Mock<IForumRepository>();
            repository.Setup(r => r.ReadAsync(321)).ReturnsAsync(dto);
            var controller = new ForumsController(repository.Object);

            var get = await controller.Get(321);

            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IForumRepository>();
            var controller = new ForumsController(repository.Object);

            var get = await controller.Get(321);

            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Post_given_dto_creates_forum()
        {
            var repository = new Mock<IForumRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<ForumCreateDto>())).ReturnsAsync(new ForumDetailDto());
            var controller = new ForumsController(repository.Object);
            var dto = new ForumCreateDto();

            await controller.Post(dto);

            repository.Verify(s => s.CreateAsync(dto));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new ForumCreateDto();
            var output = new ForumDetailDto() { Id = 321 };
            var repository = new Mock<IForumRepository>();
            repository.Setup(r => r.CreateAsync(input)).ReturnsAsync(output);
            var controller = new ForumsController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal(321, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }
        
        [Fact]
        public async Task Put_given_dto_updates_forum()
        {
            var repository = new Mock<IForumRepository>();
            var controller = new ForumsController(repository.Object);
            var dto = new ForumUpdateDto();

            await controller.Put(dto);

            repository.Verify(r => r.UpdateAsync(dto));
        }

        [Fact]
        public async Task Put_returns_NoContent_on_success()
        {
            var dto = new ForumUpdateDto();
            var repository = new Mock<IForumRepository>();
            repository.Setup(r => r.UpdateAsync(dto)).ReturnsAsync(true);
            var controller = new ForumsController(repository.Object);

            var put = await controller.Put(dto);

            Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_non_existing_forum_returns_NotFound()
        {
            var repository = new Mock<IForumRepository>();
            var controller = new ForumsController(repository.Object);
            var dto = new ForumUpdateDto();

            var put = await controller.Put(dto);

            Assert.IsType<NotFoundResult>(put);
        }

        [Fact]
        public async Task Delete_given_id_deletes_forum()
        {
            var repository = new Mock<IForumRepository>();
            var controller = new ForumsController(repository.Object);

            await controller.Delete(321);

            repository.Verify(r => r.DeleteAsync(321));
        }

        [Fact]
        public async Task Delete_returns_NoContent_on_success()
        {
            var repository = new Mock<IForumRepository>();
            repository.Setup(r => r.DeleteAsync(321)).ReturnsAsync(true);
            var controller = new ForumsController(repository.Object);

            var delete = await controller.Delete(321);

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_non_existing_forum_returns_NotFound()
        {
            var repository = new Mock<IForumRepository>();
            var controller = new ForumsController(repository.Object);

            var delete = await controller.Delete(321);

            Assert.IsType<NotFoundResult>(delete);
        }

        [Fact]
        public void GetByCommunityId_given_community_id_returns_forums_in_commuity()
        {
            var dto = new ForumDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IForumRepository>();
            repository.Setup(r => r.ReadByCommunityId(123)).Returns(dtos.Object);

            var controller = new ForumsController(repository.Object);
            var get = controller.GetByCommunityId(123);

            Assert.Equal(dto, get.Value.Single());
        }
    }
}