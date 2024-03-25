using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomadChat.Infrastrusture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUnreadProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Unread",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unread",
                table: "ChatMessages");
        }
    }
}
