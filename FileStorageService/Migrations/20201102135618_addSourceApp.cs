using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorageService.Migrations
{
    public partial class addSourceApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceApplication",
                table: "Files",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceApplication",
                table: "Files");
        }
    }
}
