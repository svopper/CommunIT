using CommunIT.Entities;
using CommunIT.Shared.Portable.DTOs.Community;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class CommunityRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_creates_and_returns_a_community_when_given_a_valid_dto()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityDto = new CommunityCreateDto
                {
                    Name = "Bunnies4Life",
                    Description = "Community for bunny-lovers",
                    BaseTagIds = new[] { 1 },
                    SubTagIds = new[] { 4 },
                    CreatedBy = user.Username
                    
                };

                var result = await repository.CreateAsync(communityDto);

                Assert.NotNull(result);
                Assert.Equal(communityDto.Name, result.Name);
                Assert.Equal(communityDto.Description, result.Description);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_a_community_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityDto = new CommunityCreateDto
                {
                    Name = "Bunnies4Life",
                    Description = "Community for bunny-lovers",
                    BaseTagIds = new[] { 1 },
                    SubTagIds = new[] { 4 },
                    CreatedBy = user.Username
                };

                var foundCommunity = await repository.CreateAsync(communityDto);
                var result = await repository.ReadAsync(foundCommunity.Id);

                Assert.NotNull(result);
                Assert.Equal(communityDto.Name, result.Name);
                Assert.Equal(communityDto.Description, result.Description);
                Assert.True(result.Id > 0);
            }
        }

        [Fact]
        public async Task ReadAsync_returns_null_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);

                var result = await repository.ReadAsync(123);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ReadByUserId_given_userid_returns_communities_user_is_member_of()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = new Community { Name = "Stuff", Description = "Community about stuff" };
                var anotherCommunity = new Community { Name = "Bunny Lovers", Description = "Community about bunnies" };
                var user = new User { Username = "Han Solo" };

                var communityCreated = await context.Communities.AddAsync(community);
                await context.Communities.AddAsync(anotherCommunity);

                var userCreated = await context.Users.AddAsync(user);

                var communityUser = new CommunityUser
                {
                    CommunityId = communityCreated.Entity.Id,
                    UserId = userCreated.Entity.Username
                };

                await context.CommunityUsers.AddAsync(communityUser);
                await context.SaveChangesAsync();
                var repository = new CommunityRepository(context);

                var communities = repository.ReadByUserId(communityUser.UserId).ToList();

                Assert.Equal(community.Name, communities.Single().Name);
                Assert.Single(communities);
            }
        }

        [Fact]
        public async Task Read_returns_list_of_communities()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var entity = new Community
                {
                    Name = "Bunnies",
                    Description = "All about the bass",

                };

                var c = context.Communities.Add(entity);
                await context.SaveChangesAsync();

                var repository = new CommunityRepository(context);

                var communities = repository.Read();

                var community = communities.Last();

                Assert.Equal("Bunnies", community.Name);
                Assert.Equal("All about the bass", community.Description);
            }
        }


        [Fact]
        public async Task UpdateAsync_returns_true_when_given_an_existing_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityDto = new CommunityCreateDto
                {
                    Name = "Bunnies4Life",
                    Description = "Community for bunny-lovers",
                    BaseTagIds = new[] { 1 },
                    SubTagIds = new[] { 4 },
                    CreatedBy = user.Username
                };
                var foundCommunity = await repository.CreateAsync(communityDto);

                var newCommunityDto = new CommunityUpdateListViewDto
                {
                    Id = foundCommunity.Id,
                    Name = "New name",
                    Description = "New Description"
                };
                var result = await repository.UpdateAsync(newCommunityDto);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_returns_false_when_given_an_non_existing_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);

                var newCommunityDto = new CommunityUpdateListViewDto
                {
                    Id = 123,
                    Name = "New name",
                    Description = "New Description"
                };
                var result = await repository.UpdateAsync(newCommunityDto);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_true_when_given_an_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityDto = new CommunityCreateDto
                {
                    Name = "Bunnies4Life",
                    Description = "Community for bunny-lovers",
                    BaseTagIds = new[] { 1 },
                    SubTagIds = new[] { 4 },
                    CreatedBy = user.Username
                };

                var foundCommunity = await repository.CreateAsync(communityDto);
                var result = await repository.DeleteAsync(foundCommunity.Id);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_returns_false_when_given_an_non_existing_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repository = new CommunityRepository(context);

                var result = await repository.DeleteAsync(123);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task AddUserToCommunity_returns_true_when_given_valid_ids()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = new Community { Name = "Stuff", Description = "Community about stuff" };
                var user = new User { Username = "user2@domain.com", DisplayName = "Han Solo" };

                var crComm = await context.Communities.AddAsync(community);
                var crUser = await context.Users.AddAsync(user);

                await context.SaveChangesAsync();
                
                var repo = new CommunityRepository(context);

                var result = await repo.AddUserToCommunity(crComm.Entity.Id, crUser.Entity.Username);
                
                Assert.True(result);
            }
        }
        
        [Fact]
        public async Task AddUserToCommunity_returns_false_when_given_invalid_ids()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repo = new CommunityRepository(context);

                var result = await repo.AddUserToCommunity(1337, "hvorblevelefantens@lol.dk");
                
                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReadAdministratedByUserId_returns_list_when_given_userid()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityUserCreate = new CommunityUser
                {
                    CommunityId = community.Id,
                    UserId = user.Username,
                    IsAdmin = true
                };
                var communityUser = await context.CommunityUsers.AddAsync(communityUserCreate);
                await context.SaveChangesAsync();

                var repository = new CommunityRepository(context);
                var administratedCommunities = repository.ReadAdministratedByUserId(user.Username);

                Assert.Single(administratedCommunities);
                Assert.Equal(community.Id, administratedCommunities.Single().Id);
            }
        }

        [Fact]
        public async Task ReadAdministratedByUserId_returns_empty_list_when_given_userid_with_no_administraing_communities()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = await TestHelper.CreateTestUserAsync(context);

                var repository = new CommunityRepository(context);
                var administratedCommunities = repository.ReadAdministratedByUserId(user.Username);

                Assert.Empty(administratedCommunities);
            }
        }

        [Fact]
        public async Task ReadFavoritedByUserId_returns_list_when_given_userid()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityUserCreate = new CommunityUser
                {
                    CommunityId = community.Id,
                    UserId = user.Username,
                    IsFavorite = true
                };
                var communityUser = await context.CommunityUsers.AddAsync(communityUserCreate);
                await context.SaveChangesAsync();

                var repository = new CommunityRepository(context);
                var favoriteCommunities = repository.ReadFavoritedByUserId(user.Username);

                Assert.Single(favoriteCommunities);
                Assert.Equal(community.Id, favoriteCommunities.Single().Id);
            }
        }

        [Fact]
        public async Task ReadFavoritedByUserId_returns_empty_list_when_given_userid_with_no_favorites()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = await TestHelper.CreateTestUserAsync(context);
                
                var repository = new CommunityRepository(context);
                var favoriteCommunities = repository.ReadFavoritedByUserId(user.Username);

                Assert.Empty(favoriteCommunities);
            }
        }

        [Fact]
        public async Task ReadMostPopular_returns_top5_most_popular_communities()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                await context.Communities.AddAsync(new Community { Name = "Name1", Description = "Desc1" });
                await context.Communities.AddAsync(new Community { Name = "Name2", Description = "Desc2" });
                await context.Communities.AddAsync(new Community { Name = "Name3", Description = "Desc3" });
                await context.Communities.AddAsync(new Community { Name = "Name4", Description = "Desc4" });
                await context.Communities.AddAsync(new Community { Name = "Name5", Description = "Desc5" });

                var repository = new CommunityRepository(context);
                var mostPopular = repository.ReadMostPopular();

                Assert.Equal(5, mostPopular.Count());
            }
        }

        [Fact]
        public async Task IsUserInCommunity_returns_true_when_user_is_member_of_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityUser = new CommunityUser { CommunityId = community.Id, UserId = user.Username };
                await context.CommunityUsers.AddAsync(communityUser);
                await context.SaveChangesAsync();

                var repo = new CommunityRepository(context);
                var result = await repo.IsUserInCommunityAsync(community.Id, user.Username);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task IsUserInCommunity_returns_false_when_user_is_not_member_of_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);

                var repo = new CommunityRepository(context);
                var result = await repo.IsUserInCommunityAsync(community.Id, user.Username);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsUserInCommunity_returns_false_when_given_non_existing_userId()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);

                var repo = new CommunityRepository(context);
                var result = await repo.IsUserInCommunityAsync(community.Id, "test@itu.dk");

                Assert.False(result);
            }
        }

        [Fact]
        public async Task IsUserInCommunity_returns_false_when_given_non_existing_communityId()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = await TestHelper.CreateTestUserAsync(context);

                var repo = new CommunityRepository(context);
                var result = await repo.IsUserInCommunityAsync(1, user.Username);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task AddCommunityToUserFavorites_returns_true_when_given_valid_ids_for_user_and_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);
                var user = await TestHelper.CreateTestUserAsync(context);
                var communityUser = new CommunityUser { CommunityId = community.Id, UserId = user.Username };
                await context.CommunityUsers.AddAsync(communityUser);
                await context.SaveChangesAsync();

                var repo = new CommunityRepository(context);
                var result = await repo.AddCommunityToUserFavorites(community.Id, user.Username);
                var updatedCommunityUser = context.CommunityUsers.Where(cu => cu.CommunityId == community.Id && cu.UserId == user.Username).Single();
                
                Assert.True(result);
                Assert.True(updatedCommunityUser.IsFavorite);
            }
        }

        [Fact]
        public async Task AddCommunityToUserFavorites_returns_false_when_given_invalid_userId()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var community = await TestHelper.CreateTestCommunityAsync(context);

                var repo = new CommunityRepository(context);
                var result = await repo.AddCommunityToUserFavorites(community.Id, "test@itu.dk");

                Assert.False(result);
            }
        }

        [Fact]
        public async Task AddCommunityToUserFavorites_returns_false_when_given_invalid_communityId()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var user = await TestHelper.CreateTestUserAsync(context);

                var repo = new CommunityRepository(context);
                var result = await repo.AddCommunityToUserFavorites(999, user.Username);

                Assert.False(result);
            }
        }


    }
}
