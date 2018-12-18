using System.Linq;
using System.Threading.Tasks;
using CommunIT.Entities;
using Xunit;

namespace CommunIT.Models.Tests
{
    public class TagRepositoryTests
    {
        [Fact]
        public async Task Read_returns_list_of_all_Basetags()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {   
                var repo = new TagRepository(context);

                var tags = repo.ReadBaseTags();
                
                Assert.Equal(9, tags.Count());
            }
        }
        [Fact]
        public async Task Read_returns_list_of_all_Subtags()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repo = new TagRepository(context);

                var tags = repo.ReadSubTags();

                Assert.Equal(51, tags.Count());
            }
        }

        [Fact]
        public async Task ReadTagsForCommunityAsync_returns_tags_for_community()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                await context.Communities.AddAsync(new Community
                {
                    Id = 89,
                    Name = "Bunnies",
                    Description = "Hello",
                    CommunityBaseTags = new[] {new CommunityBaseTag{CommunityId = 89, BaseTagId = 1}},
                    CommunitySubTags = new[] {new CommunitySubTag{CommunityId = 89, SubTagId = 4}}
                });
                await context.SaveChangesAsync();
                
                var repo = new TagRepository(context);

                var result = await repo.ReadTagsForCommunityAsync(89);

                Assert.Single(result.BaseTags);
                Assert.Single(result.SubTags);
                Assert.Equal(1, result.BaseTags.Single().Id);
                Assert.Equal(4, result.SubTags.Single().Id);
            }
        }

        [Fact]
        public async Task ReadTagsForCommunityAsync_returns_null_when_given_invalid_community_id()
        {
            using (var context = await TestHelper.CreateContextWithConnectionAsync())
            {
                var repo = new TagRepository(context);

                var result = await repo.ReadTagsForCommunityAsync(9999);

                Assert.Null(result);
            }
        }
    }
}