using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Branch_BranchId",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_BranchId",
                table: "Department",
                newName: "IX_Department_BranchId");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Department",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Department",
                type: "varchar(2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Department",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                table: "Department",
                type: "varchar(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Department",
                type: "varchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Department",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Branch_BranchId",
                table: "Department",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Branch_BranchId",
                table: "Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameIndex(
                name: "IX_Department_BranchId",
                table: "Departments",
                newName: "IX_Departments_BranchId");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Departments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Departments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Departments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentCode",
                table: "Departments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(6)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Branch_BranchId",
                table: "Departments",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
