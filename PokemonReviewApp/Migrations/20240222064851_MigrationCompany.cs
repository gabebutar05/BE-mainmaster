using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class MigrationCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Npwp = table.Column<string>(type: "varchar(20)", nullable: false),
                    NpwpDate = table.Column<DateTime>(type: "date", nullable: false),
                    KseiCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    SinvestSaCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    SinvestMiCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", nullable: false),
                    Address2 = table.Column<string>(type: "varchar(255)", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    ZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CompanyCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Npwp = table.Column<string>(type: "varchar(20)", nullable: false),
                    NpwpDate = table.Column<DateTime>(type: "date", nullable: false),
                    KseiCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    SinvestSaCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    SinvestMiCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", nullable: false),
                    Address2 = table.Column<string>(type: "varchar(255)", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    ZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    ActionRemarks = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CompanyCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Npwp = table.Column<string>(type: "varchar(20)", nullable: false),
                    NpwpDate = table.Column<DateTime>(type: "date", nullable: false),
                    KseiCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    SinvestSaCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    SinvestMiCode = table.Column<string>(type: "varchar(15)", nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", nullable: false),
                    Address2 = table.Column<string>(type: "varchar(255)", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    ZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: false),
                    ActionRemarks = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLogTemp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_CompanyCode",
                table: "Company",
                column: "CompanyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLogTemp_CompanyCode",
                table: "CompanyLogTemp",
                column: "CompanyCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "CompanyLog");

            migrationBuilder.DropTable(
                name: "CompanyLogTemp");
        }
    }
}
