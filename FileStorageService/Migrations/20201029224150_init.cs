using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorageService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Filename = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PersonId = table.Column<int>(maxLength: 50, nullable: true),
                    FileExtension = table.Column<string>(maxLength: 20, nullable: false),
                    FileSize = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FullBlobName = table.Column<string>(maxLength: 255, nullable: false),
                    Category = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_IsDeleted",
                table: "Files",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Files_PersonId",
                table: "Files",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
