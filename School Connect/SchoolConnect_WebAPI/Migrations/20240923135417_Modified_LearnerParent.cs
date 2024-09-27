using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Modified_LearnerParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LearnerIdNo",
                table: "LearnerParent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ParentIdNo",
                table: "LearnerParent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParent_LearnerIdNo",
                table: "LearnerParent",
                column: "LearnerIdNo");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParent_ParentIdNo",
                table: "LearnerParent",
                column: "ParentIdNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LearnerParent_LearnerIdNo",
                table: "LearnerParent");

            migrationBuilder.DropIndex(
                name: "IX_LearnerParent_ParentIdNo",
                table: "LearnerParent");

            migrationBuilder.DropColumn(
                name: "LearnerIdNo",
                table: "LearnerParent");

            migrationBuilder.DropColumn(
                name: "ParentIdNo",
                table: "LearnerParent");
        }
    }
}
