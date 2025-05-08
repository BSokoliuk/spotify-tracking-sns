using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyFollowModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_follows_AspNetUsers_FollowerId",
                table: "follows");

            migrationBuilder.DropForeignKey(
                name: "FK_follows_AspNetUsers_id_followed",
                table: "follows");

            migrationBuilder.DropIndex(
                name: "IX_follows_id_followed",
                table: "follows");

            migrationBuilder.DropColumn(
                name: "id_followed",
                table: "follows");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "follows",
                newName: "follower_id");

            migrationBuilder.RenameColumn(
                name: "id_follower",
                table: "follows",
                newName: "followed_id");

            migrationBuilder.RenameIndex(
                name: "IX_follows_FollowerId",
                table: "follows",
                newName: "IX_follows_follower_id");

            migrationBuilder.CreateIndex(
                name: "IX_follows_followed_id",
                table: "follows",
                column: "followed_id");

            migrationBuilder.AddForeignKey(
                name: "FK_follows_AspNetUsers_followed_id",
                table: "follows",
                column: "followed_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_follows_AspNetUsers_follower_id",
                table: "follows",
                column: "follower_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_follows_AspNetUsers_followed_id",
                table: "follows");

            migrationBuilder.DropForeignKey(
                name: "FK_follows_AspNetUsers_follower_id",
                table: "follows");

            migrationBuilder.DropIndex(
                name: "IX_follows_followed_id",
                table: "follows");

            migrationBuilder.RenameColumn(
                name: "follower_id",
                table: "follows",
                newName: "FollowerId");

            migrationBuilder.RenameColumn(
                name: "followed_id",
                table: "follows",
                newName: "id_follower");

            migrationBuilder.RenameIndex(
                name: "IX_follows_follower_id",
                table: "follows",
                newName: "IX_follows_FollowerId");

            migrationBuilder.AddColumn<string>(
                name: "id_followed",
                table: "follows",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_follows_id_followed",
                table: "follows",
                column: "id_followed");

            migrationBuilder.AddForeignKey(
                name: "FK_follows_AspNetUsers_FollowerId",
                table: "follows",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_follows_AspNetUsers_id_followed",
                table: "follows",
                column: "id_followed",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
