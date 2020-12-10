using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorageService.Migrations
{
    public partial class DbRenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_PersonId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "AuditLogs");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Files",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FullBlobName",
                table: "AuditLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AuditLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserId",
                table: "Files",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_UserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FullBlobName",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditLogs");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Files",
                type: "int",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_PersonId",
                table: "Files",
                column: "PersonId");
        }
    }
}
