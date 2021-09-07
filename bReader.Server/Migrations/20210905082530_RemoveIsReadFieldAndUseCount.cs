using Microsoft.EntityFrameworkCore.Migrations;

namespace bReader.Server.Migrations
{
    public partial class RemoveIsReadFieldAndUseCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "Feeds",
                newName: "UnreadCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnreadCount",
                table: "Feeds",
                newName: "IsRead");
        }
    }
}
