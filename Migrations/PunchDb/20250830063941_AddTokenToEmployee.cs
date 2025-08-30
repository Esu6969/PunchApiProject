using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PunchApiProject.Migrations.PunchDb
{
    /// <inheritdoc />
    public partial class AddTokenToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginRecord_Employee_EmployeeId",
                table: "LoginRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Punches_Employee_EmployeeId",
                table: "Punches");

            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecord_Employee_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginRecord_Employees_EmployeeId",
                table: "LoginRecord",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Punches_Employees_EmployeeId",
                table: "Punches",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginRecord_Employees_EmployeeId",
                table: "LoginRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Punches_Employees_EmployeeId",
                table: "Punches");

            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginRecord_Employee_EmployeeId",
                table: "LoginRecord",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Punches_Employee_EmployeeId",
                table: "Punches",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PunchRecord_Employee_EmployeeId",
                table: "PunchRecord",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
