using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class TelePhoneNumberAndIndexInclusion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelePhoneNumber",
                table: "Schools",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_TelePhoneNumber",
                table: "Schools",
                column: "TelePhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schools_TelePhoneNumber",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "TelePhoneNumber",
                table: "Schools");
        }
    }
}
