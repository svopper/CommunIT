using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommunIT.Entities.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    University = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "CommunityBaseTags",
                columns: table => new
                {
                    CommunityId = table.Column<int>(nullable: false),
                    BaseTagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityBaseTags", x => new { x.CommunityId, x.BaseTagId });
                    table.ForeignKey(
                        name: "FK_CommunityBaseTags_BaseTags_BaseTagId",
                        column: x => x.BaseTagId,
                        principalTable: "BaseTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityBaseTags_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CommunityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    CommunityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forums_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunitySubTags",
                columns: table => new
                {
                    CommunityId = table.Column<int>(nullable: false),
                    SubTagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunitySubTags", x => new { x.CommunityId, x.SubTagId });
                    table.ForeignKey(
                        name: "FK_CommunitySubTags_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunitySubTags_SubTags_SubTagId",
                        column: x => x.SubTagId,
                        principalTable: "SubTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    CommunityId = table.Column<int>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsFavorite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityUsers", x => new { x.CommunityId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommunityUsers_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventUsers",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUsers", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventUsers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ForumId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threads_Forums_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Threads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ThreadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BaseTags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Career" },
                    { 2, "Creativity" },
                    { 3, "Culture" },
                    { 4, "Debate" },
                    { 5, "Health" },
                    { 6, "Life style" },
                    { 7, "Nature" },
                    { 8, "Sports" },
                    { 9, "Online life" }
                });

            migrationBuilder.InsertData(
                table: "Communities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 6, "What an idiot..", "Who makes all of these communities?" },
                    { 5, "This is just a Reddit rip off, anyway", "God, there is a lot of dumb communities" },
                    { 4, "Close this shit..", "This application is a Reddit rip off!" },
                    { 2, "Bunnies that play football is disgusting!", "Bunny football haters" },
                    { 1, "We love bunnies that play football", "Bunny football lovers Com 1" },
                    { 3, "It's cold outside - so wear a jacket, goddammit!", "Your mom's community" }
                });

            migrationBuilder.InsertData(
                table: "SubTags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 38, "Hiking" },
                    { 29, "Podcast" },
                    { 30, "Interior design" },
                    { 31, "Beauty" },
                    { 32, "Treatments" },
                    { 33, "Fashion" },
                    { 34, "Gossip" },
                    { 35, "Diets" },
                    { 36, "Agriculture" },
                    { 37, "Forest" },
                    { 39, "Animals" },
                    { 47, "Equipment" },
                    { 41, "ecology" },
                    { 42, "Championships" },
                    { 43, "Hobby sports" },
                    { 44, "Sport" },
                    { 45, "Hooligans" },
                    { 46, "Roligan" },
                    { 28, "Mindfullness" },
                    { 48, "Betting" },
                    { 49, "Gaming" },
                    { 50, "Programming" },
                    { 51, "VR" },
                    { 40, "Climate" },
                    { 27, "Diets" },
                    { 20, "Religion" },
                    { 25, "Fitness" },
                    { 1, "Job" },
                    { 2, "Education" },
                    { 3, "Payment" },
                    { 4, "Applications" },
                    { 5, "Union" },
                    { 6, "Coaching" },
                    { 7, "Crochet" },
                    { 8, "Knitting" },
                    { 9, "Sewing" },
                    { 10, "Drawing" },
                    { 26, "Psychology" },
                    { 12, "Art" },
                    { 11, "Painting" },
                    { 14, "Architecture" },
                    { 15, "Culture activities" },
                    { 16, "Literature" },
                    { 17, "Politics" },
                    { 18, "Philosophy" },
                    { 19, "Science" },
                    { 21, "Environment" },
                    { 22, "Recipe" },
                    { 23, "Medicin" },
                    { 24, "Diseases" },
                    { 13, "Literature" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "Bio", "Created", "DisplayName", "University" },
                values: new object[,]
                {
                    { "emfi@itu.dk", "lol wtf", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), "emfi", null },
                    { "kols@itu.dk", "I like cake and cookies", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), "Longjohn69", null },
                    { "amle@itu.dk", "Hello, World", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), "amle", null },
                    { "tosk@itu.dk", "What the fuck is going on", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), "tosk", null }
                });

            migrationBuilder.InsertData(
                table: "CommunityBaseTags",
                columns: new[] { "CommunityId", "BaseTagId" },
                values: new object[] { 1, 8 });

            migrationBuilder.InsertData(
                table: "CommunitySubTags",
                columns: new[] { "CommunityId", "SubTagId" },
                values: new object[] { 1, 43 });

            migrationBuilder.InsertData(
                table: "CommunityUsers",
                columns: new[] { "CommunityId", "UserId", "DateJoined", "IsAdmin", "IsFavorite" },
                values: new object[,]
                {
                    { 1, "kols@itu.dk", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), true, false },
                    { 1, "amle@itu.dk", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), false, false },
                    { 1, "tosk@itu.dk", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), false, false }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CommunityId", "Date", "Description", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2018, 12, 20, 0, 0, 0, 0, DateTimeKind.Local), "Ever wanted your bunnny to play like Cristiano Ronaldo? Meet us in the forest on on the spcified date", "Learn your bunny to do a scissor kick" },
                    { 2, 1, new DateTime(2018, 12, 21, 0, 0, 0, 0, DateTimeKind.Local), "Hungry?", "Bunny slaughtering for beginners" }
                });

            migrationBuilder.InsertData(
                table: "Forums",
                columns: new[] { "Id", "CommunityId", "Created", "Description", "Name" },
                values: new object[] { 1, 1, new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), "Do they all like carrots?", "How to feed your bunny" });

            migrationBuilder.InsertData(
                table: "EventUsers",
                columns: new[] { "EventId", "UserId" },
                values: new object[,]
                {
                    { 1, "amle@itu.dk" },
                    { 1, "tosk@itu.dk" },
                    { 2, "kols@itu.dk" },
                    { 2, "tosk@itu.dk" }
                });

            migrationBuilder.InsertData(
                table: "Threads",
                columns: new[] { "Id", "Content", "Created", "ForumId", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Was just wondering", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), 1, "Can they drink bleach?", "kols@itu.dk" },
                    { 2, "pl0x answer", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), 1, "What do you feed?", "amle@itu.dk" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "Created", "ThreadId", "UserId" },
                values: new object[] { 1, "sure", new DateTime(2018, 12, 18, 0, 0, 0, 0, DateTimeKind.Local), 1, "kols@itu.dk" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ThreadId",
                table: "Comments",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityBaseTags_BaseTagId",
                table: "CommunityBaseTags",
                column: "BaseTagId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunitySubTags_SubTagId",
                table: "CommunitySubTags",
                column: "SubTagId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUsers_UserId",
                table: "CommunityUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CommunityId",
                table: "Events",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_EventUsers_UserId",
                table: "EventUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_CommunityId",
                table: "Forums",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ForumId",
                table: "Threads",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_UserId",
                table: "Threads",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CommunityBaseTags");

            migrationBuilder.DropTable(
                name: "CommunitySubTags");

            migrationBuilder.DropTable(
                name: "CommunityUsers");

            migrationBuilder.DropTable(
                name: "EventUsers");

            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "BaseTags");

            migrationBuilder.DropTable(
                name: "SubTags");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Communities");
        }
    }
}
