using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PunchApiProject.Migrations.PunchDb
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginRecord_Employees_EmployeeId",
                table: "LoginRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginRecord",
                table: "LoginRecord");

            migrationBuilder.RenameTable(
                name: "LoginRecord",
                newName: "LoginRecords");

            migrationBuilder.RenameIndex(
                name: "IX_LoginRecord_EmployeeId",
                table: "LoginRecords",
                newName: "IX_LoginRecords_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginRecords",
                table: "LoginRecords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginRecords_Employees_EmployeeId",
                table: "LoginRecords",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginRecords_Employees_EmployeeId",
                table: "LoginRecords");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Username",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginRecords",
                table: "LoginRecords");

            migrationBuilder.RenameTable(
                name: "LoginRecords",
                newName: "LoginRecord");

            migrationBuilder.RenameIndex(
                name: "IX_LoginRecords_EmployeeId",
                table: "LoginRecord",
                newName: "IX_LoginRecord_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginRecord",
                table: "LoginRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginRecord_Employees_EmployeeId",
                table: "LoginRecord",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
