using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Attendance",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "SchoolId",
                table: "Attendance",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_ClassId",
                table: "Attendance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Date",
                table: "Attendance",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_SchoolId",
                table: "Attendance",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Schools_SchoolId",
                table: "Attendance",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_SubGrade_ClassId",
                table: "Attendance",
                column: "ClassId",
                principalTable: "SubGrade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Schools_SchoolId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_SubGrade_ClassId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_ClassId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_Date",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_SchoolId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Attendance");
        }
    }
}
