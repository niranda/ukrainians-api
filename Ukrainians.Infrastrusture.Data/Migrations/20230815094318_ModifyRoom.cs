using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomadChat.Infrastrusture.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "ChatRooms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "ChatRooms");
        }
    }
}
