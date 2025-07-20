using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PunchApiProject.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.CreateIndex(
                name: "IX_PunchRecords_EmployeeId",
                table: "PunchRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginRecords_EmployeeId",
                table: "LoginRecords",
                column: "EmployeeId");

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

            migrationBuilder.DropTable(
                name: "LoginRecords");

            migrationBuilder.DropIndex(
                name: "IX_PunchRecords_EmployeeId",
                table: "PunchRecords");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Employees");
        }
    }
}
