using System;
using System.Data.Common;
using System.Threading.Tasks;
using CommunIT.Entities.Context;
using CommunIT.Shared.Portable.DTOs.Community;
using CommunIT.Shared.Portable.DTOs.Event;
using CommunIT.Shared.Portable.DTOs.User;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.Models.Tests
{
    public static class TestHelper
    {
        public static async Task<DbConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            return connection;
        }

        public static async Task<ICommunITContext> CreateContextAsync(DbConnection connection)
        {
            var builder = new DbContextOptionsBuilder<CommunITContext>()
                .UseSqlite(connection);

            var context = new CommunITContext(builder.Options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        public static async Task<ICommunITContext> CreateContextWithConnectionAsync()
        {
            var connection = await CreateConnectionAsync();
            return await CreateContextAsync(connection);
        }

        public static async Task<CommunityDetailDto> CreateTestCommunityAsync(ICommunITContext context)
        {
            var adminUser = await context.Users.AddAsync(new Entities.User { Username = "admin@itu.dk" });
            await context.SaveChangesAsync();

            var community = new CommunityCreateDto
            {
                Name = "Bunnies4Life",
                Description = "Community for bunny-lovers",
                BaseTagIds = new[] { 1 },
                SubTagIds = new[] { 4 },
                CreatedBy = adminUser.Entity.Username
            };

            var repository = new CommunityRepository(context);

            return await repository.CreateAsync(community);
        }

        public static async Task<UserDetailDto> CreateTestUserAsync(ICommunITContext context)
        {
            var dto = new UserCreateDto
            {
                Username = "mail@user.com",
                DisplayName = "Longjohn1337",
                Bio = "I like cookies and cake"
            };

            var repository = new UserRepository(context);

            return await repository.CreateAsync(dto);
        }
    }
}