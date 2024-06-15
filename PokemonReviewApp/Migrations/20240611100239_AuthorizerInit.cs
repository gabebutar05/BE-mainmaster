using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizerInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorizer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizer_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerDownline",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerModuleId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerDownline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerDownlineLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerDownlineId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerDownlineTempId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerModuleId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerDownlineLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerDownlineLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerDownlineId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerModuleId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerDownlineLogTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerTempId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ActionRemarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ActionRemarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerLogTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerId = table.Column<int>(type: "integer", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerModule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerModuleLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerModuleId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerModuleTempId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerId = table.Column<int>(type: "integer", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerModuleLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizerModuleLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorizerModuleId = table.Column<int>(type: "integer", nullable: false),
                    AuthorizerId = table.Column<int>(type: "integer", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizerModuleLogTemp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorizer_EmployeeId",
                table: "Authorizer",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizerDownline_AuthorizerModuleId_EmployeeId",
                table: "AuthorizerDownline",
                columns: new[] { "AuthorizerModuleId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizerModule_AuthorizerId_ModuleId",
                table: "AuthorizerModule",
                columns: new[] { "AuthorizerId", "ModuleId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorizer");

            migrationBuilder.DropTable(
                name: "AuthorizerDownline");

            migrationBuilder.DropTable(
                name: "AuthorizerDownlineLog");

            migrationBuilder.DropTable(
                name: "AuthorizerDownlineLogTemp");

            migrationBuilder.DropTable(
                name: "AuthorizerLog");

            migrationBuilder.DropTable(
                name: "AuthorizerLogTemp");

            migrationBuilder.DropTable(
                name: "AuthorizerModule");

            migrationBuilder.DropTable(
                name: "AuthorizerModuleLog");

            migrationBuilder.DropTable(
                name: "AuthorizerModuleLogTemp");
        }
    }
}
