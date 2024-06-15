using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class addlogroleid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleTempId",
                table: "RoleLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "RoleDetailLogTemp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActionDetail",
                table: "RoleDetailLogTemp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "RoleDetailLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActionDetail",
                table: "RoleDetailLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "RoleDetail",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActionDetail",
                table: "RoleDetail",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleTempId",
                table: "RoleLog");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "RoleDetailLogTemp");

            migrationBuilder.DropColumn(
                name: "ActionDetail",
                table: "RoleDetailLogTemp");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "RoleDetailLog");

            migrationBuilder.DropColumn(
                name: "ActionDetail",
                table: "RoleDetailLog");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "RoleDetail");

            migrationBuilder.DropColumn(
                name: "ActionDetail",
                table: "RoleDetail");
        }
    }
}
