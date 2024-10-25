using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScheduleForLater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleForLater",
                table: "Announcements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ScheduleForLater",
                table: "Announcements",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
