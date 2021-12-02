using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApp.DAL.Migrations
{
    public partial class AddChatAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Chats");
        }
    }
}
