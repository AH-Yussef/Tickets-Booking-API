using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketsBooking.Infrastructure.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(767)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(767)", nullable: false),
                    Accepted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "EventProviders",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Bio = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    WebsiteLink = table.Column<string>(type: "varchar(767)", nullable: true),
                    Verified = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventProviders", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Keyword = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Keyword);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ProviderName = table.Column<string>(type: "varchar(200)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    AllTickets = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SingleTicketPrice = table.Column<float>(type: "float", nullable: false),
                    BoughtTickets = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReservationDueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    SubCategory = table.Column<string>(type: "text", nullable: true),
                    Accepted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_EventProviders_ProviderName",
                        column: x => x.ProviderName,
                        principalTable: "EventProviders",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Link = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    EventProviderName = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Link);
                    table.ForeignKey(
                        name: "FK_SocialMedias_EventProviders_EventProviderName",
                        column: x => x.EventProviderName,
                        principalTable: "EventProviders",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTag",
                columns: table => new
                {
                    EventsEventID = table.Column<string>(type: "varchar(200)", nullable: false),
                    TagsKeyword = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTag", x => new { x.EventsEventID, x.TagsKeyword });
                    table.ForeignKey(
                        name: "FK_EventTag_Events_EventsEventID",
                        column: x => x.EventsEventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTag_Tags_TagsKeyword",
                        column: x => x.TagsKeyword,
                        principalTable: "Tags",
                        principalColumn: "Keyword",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(767)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Team = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    EventID = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventProviders_WebsiteLink",
                table: "EventProviders",
                column: "WebsiteLink",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ProviderName",
                table: "Events",
                column: "ProviderName");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_TagsKeyword",
                table: "EventTag",
                column: "TagsKeyword");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventID",
                table: "Participants",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_EventProviderName",
                table: "SocialMedias",
                column: "EventProviderName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "EventTag");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventProviders");
        }
    }
}
