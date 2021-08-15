using Microsoft.EntityFrameworkCore.Migrations;

namespace bReader.Server.Migrations
{
    public partial class r : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedItems_Feeds_SourceFeedPk",
                table: "FeedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_Groups_GroupId",
                table: "Feeds");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Feeds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedItems_Feeds_SourceFeedPk",
                table: "FeedItems",
                column: "SourceFeedPk",
                principalTable: "Feeds",
                principalColumn: "Pk",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_Groups_GroupId",
                table: "Feeds",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedItems_Feeds_SourceFeedPk",
                table: "FeedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_Groups_GroupId",
                table: "Feeds");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Feeds",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedItems_Feeds_SourceFeedPk",
                table: "FeedItems",
                column: "SourceFeedPk",
                principalTable: "Feeds",
                principalColumn: "Pk",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_Groups_GroupId",
                table: "Feeds",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
