using CommunIT.API.Controllers;
using CommunIT.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using CommunIT.Shared.Portable.DTOs.User;
using Xunit;

namespace CommunIT.API.Tests
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task Get_returns_dtos()
        {
            var dto = new UserDetailDto();
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.ReadAsync("mail@domain.com")).ReturnsAsync(dto);
            var controller = new UsersController(repository.Object);

            var get = await controller.Get(new UserSearchDto { Username = "mail@domain.com" });

            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            var controller = new UsersController(repository.Object);

            var get = await controller.Get(new UserSearchDto { Username = "mail@domain.com" });

            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Get_given_no_params_returns_all_users()
        {
            var dto = new UserDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.Read()).Returns(dtos.Object);

            var controller = new UsersController(repository.Object);
            var get = await controller.Get();

            Assert.Equal(dto, get.Value.Single());
        }

        [Fact]
        public async Task Post_given_dto_creates_user()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<UserCreateDto>())).ReturnsAsync(new UserDetailDto());
            var controller = new UsersController(repository.Object);
            var dto = new UserCreateDto();

            await controller.Post(dto);

            repository.Verify(s => s.CreateAsync(dto));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new UserCreateDto();
            var output = new UserDetailDto { Username = "mail@domain.com" };
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.CreateAsync(input)).ReturnsAsync(output);
            var controller = new UsersController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal("mail@domain.com", result.RouteValues["username"]);
            Assert.Equal(output, result.Value);
        }
        
        [Fact]
        public async Task Put_given_dto_updates_user()
        {
            var repository = new Mock<IUserRepository>();
            var controller = new UsersController(repository.Object);
            var dto = new UserCreateDto { Username = "mail@mail.com" };

            await controller.Put(dto);

            repository.Verify(r => r.UpdateAsync("mail@mail.com", dto));
        }

        [Fact]
        public async Task Put_returns_NoContent_on_success()
        {
            var dto = new UserCreateDto { Username = "mail@domain.com" };
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.UpdateAsync("mail@domain.com", dto)).ReturnsAsync(true);
            var controller = new UsersController(repository.Object);

            var put = await controller.Put(dto);

            Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_non_existing_user_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            var controller = new UsersController(repository.Object);
            var dto = new UserCreateDto { Username = "mail@domain.com" };

            var put = await controller.Put(dto);

            Assert.IsType<NotFoundResult>(put);
        }

        [Fact]
        public async Task Delete_given_id_deletes_user()
        {
            var repository = new Mock<IUserRepository>();
            var controller = new UsersController(repository.Object);

            await controller.Delete("mail@domain.com");

            repository.Verify(r => r.DeleteAsync("mail@domain.com"));
        }

        [Fact]
        public async Task Delete_returns_NoContent_on_success()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.DeleteAsync("mail@domain.com")).ReturnsAsync(true);
            var controller = new UsersController(repository.Object);

            var delete = await controller.Delete("mail@domain.com");

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            var controller = new UsersController(repository.Object);

            var delete = await controller.Delete("mail@domain.com");

            Assert.IsType<NotFoundResult>(delete);
        }

        [Fact]
        public async Task GetByEventId_returns_list_of_users_in_event()
        {
            var dto = new UserDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.ReadUsersInEvent(1)).Returns(dtos.Object);

            var controller = new UsersController(repository.Object);
            var get = await controller.GetByEventId(1);

            Assert.Equal(dto, get.Value.Single());
        }

        [Fact]
        public void IsUserAdminInCommunity_returns_Ok_if_user_is_admin_in_community()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.IsUserAdminInCommunity("test@itu.dk", 1)).Returns(true);
            var controller = new UsersController(repository.Object);

            var result = controller.IsUserAdminInCommunity("test@itu.dk", 1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void IsUserAdminInCommunity_returns_NotFound_if_user_is_not_admin_in_community()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.IsUserAdminInCommunity("test@itu.dk", 1)).Returns(false);
            var controller = new UsersController(repository.Object);

            var result = controller.IsUserAdminInCommunity("test@itu.dk", 1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByCommunityId_returns_list_of_all_users_in_community()
        {
            var dto = new UserDetailDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<IUserRepository>();
            repository.Setup(s => s.ReadUsersInCommunity(1)).Returns(dtos.Object);

            var controller = new UsersController(repository.Object);
            var get = await controller.GetByCommunityId(1);

            Assert.Equal(dto, get.Value.Single());
        }


    }
}
