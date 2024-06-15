using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class updateFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchLogTemp_City_CityId",
                table: "BranchLogTemp");

            migrationBuilder.DropForeignKey(
                name: "FK_ZipCode_City_CityId",
                table: "ZipCode");

            migrationBuilder.DropIndex(
                name: "IX_BranchLogTemp_CityId",
                table: "BranchLogTemp");

            migrationBuilder.DropColumn(
                name: "City",
                table: "BranchLog");

            migrationBuilder.AddColumn<int>(
                name: "ZipCodeId",
                table: "BranchLogTemp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "BranchLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZipCodeId",
                table: "BranchLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZipCodeId",
                table: "Branch",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Branch_ZipCodeId",
                table: "Branch",
                column: "ZipCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_ZipCode_ZipCodeId",
                table: "Branch",
                column: "ZipCodeId",
                principalTable: "ZipCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ZipCode_City_CityId",
                table: "ZipCode",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_ZipCode_ZipCodeId",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_ZipCode_City_CityId",
                table: "ZipCode");

            migrationBuilder.DropIndex(
                name: "IX_Branch_ZipCodeId",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "ZipCodeId",
                table: "BranchLogTemp");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "BranchLog");

            migrationBuilder.DropColumn(
                name: "ZipCodeId",
                table: "BranchLog");

            migrationBuilder.DropColumn(
                name: "ZipCodeId",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "BranchLog",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLogTemp_CityId",
                table: "BranchLogTemp",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchLogTemp_City_CityId",
                table: "BranchLogTemp",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZipCode_City_CityId",
                table: "ZipCode",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
