using System.Linq;
using System.Threading.Tasks;
using CommunIT.API.Controllers;
using CommunIT.Models.Interfaces;
using CommunIT.Shared.Portable.DTOs.Community;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CommunIT.API.Tests
{
    public class CommunityControllerTests
    {
        [Fact]
        public async Task Get_returns_dto_given_valid_id()
        {
            var dto = new CommunityDetailDto();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.ReadAsync(123)).ReturnsAsync(dto);
            var controller = new CommunityController(repository.Object);

            var get = await controller.Get(123);

            Assert.Equal(dto, get.Value);
        }

        [Fact]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ICommunityRepository>();
            var controller = new CommunityController(repository.Object);

            var get = await controller.Get(123);

            Assert.IsType<NotFoundResult>(get.Result);
        }

        [Fact]
        public async Task Get_given_userId_returns_all_users_joined_communities()
        {
            var dto = new CommunityUpdateListViewDto { Id = 123 };
            var dto2 = new CommunityUpdateListViewDto { Id = 123 };
            var dtos = new[] { dto, dto2 }.AsQueryable().BuildMock();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.ReadByUserId("mail@domain.com")).Returns(dtos.Object);
            var controller = new CommunityController(repository.Object);

            var comments = await controller.GetByUserId("mail@domain.com");

            Assert.True(comments.Value.All(c => c.Id == 123));
        }

        [Fact]
        public async Task Get_given_no_params_returns_all_communities()
        {
            var dto = new CommunityUpdateListViewDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(s => s.Read()).Returns(dtos.Object);

            var controller = new CommunityController(repository.Object);
            var get = await controller.Get();

            Assert.Equal(dto, get.Value.Single());
        }
        

        [Fact]
        public async Task Post_given_dto_creates_community()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(s => s.CreateAsync(It.IsAny<CommunityCreateDto>())).ReturnsAsync(new CommunityDetailDto());
            var controller = new CommunityController(repository.Object);
            
            var dto = new CommunityCreateDto();
            await controller.Post(dto);

            repository.Verify(s => s.CreateAsync(dto));
        }

        [Fact]
        public async Task Post_given_dto_returns_CreatedAtActionResult()
        {
            var input = new CommunityCreateDto();
            var output = new CommunityDetailDto() { Id = 123 };
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.CreateAsync(input)).ReturnsAsync(output);
            var controller = new CommunityController(repository.Object);

            var post = await controller.Post(input);
            var result = post.Result as CreatedAtActionResult;

            Assert.Equal("Get", result.ActionName);
            Assert.Equal(123, result.RouteValues["id"]);
            Assert.Equal(output, result.Value);
        }

        [Fact]
        public async Task Put_given_dto_updates_community()
        {
            var repository = new Mock<ICommunityRepository>();
            var controller = new CommunityController(repository.Object);
            var dto = new CommunityUpdateListViewDto();

            await controller.Put(dto);

            repository.Verify(r => r.UpdateAsync(dto));
        }

        [Fact]
        public async Task Put_returns_NoContent_on_success()
        {
            var dto = new CommunityUpdateListViewDto();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.UpdateAsync(dto)).ReturnsAsync(true);
            var controller = new CommunityController(repository.Object);

            var put = await controller.Put(dto);

            Assert.IsType<NoContentResult>(put);
        }

        [Fact]
        public async Task Put_given_non_existing_community_returns_NotFound()
        {
            var repository = new Mock<ICommunityRepository>();
            var controller = new CommunityController(repository.Object);
            var dto = new CommunityUpdateListViewDto();

            var put = await controller.Put(dto);

            Assert.IsType<NotFoundResult>(put);
        }

        [Fact]
        public async Task Delete_given_id_deletes_community()
        {
            var repository = new Mock<ICommunityRepository>();
            var controller = new CommunityController(repository.Object);

            await controller.Delete(123);

            repository.Verify(r => r.DeleteAsync(123));
        }

        [Fact]
        public async Task Delete_returns_NoContent_on_success()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.DeleteAsync(123)).ReturnsAsync(true);
            var controller = new CommunityController(repository.Object);

            var delete = await controller.Delete(123);

            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_given_non_existing_community_returns_NotFound()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.DeleteAsync(123)).ReturnsAsync(false);
            var controller = new CommunityController(repository.Object);

            var delete = await controller.Delete(123);

            Assert.IsType<NotFoundResult>(delete);
        }

        [Fact]
        public async Task AddUserToCommunity_given_valid_ids_return_NoContent()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.AddUserToCommunity(1, "user@domain.com")).ReturnsAsync(true);
            var controller = new CommunityController(repository.Object);

            var post = await controller.PostUserToCommunity(1, "user@domain.com");

            Assert.IsType<NoContentResult>(post);
        }
        
        [Fact]
        public async Task AddUserToCommunity_given_invalid_ids_return_NotFound()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.AddUserToCommunity(1, "user@domain.com")).ReturnsAsync(false);
            var controller = new CommunityController(repository.Object);

            var post = await controller.PostUserToCommunity(1, "user@domain.com");

            Assert.IsType<NotFoundResult>(post);
        }

        [Fact]
        public async Task GetAdministratedByUserId_given_userid_returns_list_of_communities_administrated_by_user()
        {
            var dto = new CommunityUpdateListViewDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(s => s.ReadAdministratedByUserId("test@itu.dk")).Returns(dtos.Object);

            var controller = new CommunityController(repository.Object);
            var get = await controller.GetAdministratedByUserId("test@itu.dk");

            Assert.Equal(dto, get.Value.Single());
        }

        [Fact]
        public async Task GetFavoritedByUserId_given_userid_returns_list_of_useres_favorite_communities()
        {
            var dto = new CommunityUpdateListViewDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(s => s.ReadFavoritedByUserId("test@itu.dk")).Returns(dtos.Object);

            var controller = new CommunityController(repository.Object);
            var get = await controller.GetFavoritedByUserId("test@itu.dk");

            Assert.Equal(dto, get.Value.Single());
        }

        [Fact]
        public async Task PostCommunityToUserFavorite_given_userId_and_communityId_returns_NoContent_on_success()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.AddCommunityToUserFavorites(1, "test@itu.dk")).ReturnsAsync(true);
            var controller = new CommunityController(repository.Object);

            var post = await controller.PostCommunityToUserFavorite(1, "test@itu.dk");

            Assert.IsType<NoContentResult>(post);
        }

        [Fact]
        public async Task PostCommunityToUserFavorite_given_invalid_userId_and_communityId_returns_NotFound_on_failure()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.AddCommunityToUserFavorites(99, "test@itu.dk")).ReturnsAsync(false);
            var controller = new CommunityController(repository.Object);

            var post = await controller.PostCommunityToUserFavorite(99, "test@itu.dk");

            Assert.IsType<NotFoundResult>(post);
        }

        [Fact]
        public async Task GetMostPopular_returns_list_of_most_popular_communities()
        {
            var dto = new CommunityUpdateListViewDto();
            var dtos = new[] { dto }.AsQueryable().BuildMock();
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(s => s.ReadMostPopular()).Returns(dtos.Object);

            var controller = new CommunityController(repository.Object);
            var get = await controller.GetMostPopular();

            Assert.Equal(dto, get.Value.Single());
        }

        [Fact]
        public async Task IsUserInCommunity_returns_Ok_when_user_is_found()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.IsUserInCommunityAsync(1, "test@itu.dk")).ReturnsAsync(true);
            var controller = new CommunityController(repository.Object);

            var result = await controller.IsUserInCommunity(1, "test@itu.dk");

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task IsUserInCommunity_returns_NotFound_when_user_is_not_found()
        {
            var repository = new Mock<ICommunityRepository>();
            repository.Setup(r => r.IsUserInCommunityAsync(1, "test@itu.dk")).ReturnsAsync(false);
            var controller = new CommunityController(repository.Object);

            var result = await controller.IsUserInCommunity(1, "test@itu.dk");

            Assert.IsType<NotFoundResult>(result);
        }
    }
}