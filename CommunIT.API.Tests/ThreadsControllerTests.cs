using System.Linq;
using System.Threading.Tasks;
using CommunIT.API.Controllers;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Thread;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CommunIT.API.Tests
{
    public class ThreadsControllerTests
    {
        [Fact]
        public async Task Get_returns_dto()
        {
            var dto = new ThreadDetailDto();
            var repository = new Mock<IThreadRepository>();
            repository.Setup(r => r.ReadAsync(123)).ReturnsAsync(dto);
            var controller = new ThreadsController(repository.Object);

            var get = await controller.Get(123);
            
            Assert.Equal(dto, get.Value);
        }
        
        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IThreadRepository>();
            var controller = new ThreadsController(repository.Object);

            var get = await controller.Get(123);

            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Post_given_dto_creates_thread()
        {
            var repository = new Mock<IThreadRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<ThreadCreateDto>())).ReturnsAsync(new ThreadDetailDto());
            var controller = new ThreadsController(repository.Object);
            var dto = new ThreadCreateDto();

            await controller.Post(dto);

            repository.Verify(s => s.CreateAsync(dto));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new ThreadCreateDto();
            var output = new ThreadDetailDto() { Id = 123 };
            var repository = new Mock<IThreadRepository>();
            repository.Setup(r => r.CreateAsync(input)).ReturnsAsync(output);
            var controller = new ThreadsController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal(123, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }
        
        [Fact]
        public async Task Put_given_dto_updates_thread()
        {
            var repository = new Mock<IThreadRepository>();
            var controller = new ThreadsController(repository.Object);
            var dto = new ThreadUpdateDto();

            await controller.Put(dto);

            repository.Verify(r => r.UpdateAsync(dto));
        }

        [Fact]
        public async Task Put_returns_NoContent_on_success()
        {
            var dto = new ThreadUpdateDto();
            var repository = new Mock<IThreadRepository>();
            repository.Setup(r => r.UpdateAsync(dto)).ReturnsAsync(true);
            var controller = new ThreadsController(repository.Object);

            var put = await controller.Put(dto);

            Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_non_existing_thread_returns_NotFound()
        {
            var repository = new Mock<IThreadRepository>();
            var controller = new ThreadsController(repository.Object);
            var dto = new ThreadUpdateDto();

            var put = await controller.Put(dto);

            Assert.IsType<NotFoundResult>(put);
        }

        [Fact]
        public async Task Delete_given_id_deletes_thread()
        {
            var repository = new Mock<IThreadRepository>();
            var controller = new ThreadsController(repository.Object);

            await controller.Delete(123);

            repository.Verify(r => r.DeleteAsync(123));
        }

        [Fact]
        public async Task Delete_returns_NoContent_on_success()
        {
            var repository = new Mock<IThreadRepository>();
            repository.Setup(r => r.DeleteAsync(123)).ReturnsAsync(true);
            var controller = new ThreadsController(repository.Object);

            var delete = await controller.Delete(123);

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_non_existing_forum_returns_NotFound()
        {
            var repository = new Mock<IThreadRepository>();
            var controller = new ThreadsController(repository.Object);

            var delete = await controller.Delete(123);

            Assert.IsType<NotFoundResult>(delete);
        }

        [Fact]
        public void GetByForumId_given_forum_id_returns_threads_in_forum()
        {
            var dto = new ThreadDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IThreadRepository>();
            repository.Setup(r => r.ReadByForumId(1337)).Returns(dtos.Object);

            var controller = new ThreadsController(repository.Object);
            var get = controller.GetByForumId(1337);

            Assert.Equal(dto, get.Value.Single());
        }
    }
}