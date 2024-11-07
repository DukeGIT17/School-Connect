using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChatUpdateIdentificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverIdentificate",
                table: "Chats",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderIdentificate",
                table: "Chats",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ReceiverIdentificate",
                table: "Chats",
                column: "ReceiverIdentificate");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SenderIdentificate",
                table: "Chats",
                column: "SenderIdentificate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chats_ReceiverIdentificate",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_SenderIdentificate",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ReceiverIdentificate",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "SenderIdentificate",
                table: "Chats");
        }
    }
}
