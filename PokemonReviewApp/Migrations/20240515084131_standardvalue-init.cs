using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class standardvalueinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StandardValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataName = table.Column<string>(type: "varchar(125)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ValueType = table.Column<string>(type: "varchar(2)", nullable: false),
                    ValueOption = table.Column<string>(type: "text", nullable: true),
                    DataValue = table.Column<string>(type: "text", nullable: false),
                    ValueInPercentage = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardValue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardValueLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataId = table.Column<int>(type: "integer", nullable: false),
                    DataTempId = table.Column<int>(type: "integer", nullable: false),
                    DataName = table.Column<string>(type: "varchar(125)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ValueType = table.Column<string>(type: "varchar(2)", nullable: false),
                    ValueOption = table.Column<string>(type: "text", nullable: true),
                    DataValue = table.Column<string>(type: "text", nullable: false),
                    ValueInPercentage = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ActionRemarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardValueLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardValueLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataId = table.Column<int>(type: "integer", nullable: false),
                    DataName = table.Column<string>(type: "varchar(125)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ValueType = table.Column<string>(type: "varchar(2)", nullable: false),
                    ValueOption = table.Column<string>(type: "text", nullable: true),
                    DataValue = table.Column<string>(type: "text", nullable: false),
                    ValueInPercentage = table.Column<bool>(type: "boolean", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    ActionRemarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardValueLogTemp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardValue_DataName",
                table: "StandardValue",
                column: "DataName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StandardValueLogTemp_DataName",
                table: "StandardValueLogTemp",
                column: "DataName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandardValue");

            migrationBuilder.DropTable(
                name: "StandardValueLog");

            migrationBuilder.DropTable(
                name: "StandardValueLogTemp");
        }
    }
}
