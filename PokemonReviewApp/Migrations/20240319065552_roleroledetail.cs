using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class roleroledetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleDetailLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Run = table.Column<bool>(type: "boolean", nullable: false),
                    Add = table.Column<bool>(type: "boolean", nullable: false),
                    Update = table.Column<bool>(type: "boolean", nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    RoleDetailId = table.Column<int>(type: "integer", nullable: false),
                    MenuId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDetailLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleDetailLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Run = table.Column<bool>(type: "boolean", nullable: false),
                    Add = table.Column<bool>(type: "boolean", nullable: false),
                    Update = table.Column<bool>(type: "boolean", nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    RoleDetailId = table.Column<int>(type: "integer", nullable: false),
                    MenuId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDetailLogTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleLogTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Run = table.Column<bool>(type: "boolean", nullable: false),
                    Add = table.Column<bool>(type: "boolean", nullable: false),
                    Update = table.Column<bool>(type: "boolean", nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    MenuId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleDetail_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleDetail_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_ModuleId",
                table: "Role",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDetail_MenuId",
                table: "RoleDetail",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleDetail_RoleId",
                table: "RoleDetail",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleDetail");

            migrationBuilder.DropTable(
                name: "RoleDetailLog");

            migrationBuilder.DropTable(
                name: "RoleDetailLogTemp");

            migrationBuilder.DropTable(
                name: "RoleLog");

            migrationBuilder.DropTable(
                name: "RoleLogTemp");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
