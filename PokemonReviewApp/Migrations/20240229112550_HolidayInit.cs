using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class HolidayInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HolidayDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "varchar(125)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HolidayId = table.Column<int>(type: "integer", nullable: false),
                    HolidayTempId = table.Column<int>(type: "integer", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "varchar(125)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ActionRemarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HolidayId = table.Column<int>(type: "integer", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "varchar(125)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ActionRemarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayLogTemp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayDate",
                table: "Holiday",
                column: "HolidayDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HolidayLogTemp_HolidayDate",
                table: "HolidayLogTemp",
                column: "HolidayDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "HolidayLog");

            migrationBuilder.DropTable(
                name: "HolidayLogTemp");
        }
    }
}
