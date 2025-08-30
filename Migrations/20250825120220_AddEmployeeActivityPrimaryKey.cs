using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PunchApiProject.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeActivityPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecords_Employees_EmployeeId",
                table: "PunchRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "PunchRecords",
                newName: "PunchRecord");

            migrationBuilder.RenameIndex(
                name: "IX_PunchRecords_EmployeeId",
                table: "PunchRecord",
                newName: "IX_PunchRecord_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmployeeActivities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    PunchInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PunchOutTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeActivities", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "Punches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    PunchIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PunchOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropTable(
                name: "EmployeeActivities");

            migrationBuilder.DropTable(
                name: "Punches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord");

            migrationBuilder.RenameTable(
                name: "PunchRecord",
                newName: "PunchRecords");

            migrationBuilder.RenameIndex(
                name: "IX_PunchRecord_EmployeeId",
                table: "PunchRecords",
                newName: "IX_PunchRecords_EmployeeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
