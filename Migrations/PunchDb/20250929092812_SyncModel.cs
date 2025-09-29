using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PunchApiProject.Migrations.PunchDb
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropTable(
                name: "LoginRecords");

            migrationBuilder.DropTable(
                name: "Punches");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Username",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PunchOutTime",
                table: "PunchRecord");

            migrationBuilder.RenameTable(
                name: "PunchRecord",
                newName: "PunchRecords");

            migrationBuilder.RenameColumn(
                name: "PunchInTime",
                table: "PunchRecords",
                newName: "ActionDateTime");

            migrationBuilder.RenameIndex(
                name: "IX_PunchRecord_EmployeeId",
                table: "PunchRecords",
                newName: "IX_PunchRecords_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActionType",
                table: "PunchRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PunchRecords_Employees_EmployeeId",
                table: "PunchRecords",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecords_Employees_EmployeeId",
                table: "PunchRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "PunchRecords");

            migrationBuilder.RenameTable(
                name: "PunchRecords",
                newName: "PunchRecord");

            migrationBuilder.RenameColumn(
                name: "ActionDateTime",
                table: "PunchRecord",
                newName: "PunchInTime");

            migrationBuilder.RenameIndex(
                name: "IX_PunchRecords_EmployeeId",
                table: "PunchRecord",
                newName: "IX_PunchRecord_EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PunchOutTime",
                table: "PunchRecord",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LoginRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Punches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ActionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Punches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Punches_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Username",
                table: "Employees",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoginRecords_EmployeeId",
                table: "LoginRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Punches_EmployeeId",
                table: "Punches",
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
