using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomadChat.Infrastrusture.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPictureColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "ChatMessages",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "ChatMessages");
        }
    }
}
