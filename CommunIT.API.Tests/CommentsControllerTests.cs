using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.API.Controllers;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Comment;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CommunIT.API.Tests
{
    public class CommentsControllerTests
    {
        [Fact]
        public async Task Get_given_existing_id_returns_dto()
        {
            var dto = new CommentDetailDto();
            var repository = new Mock<ICommentRepository>();
            repository.Setup(r => r.ReadAsync(5)).ReturnsAsync(dto);
            var controller = new CommentsController(repository.Object);
            
            var get = await controller.Get(5);

            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ICommentRepository>();
            var controller = new CommentsController(repository.Object);

            var get = await controller.Get(123);
            
            Assert.IsType<NotFoundResult>(get.Result);
        }
        
        [Fact]
        public void Get_given_userId_returns_all_comments_by_user()
        {
            var dto = new CommentDetailDto { Id = 123 };
            var dto2 = new CommentDetailDto { Id = 123 };
            var dtos = new[] { dto, dto2 }.AsQueryable().BuildMock();
            var repository = new Mock<ICommentRepository>();
            repository.Setup(r => r.ReadByUserId("user@mail.com")).Returns(dtos.Object);
            var controller = new CommentsController(repository.Object);

            var comments = controller.GetCommentsByUserId("user@mail.com");

            Assert.True(comments.All(c => c.Id == 123));
        }

        [Fact]
        public void Get_given_non_existing_userId_returns_empty_list()
        {
            var repository = new Mock<ICommentRepository>();
            var returnList = new List<CommentDetailDto>();
            repository.Setup(r => r.ReadByUserId("user@mail.com")).Returns(returnList);
            var controller = new CommentsController(repository.Object);

            var comments = controller.GetCommentsByUserId("user@mail.com");

            Assert.Empty(comments);
        }

        [Fact]
        public async Task Post_given_dto_creates_comment()
        {
            var repository = new Mock<ICommentRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<CommentCreateUpdateDto>()))
                .ReturnsAsync(new CommentDetailDto());
            var controller = new CommentsController(repository.Object);
            var dto = new CommentCreateUpdateDto();

            await controller.Post(dto);
            
            repository.Verify(c => c.CreateAsync(dto));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new CommentCreateUpdateDto();
            var output = new CommentDetailDto { Id = 1337 };
            var repository = new Mock<ICommentRepository>();
            repository.Setup(r => r.CreateAsync(input)).ReturnsAsync(output);
            var controller = new CommentsController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal(1337, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }

        [Fact]
        public async Task Post_given_dto_without_content_returns_BadRequest()
        {
            var repository = new Mock<ICommentRepository>();
            var controller = new CommentsController(repository.Object);
            
            var get = await controller.Post(new CommentCreateUpdateDto());
            
            Assert.IsType<BadRequestResult>(get.Result);
        }

        [Fact]
        public async Task Put_given_dto_updates_comment()
        {
            var repository = new Mock<ICommentRepository>();
            var controller = new CommentsController(repository.Object);
            var dto = new CommentCreateUpdateDto { Content = "Merry Christmas" };

            await controller.Put(420, dto);
            
            repository.Verify(c => c.UpdateAsync(420, dto));
        }

        [Fact]
        public async Task Put_returns_NoContent_on_success()
        {
            var dto = new CommentCreateUpdateDto();
            var repository = new Mock<ICommentRepository>();
            repository.Setup(r => r.UpdateAsync(1234, dto)).ReturnsAsync(true);
            var controller = new CommentsController(repository.Object);

            var put = await controller.Put(1234, dto);

            Assert.IsType<NoContentResult>(put);
        }
        
        [Fact]
        public async Task Put_given_non_existing_comment_returns_NotFound()
        {
            var repository = new Mock<ICommentRepository>();
            var controller = new CommentsController(repository.Object);
            var dto = new CommentCreateUpdateDto();

            var put = await controller.Put(1337, dto);

            Assert.IsType<NotFoundResult>(put);
        }
        
        [Fact]
        public async Task Delete_given_id_deletes_comment()
        {
            var repository = new Mock<ICommentRepository>();
            var controller = new CommentsController(repository.Object);

            await controller.Delete(111);

            repository.Verify(r => r.DeleteAsync(111));
        }

        [Fact]
        public async Task Delete_returns_NoContent_on_success()
        {
            var repository = new Mock<ICommentRepository>();
            repository.Setup(r => r.DeleteAsync(1337)).ReturnsAsync(true);
            var controller = new CommentsController(repository.Object);

            var delete = await controller.Delete(1337);

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ICommentRepository>();
            var controller = new CommentsController(repository.Object);

            var delete = await controller.Delete(1337);

            Assert.IsType<NotFoundResult>(delete);
        }

        [Fact]
        public void GetCommentsByThreadIs_returns_list_of_comments()
        {
            var dto = new CommentDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<ICommentRepository>();
            repository.Setup(s => s.ReadByThreadId(1)).Returns(dtos.Object);

            var controller = new CommentsController(repository.Object);
            var get = controller.GetCommentsByThreadId(1);

            Assert.Equal(dto, get.Single());
        }
        
    }
}