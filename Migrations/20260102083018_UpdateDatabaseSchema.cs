using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PunchApiProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecords_Employees_EmployeeId",
                table: "PunchRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeActivities",
                table: "EmployeeActivities");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "employees");

            migrationBuilder.RenameTable(
                name: "PunchRecords",
                newName: "punch_records");

            migrationBuilder.RenameTable(
                name: "EmployeeActivities",
                newName: "employee_activities");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "employees",
                newName: "position");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "employees",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "employees",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "employees",
                newName: "department");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "employees",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "employees",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "employees",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "employees",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "employees",
                newName: "join_date");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "employees",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "employees",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "employees",
                newName: "employee_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "employees",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Email",
                table: "employees",
                newName: "IX_employees_email");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_EmployeeId",
                table: "employees",
                newName: "IX_employees_employee_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "punch_records",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "punch_records",
                newName: "employee_id");

            migrationBuilder.RenameColumn(
                name: "ActionType",
                table: "punch_records",
                newName: "action_type");

            migrationBuilder.RenameColumn(
                name: "ActionDateTime",
                table: "punch_records",
                newName: "action_date_time");

            migrationBuilder.RenameIndex(
                name: "IX_PunchRecords_EmployeeId",
                table: "punch_records",
                newName: "IX_punch_records_employee_id");

            migrationBuilder.RenameColumn(
                name: "PunchOutTime",
                table: "employee_activities",
                newName: "punch_out_time");

            migrationBuilder.RenameColumn(
                name: "PunchInTime",
                table: "employee_activities",
                newName: "punch_in_time");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "employee_activities",
                newName: "employee_id");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "employee_activities",
                newName: "activity_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "employees",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "join_date",
                table: "employees",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "employees",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_date_time",
                table: "punch_records",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "punch_out_time",
                table: "employee_activities",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "punch_in_time",
                table: "employee_activities",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employees",
                table: "employees",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_punch_records",
                table: "punch_records",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employee_activities",
                table: "employee_activities",
                column: "activity_id");

            migrationBuilder.AddForeignKey(
                name: "FK_punch_records_employees_employee_id",
                table: "punch_records",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_punch_records_employees_employee_id",
                table: "punch_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employees",
                table: "employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_punch_records",
                table: "punch_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employee_activities",
                table: "employee_activities");

            migrationBuilder.RenameTable(
                name: "employees",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "punch_records",
                newName: "PunchRecords");

            migrationBuilder.RenameTable(
                name: "employee_activities",
                newName: "EmployeeActivities");

            migrationBuilder.RenameColumn(
                name: "position",
                table: "Employees",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Employees",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Employees",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "department",
                table: "Employees",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Employees",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Employees",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Employees",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "join_date",
                table: "Employees",
                newName: "JoinDate");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Employees",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Employees",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "Employees",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Employees",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_employees_email",
                table: "Employees",
                newName: "IX_Employees_Email");

            migrationBuilder.RenameIndex(
                name: "IX_employees_employee_id",
                table: "Employees",
                newName: "IX_Employees_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PunchRecords",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "PunchRecords",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "action_type",
                table: "PunchRecords",
                newName: "ActionType");

            migrationBuilder.RenameColumn(
                name: "action_date_time",
                table: "PunchRecords",
                newName: "ActionDateTime");

            migrationBuilder.RenameIndex(
                name: "IX_punch_records_employee_id",
                table: "PunchRecords",
                newName: "IX_PunchRecords_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "punch_out_time",
                table: "EmployeeActivities",
                newName: "PunchOutTime");

            migrationBuilder.RenameColumn(
                name: "punch_in_time",
                table: "EmployeeActivities",
                newName: "PunchInTime");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "EmployeeActivities",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "activity_id",
                table: "EmployeeActivities",
                newName: "ActivityId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoinDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActionDateTime",
                table: "PunchRecords",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PunchOutTime",
                table: "EmployeeActivities",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PunchInTime",
                table: "EmployeeActivities",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeActivities",
                table: "EmployeeActivities",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PunchRecords_Employees_EmployeeId",
                table: "PunchRecords",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
