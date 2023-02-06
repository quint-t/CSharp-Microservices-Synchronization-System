using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SpecProfiles.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    EstablishmentDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Industry = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    City = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ShortInfo = table.Column<string>(type: "text", maxLength: 140, nullable: false),
                    AdditionalData = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Gender = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    City = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ShortInfo = table.Column<string>(type: "text", maxLength: 140, nullable: false),
                    FamilyStatus = table.Column<string>(type: "character varying(22)", maxLength: 22, nullable: false),
                    AdditionalData = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
