using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class department1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentTempId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    DepartmentName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    DepartmentName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLogTemp", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentLog");

            migrationBuilder.DropTable(
                name: "DepartmentLogTemp");
        }
    }
}
