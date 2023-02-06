using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpecAnnouncer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    UniqueName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.UniqueName);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    UniqueName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Url = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.UniqueName);
                });

            migrationBuilder.CreateTable(
                name: "EventsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    EventUniqueName = table.Column<string>(type: "character varying(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventsHistory_Events_EventUniqueName",
                        column: x => x.EventUniqueName,
                        principalTable: "Events",
                        principalColumn: "UniqueName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscribersSignatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    SubscriberUniqueName = table.Column<string>(type: "character varying(32)", nullable: false),
                    EventUniqueName = table.Column<string>(type: "character varying(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribersSignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscribersSignatures_Events_EventUniqueName",
                        column: x => x.EventUniqueName,
                        principalTable: "Events",
                        principalColumn: "UniqueName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscribersSignatures_Subscribers_SubscriberUniqueName",
                        column: x => x.SubscriberUniqueName,
                        principalTable: "Subscribers",
                        principalColumn: "UniqueName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponsesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponseReceived = table.Column<bool>(type: "boolean", nullable: false),
                    SubscriberUniqueName = table.Column<string>(type: "character varying(32)", nullable: false),
                    EventHistoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponsesHistory_EventsHistory_EventHistoryId",
                        column: x => x.EventHistoryId,
                        principalTable: "EventsHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponsesHistory_Subscribers_SubscriberUniqueName",
                        column: x => x.SubscriberUniqueName,
                        principalTable: "Subscribers",
                        principalColumn: "UniqueName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventsHistory_EventUniqueName",
                table: "EventsHistory",
                column: "EventUniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsesHistory_EventHistoryId",
                table: "ResponsesHistory",
                column: "EventHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsesHistory_SubscriberUniqueName",
                table: "ResponsesHistory",
                column: "SubscriberUniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribersSignatures_EventUniqueName",
                table: "SubscribersSignatures",
                column: "EventUniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribersSignatures_SubscriberUniqueName",
                table: "SubscribersSignatures",
                column: "SubscriberUniqueName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponsesHistory");

            migrationBuilder.DropTable(
                name: "SubscribersSignatures");

            migrationBuilder.DropTable(
                name: "EventsHistory");

            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
