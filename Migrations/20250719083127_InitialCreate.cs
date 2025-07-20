using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PunchApiProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords");

            migrationBuilder.RenameTable(
                name: "PunchRecords",
                newName: "PunchRecord");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "PunchRecord",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunchRecord_Employees_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunchRecord",
                table: "PunchRecord");

            migrationBuilder.DropIndex(
                name: "IX_PunchRecord_EmployeeId",
                table: "PunchRecord");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PunchRecord");

            migrationBuilder.RenameTable(
                name: "PunchRecord",
                newName: "PunchRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunchRecords",
                table: "PunchRecords",
                column: "Id");
        }
    }
}
