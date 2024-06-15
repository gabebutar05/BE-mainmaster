using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class MigrationCompanyFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Company_CityId",
                table: "Company",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CountryId",
                table: "Company",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ZipCodeId",
                table: "Company",
                column: "ZipCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_City_CityId",
                table: "Company",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Country_CountryId",
                table: "Company",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_ZipCode_ZipCodeId",
                table: "Company",
                column: "ZipCodeId",
                principalTable: "ZipCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_City_CityId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Country_CountryId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_ZipCode_ZipCodeId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_CityId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_CountryId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_ZipCodeId",
                table: "Company");
        }
    }
}
