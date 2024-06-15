using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class citychanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auths",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auths", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BranchLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    BranchTempId = table.Column<int>(type: "integer", nullable: false),
                    BranchCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    BranchName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    City = table.Column<string>(type: "varchar(50)", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    CityName = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Key",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Keys = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Key", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchCode = table.Column<string>(type: "varchar(6)", nullable: false),
                    BranchName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BranchLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    BranchCode = table.Column<string>(type: "text", nullable: false),
                    BranchName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    ContactPerson = table.Column<string>(type: "varchar(50)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    Fax = table.Column<string>(type: "varchar(20)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLogTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchLogTemp_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZipCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    ZipCode = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZipCode_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentCode = table.Column<string>(type: "text", nullable: false),
                    DepartmentName = table.Column<string>(type: "text", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CityId",
                table: "Branch",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLogTemp_CityId",
                table: "BranchLogTemp",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_BranchId",
                table: "Departments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ZipCode_CityId",
                table: "ZipCode",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auths");

            migrationBuilder.DropTable(
                name: "BranchLog");

            migrationBuilder.DropTable(
                name: "BranchLogTemp");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Key");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "ZipCode");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
