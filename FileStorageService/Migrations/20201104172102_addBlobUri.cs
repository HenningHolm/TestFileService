using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorageService.Migrations
{
    public partial class addBlobUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobUri",
                table: "Files",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobUri",
                table: "Files");
        }
    }
}
