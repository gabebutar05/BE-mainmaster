using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class updateEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDetail_Employee_EmployeId",
                table: "EmployeeDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmployeeDetailLogTemp");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmployeeDetailLog");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "EmployeeDetailLogTemp",
                newName: "EmployeeTempId");

            migrationBuilder.RenameColumn(
                name: "EmployeDetailId",
                table: "EmployeeDetailLogTemp",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "EmployeeDetailLog",
                newName: "EmployeeTempId");

            migrationBuilder.RenameColumn(
                name: "EmployeDetailTempId",
                table: "EmployeeDetailLog",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "EmployeDetailId",
                table: "EmployeeDetailLog",
                newName: "EmployeeDetailTempId");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "EmployeeDetail",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeDetail_EmployeId",
                table: "EmployeeDetail",
                newName: "IX_EmployeeDetail_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "ActionDetail",
                table: "EmployeeDetailLogTemp",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeDetailId",
                table: "EmployeeDetailLogTemp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActionDetail",
                table: "EmployeeDetailLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeDetailId",
                table: "EmployeeDetailLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDetail_Employee_EmployeeId",
                table: "EmployeeDetail",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDetail_Employee_EmployeeId",
                table: "EmployeeDetail");

            migrationBuilder.DropColumn(
                name: "ActionDetail",
                table: "EmployeeDetailLogTemp");

            migrationBuilder.DropColumn(
                name: "EmployeeDetailId",
                table: "EmployeeDetailLogTemp");

            migrationBuilder.DropColumn(
                name: "ActionDetail",
                table: "EmployeeDetailLog");

            migrationBuilder.DropColumn(
                name: "EmployeeDetailId",
                table: "EmployeeDetailLog");

            migrationBuilder.RenameColumn(
                name: "EmployeeTempId",
                table: "EmployeeDetailLogTemp",
                newName: "EmployeId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeDetailLogTemp",
                newName: "EmployeDetailId");

            migrationBuilder.RenameColumn(
                name: "EmployeeTempId",
                table: "EmployeeDetailLog",
                newName: "EmployeId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeDetailLog",
                newName: "EmployeDetailTempId");

            migrationBuilder.RenameColumn(
                name: "EmployeeDetailTempId",
                table: "EmployeeDetailLog",
                newName: "EmployeDetailId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeDetail",
                newName: "EmployeId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeDetail_EmployeeId",
                table: "EmployeeDetail",
                newName: "IX_EmployeeDetail_EmployeId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EmployeeDetailLogTemp",
                type: "varchar(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EmployeeDetailLog",
                type: "varchar(2)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDetail_Employee_EmployeId",
                table: "EmployeeDetail",
                column: "EmployeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
