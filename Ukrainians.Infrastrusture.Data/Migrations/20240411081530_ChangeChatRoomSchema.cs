using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomadChat.Infrastrusture.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeChatRoomSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatRoomId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatRoomId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ChatRoomUser",
                columns: table => new
                {
                    ChatRoomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomUser", x => new { x.ChatRoomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatRoomUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomUser_ChatRooms_ChatRoomsId",
                        column: x => x.ChatRoomsId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomUser_UsersId",
                table: "ChatRoomUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomUser");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatRoomId",
                table: "AspNetUsers",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ChatRooms_ChatRoomId",
                table: "AspNetUsers",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }
    }
}
