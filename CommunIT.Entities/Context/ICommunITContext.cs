using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CommunIT.Entities.Context
{
    public interface ICommunITContext : IDisposable
    {
        DbSet<Comment> Comments { get; set; }
        DbSet<Community> Communities { get; set; }
        DbSet<CommunityBaseTag> CommunityBaseTags { get; set; }
        DbSet<CommunitySubTag> CommunitySubTags { get; set; }
        DbSet<CommunityUser> CommunityUsers { get; set; }
        DbSet<Event> Events { get; set; }
        DbSet<EventUser> EventUsers { get; set; }
        DbSet<Forum> Forums { get; set; }
        DbSet<BaseTag> BaseTags { get; set; }
        DbSet<SubTag> SubTags { get; set; }
        DbSet<Thread> Threads { get; set; }
        DbSet<User> Users { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
