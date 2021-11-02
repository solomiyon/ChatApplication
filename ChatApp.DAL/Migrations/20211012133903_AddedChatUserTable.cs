using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.DAL.Migrations
{
    public partial class AddedChatUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_AspNetUsers_UsersId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_ChatsId",
                table: "ChatUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.DropIndex(
                name: "IX_ChatUser_UsersId",
                table: "ChatUser");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "ChatUser");

            migrationBuilder.RenameColumn(
                name: "ChatsId",
                table: "ChatUser",
                newName: "ChatId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChatUser",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_ChatId",
                table: "ChatUser",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UserId",
                table: "ChatUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_AspNetUsers_UserId",
                table: "ChatUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_ChatId",
                table: "ChatUser",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_AspNetUsers_UserId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_ChatId",
                table: "ChatUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.DropIndex(
                name: "IX_ChatUser_ChatId",
                table: "ChatUser");

            migrationBuilder.DropIndex(
                name: "IX_ChatUser_UserId",
                table: "ChatUser");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChatUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatUser");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "ChatUser",
                newName: "ChatsId");

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "ChatUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "ChatsId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UsersId",
                table: "ChatUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_AspNetUsers_UsersId",
                table: "ChatUser",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_ChatsId",
                table: "ChatUser",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
