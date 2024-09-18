using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Password_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "System Admins");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Principals");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Parents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "System Admins",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Principals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Parents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
