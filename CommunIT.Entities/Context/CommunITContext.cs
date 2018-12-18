using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CommunIT.Entities.Context
{
    public class CommunITContext : DbContext, ICommunITContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityBaseTag> CommunityBaseTags { get; set; }
        public DbSet<CommunitySubTag> CommunitySubTags { get; set; }
        public DbSet<CommunityUser> CommunityUsers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventUser> EventUsers { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BaseTag> BaseTags { get; set; }
        public DbSet<SubTag> SubTags { get; set; }

        public CommunITContext()
        {

        }

        public CommunITContext(DbContextOptions<CommunITContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                const string localDb = @"Server=(localdb)\MSSQLLocalDB;Database=CommunIT;Trusted_Connection=True;MultipleActiveResultSets=true";
                const string kasperMacDev = @"Server=127.0.0.1,1433;Database=CommunIT;User Id=SA;Password=KalleKollekt123";

                const string connectionString = localDb;

                optionsBuilder.UseSqlServer(connectionString);
                //optionsBuilder.UseSqlite(sqlinmem);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityUser>()
                .HasKey(cu => new { cu.CommunityId, cu.UserId });
            modelBuilder.Entity<CommunityBaseTag>()
                .HasKey(cb => new { cb.CommunityId, cb.BaseTagId });
            modelBuilder.Entity<CommunitySubTag>()
                .HasKey(cb => new { cb.CommunityId, cb.SubTagId });
            modelBuilder.Entity<EventUser>()
                .HasKey(eu => new { eu.EventId, eu.UserId });

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasData(
                new User { Username = "kols@itu.dk", Bio = "I like cake and cookies", DisplayName = "Longjohn69", Created = DateTime.Today },
                new User { Username = "amle@itu.dk", Bio = "Hello, World", DisplayName = "amle", Created = DateTime.Today },
                new User { Username = "emfi@itu.dk", Bio = "lol wtf", DisplayName = "emfi", Created = DateTime.Today },
                new User { Username = "tosk@itu.dk", Bio = "What the fuck is going on", DisplayName = "tosk", Created = DateTime.Today }
            );

            modelBuilder.Entity<Community>().HasData(
                new Community
                {
                    Id = 1,
                    Name = "Bunny football lovers Com 1",
                    Description = "We love bunnies that play football",
                },
                new Community
                {
                    Id = 2,
                    Name = "Bunny football haters",
                    Description = "Bunnies that play football is disgusting!"
                },
                new Community
                {
                    Id = 3,
                    Name = "Your mom's community",
                    Description = "It's cold outside - so wear a jacket, goddammit!"
                },
                new Community
                {
                    Id = 4,
                    Name = "This application is a Reddit rip off!",
                    Description = "Close this shit.."
                },
                new Community
                {
                    Id = 5,
                    Name = "God, there is a lot of dumb communities",
                    Description = "This is just a Reddit rip off, anyway"
                },
                new Community
                {
                    Id = 6,
                    Name = "Who makes all of these communities?",
                    Description = "What an idiot.."
                }
            );

            modelBuilder.Entity<CommunityUser>().HasData(
                new CommunityUser
                {
                    UserId = "kols@itu.dk",
                    CommunityId = 1,
                    DateJoined = DateTime.Today,
                    IsAdmin = true
                },
                new CommunityUser
                {
                    UserId = "amle@itu.dk",
                    CommunityId = 1,
                    DateJoined = DateTime.Today
                },
                new CommunityUser
                {
                    UserId = "tosk@itu.dk",
                    CommunityId = 1,
                    DateJoined = DateTime.Today
                }
            );

            modelBuilder.Entity<Forum>().HasData(
                new Forum
                {
                    Id = 1,
                    Name = "How to feed your bunny",
                    Description = "Do they all like carrots?",
                    Created = DateTime.Today,
                    CommunityId = 1
                }
            );

            modelBuilder.Entity<Thread>().HasData(
                new Thread
                {
                    Id = 1,
                    Created = DateTime.Today,
                    ForumId = 1,
                    Content = "Was just wondering",
                    Title = "Can they drink bleach?",
                    UserId = "kols@itu.dk"
                },
                new Thread
                {
                    Id = 2,
                    Created = DateTime.Today,
                    ForumId = 1,
                    Content = "pl0x answer",
                    Title = "What do you feed?",
                    UserId = "amle@itu.dk"
                }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Content = "sure",
                    ThreadId = 1,
                    UserId = "kols@itu.dk",
                    Created = DateTime.Today
                }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Learn your bunny to do a scissor kick",
                    Date = DateTime.Today.AddDays(2),
                    Description = "Ever wanted your bunnny to play like Cristiano Ronaldo? Meet us in the forest on on the spcified date",
                    CommunityId = 1
                },
                new Event
                {
                    Id = 2,
                    Title = "Bunny slaughtering for beginners",
                    Date = DateTime.Today.AddDays(3),
                    Description = "Hungry?",
                    CommunityId = 1
                }
            );

            modelBuilder.Entity<EventUser>().HasData(
                //new EventUser { EventId = 1, UserId = "kols@itu.dk" },
                new EventUser { EventId = 1, UserId = "amle@itu.dk" },
                new EventUser { EventId = 2, UserId = "kols@itu.dk" },
                new EventUser { EventId = 1, UserId = "tosk@itu.dk" },
                new EventUser { EventId = 2, UserId = "tosk@itu.dk" }
            );

            modelBuilder.Entity<BaseTag>().HasData(
                new BaseTag { Id = 1, Name = "Career" },
                new BaseTag { Id = 2, Name = "Creativity" },
                new BaseTag { Id = 3, Name = "Culture" },
                new BaseTag { Id = 4, Name = "Debate" },
                new BaseTag { Id = 5, Name = "Health" },
                new BaseTag { Id = 6, Name = "Life style" },
                new BaseTag { Id = 7, Name = "Nature" },
                new BaseTag { Id = 8, Name = "Sports"},
                new BaseTag { Id = 9, Name = "Online life"}
            );

            modelBuilder.Entity<SubTag>().HasData(
                //Career
                new SubTag { Id = 1, Name = "Job" },
                new SubTag { Id = 2, Name = "Education" },
                new SubTag { Id = 3, Name = "Payment" },
                new SubTag { Id = 4, Name = "Applications"},
                new SubTag { Id = 5, Name = "Union" },
                new SubTag { Id = 6, Name = "Coaching" },
                //Creativity
                new SubTag { Id = 7, Name = "Crochet" },
                new SubTag { Id = 8, Name = "Knitting" },
                new SubTag { Id = 9, Name = "Sewing" },
                new SubTag { Id = 10, Name = "Drawing" },
                new SubTag { Id = 11, Name = "Painting" },
                //Culture
                new SubTag { Id = 12, Name = "Art" },
                new SubTag { Id = 13, Name = "Literature" },
                new SubTag { Id = 14, Name = "Architecture" },
                new SubTag { Id = 15, Name = "Culture activities" },
                new SubTag { Id = 16, Name = "Literature" },
                //Debate
                new SubTag { Id = 17, Name = "Politics" },
                new SubTag { Id = 18, Name = "Philosophy" },
                new SubTag { Id = 19, Name = "Science" },
                new SubTag { Id = 20, Name = "Religion" },
                new SubTag { Id = 21, Name = "Environment" },
                //Health
                new SubTag { Id = 22, Name = "Recipe" },
                new SubTag { Id = 23, Name = "Medicin" },
                new SubTag { Id = 24, Name = "Diseases" },
                new SubTag { Id = 25, Name = "Fitness" },
                new SubTag { Id = 26, Name = "Psychology" },
                new SubTag { Id = 27, Name = "Diets" },
                new SubTag { Id = 28, Name = "Mindfullness" },
                //Life style
                new SubTag { Id = 29, Name = "Podcast" },
                new SubTag { Id = 30, Name = "Interior design" },
                new SubTag { Id = 31, Name = "Beauty" },
                new SubTag { Id = 32, Name = "Treatments" },
                new SubTag { Id = 33, Name = "Fashion" },
                new SubTag { Id = 34, Name = "Gossip" },
                new SubTag { Id = 35, Name = "Diets" },
                //Nature
                new SubTag { Id = 36, Name = "Agriculture" },
                new SubTag { Id = 37, Name = "Forest" },
                new SubTag { Id = 38, Name = "Hiking" },
                new SubTag { Id = 39, Name = "Animals" },
                new SubTag { Id = 40, Name = "Climate" },
                new SubTag { Id = 41, Name = "ecology" },
                //Sports
                new SubTag { Id = 42, Name = "Championships" },
                new SubTag { Id = 43, Name = "Hobby sports" },
                new SubTag { Id = 44, Name = "Sport" },
                new SubTag { Id = 45, Name = "Hooligans" },
                new SubTag { Id = 46, Name = "Roligan" },
                new SubTag { Id = 47, Name = "Equipment" },
                new SubTag { Id = 48, Name = "Betting" },
                //OnlineLife
                new SubTag { Id = 49, Name = "Gaming" },
                new SubTag { Id = 50, Name = "Programming" },
                new SubTag { Id = 51, Name = "VR" }
            );

            modelBuilder.Entity<CommunityBaseTag>().HasData(
                new CommunityBaseTag { CommunityId = 1, BaseTagId = 8 }
            );

            modelBuilder.Entity<CommunitySubTag>().HasData(
                new CommunitySubTag { CommunityId = 1, SubTagId = 43 }
            );

        }
    }
}
