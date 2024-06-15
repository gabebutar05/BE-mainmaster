using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class employeeDBCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Initial = table.Column<string>(type: "varchar(50)", nullable: false),
                    Nationality = table.Column<int>(type: "integer", nullable: false),
                    IDType = table.Column<int>(type: "integer", nullable: false),
                    IDNo = table.Column<string>(type: "varchar(16)", nullable: false),
                    IDExpiredDate = table.Column<DateTime>(type: "date", nullable: false),
                    NPWP = table.Column<string>(type: "varchar(16)", nullable: false),
                    BirthPlace = table.Column<string>(type: "varchar(16)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Education = table.Column<string>(type: "varchar(16)", nullable: false),
                    MaritalStatus = table.Column<int>(type: "integer", nullable: false),
                    MailAddress1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    MailAddress2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    MailCityId = table.Column<int>(type: "integer", nullable: false),
                    MailZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    HomeAddress1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    HomeAddress2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    HomeCityId = table.Column<int>(type: "integer", nullable: false),
                    HomeZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    OtherEmail = table.Column<string>(type: "varchar(50)", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    MarketingPosition = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "varchar(25)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "date", nullable: false),
                    ResignDate = table.Column<DateTime>(type: "date", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", nullable: false),
                    AllowSaveToExcel = table.Column<int>(type: "integer", nullable: false),
                    PresDirectorXN1Signer = table.Column<int>(type: "integer", nullable: false),
                    WARPERDType = table.Column<int>(type: "integer", nullable: false),
                    WARPERDStatus = table.Column<int>(type: "integer", nullable: false),
                    HasPMPILicense = table.Column<int>(type: "integer", nullable: false),
                    PMPILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMPILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMIPWMILicense = table.Column<int>(type: "integer", nullable: false),
                    PMIPWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMIPWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWMILicense = table.Column<int>(type: "integer", nullable: false),
                    PMWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWMILicense = table.Column<int>(type: "integer", nullable: false),
                    BLWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMIPPEELicense = table.Column<int>(type: "integer", nullable: false),
                    PMIPPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMIPPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWPEELicense = table.Column<int>(type: "integer", nullable: false),
                    PMWPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWPEELicense = table.Column<int>(type: "integer", nullable: false),
                    BLWPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWPPELicense = table.Column<int>(type: "integer", nullable: false),
                    PMWPPELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWPPELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWPPELicense = table.Column<int>(type: "integer", nullable: false),
                    BLWPPELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWPPELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    PMWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    BLWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPPLWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    PPLWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PPLWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDetailLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeDetailId = table.Column<int>(type: "integer", nullable: false),
                    EmployeDetailTempId = table.Column<int>(type: "integer", nullable: false),
                    EmployeId = table.Column<int>(type: "integer", nullable: false),
                    LicenseId = table.Column<int>(type: "integer", nullable: false),
                    LicenseNo = table.Column<string>(type: "varchar(16)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDetailLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDetailLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeDetailId = table.Column<int>(type: "integer", nullable: false),
                    EmployeId = table.Column<int>(type: "integer", nullable: false),
                    LicenseId = table.Column<int>(type: "integer", nullable: false),
                    LicenseNo = table.Column<string>(type: "varchar(16)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDetailLogTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeTempId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Initial = table.Column<string>(type: "varchar(50)", nullable: false),
                    Nationality = table.Column<int>(type: "integer", nullable: false),
                    IDType = table.Column<int>(type: "integer", nullable: false),
                    IDNo = table.Column<string>(type: "varchar(16)", nullable: false),
                    IDExpiredDate = table.Column<DateTime>(type: "date", nullable: false),
                    NPWP = table.Column<string>(type: "varchar(16)", nullable: false),
                    BirthPlace = table.Column<string>(type: "varchar(16)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Education = table.Column<string>(type: "varchar(16)", nullable: false),
                    MaritalStatus = table.Column<int>(type: "integer", nullable: false),
                    MailAddress1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    MailAddress2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    MailCityId = table.Column<int>(type: "integer", nullable: false),
                    MailZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    HomeAddress1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    HomeAddress2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    HomeCityId = table.Column<int>(type: "integer", nullable: false),
                    HomeZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    OtherEmail = table.Column<string>(type: "varchar(50)", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    MarketingPosition = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "varchar(25)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "date", nullable: false),
                    ResignDate = table.Column<DateTime>(type: "date", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", nullable: false),
                    AllowSaveToExcel = table.Column<int>(type: "integer", nullable: false),
                    PresDirectorXN1Signer = table.Column<int>(type: "integer", nullable: false),
                    WARPERDType = table.Column<int>(type: "integer", nullable: false),
                    WARPERDStatus = table.Column<int>(type: "integer", nullable: false),
                    HasPMPILicense = table.Column<int>(type: "integer", nullable: false),
                    PMPILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMPILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMIPWMILicense = table.Column<int>(type: "integer", nullable: false),
                    PMIPWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMIPWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWMILicense = table.Column<int>(type: "integer", nullable: false),
                    PMWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWMILicense = table.Column<int>(type: "integer", nullable: false),
                    BLWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMIPPEELicense = table.Column<int>(type: "integer", nullable: false),
                    PMIPPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMIPPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWPEELicense = table.Column<int>(type: "integer", nullable: false),
                    PMWPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWPEELicense = table.Column<int>(type: "integer", nullable: false),
                    BLWPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWPPELicense = table.Column<int>(type: "integer", nullable: false),
                    PMWPPELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWPPELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWPPELicense = table.Column<int>(type: "integer", nullable: false),
                    BLWPPELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWPPELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    PMWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    BLWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPPLWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    PPLWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PPLWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLogTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Initial = table.Column<string>(type: "varchar(50)", nullable: false),
                    Nationality = table.Column<int>(type: "integer", nullable: false),
                    IDType = table.Column<int>(type: "integer", nullable: false),
                    IDNo = table.Column<string>(type: "varchar(16)", nullable: false),
                    IDExpiredDate = table.Column<DateTime>(type: "date", nullable: false),
                    NPWP = table.Column<string>(type: "varchar(16)", nullable: false),
                    BirthPlace = table.Column<string>(type: "varchar(16)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Education = table.Column<string>(type: "varchar(16)", nullable: false),
                    MaritalStatus = table.Column<int>(type: "integer", nullable: false),
                    MailAddress1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    MailAddress2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    MailCityId = table.Column<int>(type: "integer", nullable: false),
                    MailZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    HomeAddress1 = table.Column<string>(type: "varchar(50)", nullable: false),
                    HomeAddress2 = table.Column<string>(type: "varchar(50)", nullable: false),
                    HomeCityId = table.Column<int>(type: "integer", nullable: false),
                    HomeZipCodeId = table.Column<int>(type: "integer", nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    OtherEmail = table.Column<string>(type: "varchar(50)", nullable: false),
                    BranchId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    MarketingPosition = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "varchar(25)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "date", nullable: false),
                    ResignDate = table.Column<DateTime>(type: "date", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", nullable: false),
                    AllowSaveToExcel = table.Column<int>(type: "integer", nullable: false),
                    PresDirectorXN1Signer = table.Column<int>(type: "integer", nullable: false),
                    WARPERDType = table.Column<int>(type: "integer", nullable: false),
                    WARPERDStatus = table.Column<int>(type: "integer", nullable: false),
                    HasPMPILicense = table.Column<int>(type: "integer", nullable: false),
                    PMPILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMPILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMIPWMILicense = table.Column<int>(type: "integer", nullable: false),
                    PMIPWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMIPWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWMILicense = table.Column<int>(type: "integer", nullable: false),
                    PMWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWMILicense = table.Column<int>(type: "integer", nullable: false),
                    BLWMILicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWMILicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMIPPEELicense = table.Column<int>(type: "integer", nullable: false),
                    PMIPPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMIPPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWPEELicense = table.Column<int>(type: "integer", nullable: false),
                    PMWPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWPEELicense = table.Column<int>(type: "integer", nullable: false),
                    BLWPEELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWPEELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWPPELicense = table.Column<int>(type: "integer", nullable: false),
                    PMWPPELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWPPELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWPPELicense = table.Column<int>(type: "integer", nullable: false),
                    BLWPPELicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWPPELicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPMWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    PMWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PMWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasBLWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    BLWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    BLWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    HasPPLWAPERDLicense = table.Column<int>(type: "integer", nullable: false),
                    PPLWAPERDLicenseNo = table.Column<string>(type: "varchar(20)", nullable: false),
                    PPLWAPERDLicenseIssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false),
                    ActionRemarks = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLogTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeId = table.Column<int>(type: "integer", nullable: false),
                    LicenseId = table.Column<int>(type: "integer", nullable: false),
                    LicenseNo = table.Column<string>(type: "varchar(16)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "date", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(2)", nullable: false),
                    Action = table.Column<string>(type: "varchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDetail_Employee_EmployeId",
                        column: x => x.EmployeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeDetail_License_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "License",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetail_EmployeId",
                table: "EmployeeDetail",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDetail_LicenseId",
                table: "EmployeeDetail",
                column: "LicenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDetail");

            migrationBuilder.DropTable(
                name: "EmployeeDetailLog");

            migrationBuilder.DropTable(
                name: "EmployeeDetailLogTemp");

            migrationBuilder.DropTable(
                name: "EmployeeLog");

            migrationBuilder.DropTable(
                name: "EmployeeLogTemp");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
