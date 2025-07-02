using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PunchApiProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PunchRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeName = table.Column<string>(type: "TEXT", nullable: false),
                    PunchInTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PunchOutTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PunchRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PunchRecords");
        }
    }
}
