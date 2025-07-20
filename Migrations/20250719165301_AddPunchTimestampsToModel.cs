using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PunchApiProject.Migrations
{
    /// <inheritdoc />
    public partial class AddPunchTimestampsToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord");

            migrationBuilder.DropIndex(
                name: "IX_PunchRecord_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "PunchRecord");

            migrationBuilder.RenameTable(
                name: "PunchRecord",
                newName: "PunchRecords");

            migrationBuilder.RenameColumn(
                name: "PunchOut",
                table: "PunchRecords",
                newName: "PunchOutTime");

            migrationBuilder.RenameColumn(
                name: "PunchIn",
                table: "PunchRecords",
                newName: "PunchInTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords");

            migrationBuilder.RenameTable(
                name: "PunchRecords",
                newName: "PunchRecord");

            migrationBuilder.RenameColumn(
                name: "PunchOutTime",
                table: "PunchRecord",
                newName: "PunchOut");

            migrationBuilder.RenameColumn(
                name: "PunchInTime",
                table: "PunchRecord",
                newName: "PunchIn");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "PunchRecord",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PunchRecord_EmployeeId",
                table: "PunchRecord",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
