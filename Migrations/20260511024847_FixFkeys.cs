using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portafolio.Migrations
{
    /// <inheritdoc />
    public partial class FixFkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthProviders_AppUsers_AppUserId",
                table: "AuthProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_InviteTokens_AppUsers_AppUserId",
                table: "InviteTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AppUsers_AppUserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_AppUserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_InviteTokens_AppUserId",
                table: "InviteTokens");

            migrationBuilder.DropIndex(
                name: "IX_AuthProviders_AppUserId",
                table: "AuthProviders");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "InviteTokens");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AuthProviders");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteTokens_UserId",
                table: "InviteTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthProviders_UserId",
                table: "AuthProviders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthProviders_AppUsers_UserId",
                table: "AuthProviders",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InviteTokens_AppUsers_UserId",
                table: "InviteTokens",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AppUsers_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthProviders_AppUsers_UserId",
                table: "AuthProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_InviteTokens_AppUsers_UserId",
                table: "InviteTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AppUsers_UserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_InviteTokens_UserId",
                table: "InviteTokens");

            migrationBuilder.DropIndex(
                name: "IX_AuthProviders_UserId",
                table: "AuthProviders");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "InviteTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "AuthProviders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AppUserId",
                table: "Jobs",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteTokens_AppUserId",
                table: "InviteTokens",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthProviders_AppUserId",
                table: "AuthProviders",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthProviders_AppUsers_AppUserId",
                table: "AuthProviders",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InviteTokens_AppUsers_AppUserId",
                table: "InviteTokens",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AppUsers_AppUserId",
                table: "Jobs",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
