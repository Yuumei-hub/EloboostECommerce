using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EloboostCommerce.Migrations
{
    public partial class gameimgurl2added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl2",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl2",
                table: "Games");
        }
    }
}
