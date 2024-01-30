using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EloboostCommerce.Migrations
{
    public partial class stateisboosterolarakdegistirildi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Users",
                newName: "IsBooster");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBooster",
                table: "Users",
                newName: "State");
        }
    }
}
