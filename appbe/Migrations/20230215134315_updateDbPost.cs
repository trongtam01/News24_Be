using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace appbe.Migrations
{
    public partial class updateDbPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Post",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Post",
                newName: "ProductId");
        }
    }
}
