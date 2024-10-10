using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNo = table.Column<long>(type: "bigint", nullable: false),
                    ParentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "System Admins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffNr = table.Column<long>(type: "bigint", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_System Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmisNumber = table.Column<long>(type: "bigint", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelePhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemAdminId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_System Admins_SystemAdminId",
                        column: x => x.SystemAdminId,
                        principalTable: "System Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PostalCode = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeDesignate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grade_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupMemberIDs = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Principals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffNr = table.Column<long>(type: "bigint", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Principals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Principals_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffNr = table.Column<long>(type: "bigint", nullable: false),
                    Subjects = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    AnnouncementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Recipients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(650)", maxLength: 650, nullable: false),
                    SendEmail = table.Column<bool>(type: "bit", nullable: false),
                    SendSMS = table.Column<bool>(type: "bit", nullable: false),
                    ViewedRecipients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleForLater = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToPost = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeacherID = table.Column<long>(type: "bigint", nullable: true),
                    PrincipalID = table.Column<long>(type: "bigint", nullable: true),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.AnnouncementId);
                    table.ForeignKey(
                        name: "FK_Announcements_Principals_PrincipalID",
                        column: x => x.PrincipalID,
                        principalTable: "Principals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcements_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Announcements_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupActor",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TeacherId = table.Column<long>(type: "bigint", nullable: false),
                    ActorType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupActor", x => new { x.GroupId, x.TeacherId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_GroupActor_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupActor_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupActor_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubGrade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassDesignate = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MainTeacherId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubGrade_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubGrade_Teachers_MainTeacherId",
                        column: x => x.MainTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Learners",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNo = table.Column<long>(type: "bigint", nullable: false),
                    ClassCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Subjects = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolID = table.Column<long>(type: "bigint", nullable: false),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Learners_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Learners_SubGrade_ClassID",
                        column: x => x.ClassID,
                        principalTable: "SubGrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherGrade",
                columns: table => new
                {
                    TeacherID = table.Column<long>(type: "bigint", nullable: false),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    StaffNr = table.Column<long>(type: "bigint", nullable: false),
                    ClassDesignate = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherGrade", x => new { x.TeacherID, x.ClassID });
                    table.ForeignKey(
                        name: "FK_TeacherGrade_SubGrade_ClassID",
                        column: x => x.ClassID,
                        principalTable: "SubGrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherGrade_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearnerParents",
                columns: table => new
                {
                    LearnerID = table.Column<long>(type: "bigint", nullable: false),
                    ParentID = table.Column<long>(type: "bigint", nullable: false),
                    LearnerIdNo = table.Column<long>(type: "bigint", nullable: false),
                    ParentIdNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerParents", x => new { x.LearnerID, x.ParentID });
                    table.ForeignKey(
                        name: "FK_LearnerParents_Learners_LearnerID",
                        column: x => x.LearnerID,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearnerParents_Parents_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_SchoolID",
                table: "Address",
                column: "SchoolID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_PrincipalID",
                table: "Announcements",
                column: "PrincipalID");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_SchoolID",
                table: "Announcements",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherID",
                table: "Announcements",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradeDesignate",
                table: "Grade",
                column: "GradeDesignate");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_SchoolID",
                table: "Grade",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupActor_ParentId",
                table: "GroupActor",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupActor_TeacherId_ParentId",
                table: "GroupActor",
                columns: new[] { "TeacherId", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupMemberIDs",
                table: "Groups",
                column: "GroupMemberIDs");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SchoolID",
                table: "Groups",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParents_LearnerIdNo",
                table: "LearnerParents",
                column: "LearnerIdNo");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParents_ParentID",
                table: "LearnerParents",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerParents_ParentIdNo",
                table: "LearnerParents",
                column: "ParentIdNo");

            migrationBuilder.CreateIndex(
                name: "IX_Learners_ClassID",
                table: "Learners",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Learners_IdNo",
                table: "Learners",
                column: "IdNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Learners_SchoolID",
                table: "Learners",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Learners_Subjects",
                table: "Learners",
                column: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_EmailAddress",
                table: "Parents",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_IdNo",
                table: "Parents",
                column: "IdNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parents_PhoneNumber",
                table: "Parents",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_EmailAddress",
                table: "Principals",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_PhoneNumber",
                table: "Principals",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_SchoolID",
                table: "Principals",
                column: "SchoolID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Principals_StaffNr",
                table: "Principals",
                column: "StaffNr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_EmailAddress",
                table: "Schools",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_EmisNumber",
                table: "Schools",
                column: "EmisNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SystemAdminId",
                table: "Schools",
                column: "SystemAdminId",
                unique: true,
                filter: "[SystemAdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_TelePhoneNumber",
                table: "Schools",
                column: "TelePhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubGrade_ClassDesignate",
                table: "SubGrade",
                column: "ClassDesignate");

            migrationBuilder.CreateIndex(
                name: "IX_SubGrade_GradeId",
                table: "SubGrade",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubGrade_MainTeacherId",
                table: "SubGrade",
                column: "MainTeacherId",
                unique: true,
                filter: "[MainTeacherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_System Admins_EmailAddress",
                table: "System Admins",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_System Admins_PhoneNumber",
                table: "System Admins",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_System Admins_StaffNr",
                table: "System Admins",
                column: "StaffNr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGrade_ClassDesignate",
                table: "TeacherGrade",
                column: "ClassDesignate");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGrade_ClassID",
                table: "TeacherGrade",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherGrade_StaffNr",
                table: "TeacherGrade",
                column: "StaffNr");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_EmailAddress",
                table: "Teachers",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_PhoneNumber",
                table: "Teachers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolID",
                table: "Teachers",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_StaffNr",
                table: "Teachers",
                column: "StaffNr",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "GroupActor");

            migrationBuilder.DropTable(
                name: "LearnerParents");

            migrationBuilder.DropTable(
                name: "TeacherGrade");

            migrationBuilder.DropTable(
                name: "Principals");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Learners");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "SubGrade");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "System Admins");
        }
    }
}
