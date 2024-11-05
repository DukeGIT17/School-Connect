using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChatUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Schools_SchoolId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_SubGrade_ClassId",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "SchoolId",
                table: "Attendance",
                newName: "SchoolID");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Attendance",
                newName: "ClassID");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_SchoolId",
                table: "Attendance",
                newName: "IX_Attendance_SchoolID");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_ClassId",
                table: "Attendance",
                newName: "IX_Attendance_ClassID");

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    TimeSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SenderId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecieverId = table.Column<long>(type: "INTEGER", nullable: false),
                    TeacherId = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<long>(type: "INTEGER", nullable: false),
                    SchoolID = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ParentId",
                table: "Chats",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_RecieverId",
                table: "Chats",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SchoolID",
                table: "Chats",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_SenderId",
                table: "Chats",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_TeacherId",
                table: "Chats",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_TimeSent",
                table: "Chats",
                column: "TimeSent");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Schools_SchoolID",
                table: "Attendance",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_SubGrade_ClassID",
                table: "Attendance",
                column: "ClassID",
                principalTable: "SubGrade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Schools_SchoolID",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_SubGrade_ClassID",
                table: "Attendance");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.RenameColumn(
                name: "SchoolID",
                table: "Attendance",
                newName: "SchoolId");

            migrationBuilder.RenameColumn(
                name: "ClassID",
                table: "Attendance",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_SchoolID",
                table: "Attendance",
                newName: "IX_Attendance_SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_ClassID",
                table: "Attendance",
                newName: "IX_Attendance_ClassId");

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
    }
}
