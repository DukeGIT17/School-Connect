﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolConnect_DomainLayer.Data;

#nullable disable

namespace SchoolConnect_WebAPI.Migrations
{
    [DbContext(typeof(SchoolConnectDbContext))]
    [Migration("20240916183417_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Address", b =>
                {
                    b.Property<int>("AddressID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<int>("PostalCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<long>("SchoolID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Suburb")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("AddressID");

                    b.HasIndex("SchoolID")
                        .IsUnique();

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Announcement", b =>
                {
                    b.Property<int>("AnnouncementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(650)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<long?>("PrincipalID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Recipients")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("ScheduleForLater")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SchoolID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("SendEmail")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("SendSMS")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TeacherID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeToPost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("ViewedRecipients")
                        .HasColumnType("TEXT");

                    b.HasKey("AnnouncementId");

                    b.HasIndex("PrincipalID");

                    b.HasIndex("SchoolID");

                    b.HasIndex("TeacherID");

                    b.ToTable("Announcements", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GroupMemberIDs")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<long>("SchoolID")
                        .HasColumnType("INTEGER");

                    b.HasKey("GroupId");

                    b.HasIndex("SchoolID");

                    b.ToTable("Groups", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.GroupActor", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ParentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActorType")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.HasKey("GroupId", "TeacherId", "ParentId");

                    b.HasIndex("ParentId");

                    b.HasIndex("TeacherId", "ParentId");

                    b.ToTable("GroupActor");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Learner", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClassID")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("IdNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<long>("SchoolID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Subjects")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdNo")
                        .IsUnique();

                    b.HasIndex("SchoolID");

                    b.HasIndex("Subjects");

                    b.ToTable("Learners", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.LearnerParent", b =>
                {
                    b.Property<long>("LearnerID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ParentID")
                        .HasColumnType("INTEGER");

                    b.HasKey("LearnerID", "ParentID");

                    b.HasIndex("ParentID");

                    b.ToTable("LearnerParent");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Parent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("IdNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentType")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdNo")
                        .IsUnique();

                    b.ToTable("Parents", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Principal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<long>("SchoolID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("StaffNr")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SchoolID")
                        .IsUnique();

                    b.HasIndex("StaffNr")
                        .IsUnique();

                    b.ToTable("Principals", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.School", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateRegistered")
                        .HasColumnType("TEXT");

                    b.Property<long>("EmisNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Logo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<long?>("SystemAdminId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmisNumber")
                        .IsUnique();

                    b.HasIndex("SystemAdminId")
                        .IsUnique();

                    b.ToTable("Schools", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.SysAdmin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("PhoneNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<long>("StaffNr")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StaffNr")
                        .IsUnique();

                    b.ToTable("System Admins", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Teacher", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClassIDs")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MainClass")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<long>("SchoolID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("StaffNr")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Subjects")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SchoolID");

                    b.HasIndex("StaffNr")
                        .IsUnique();

                    b.ToTable("Teachers", (string)null);
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Address", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.School", "School")
                        .WithOne("SchoolAddress")
                        .HasForeignKey("SchoolConnect_DomainLayer.Models.Address", "SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("School");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Announcement", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.Principal", "PrincipalAnnouncementNP")
                        .WithMany("AnnouncementsNP")
                        .HasForeignKey("PrincipalID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SchoolConnect_DomainLayer.Models.School", "AnnouncementSchoolNP")
                        .WithMany("SchoolAnnouncementNP")
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolConnect_DomainLayer.Models.Teacher", "TeacherAnnouncementNP")
                        .WithMany("AnnouncementsNP")
                        .HasForeignKey("TeacherID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("AnnouncementSchoolNP");

                    b.Navigation("PrincipalAnnouncementNP");

                    b.Navigation("TeacherAnnouncementNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Group", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.School", "GroupSchoolNP")
                        .WithMany("SchoolGroupsNP")
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupSchoolNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.GroupActor", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.Group", "GroupNP")
                        .WithMany("GroupActorNP")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolConnect_DomainLayer.Models.Parent", "ParentNP")
                        .WithMany("GroupsNP")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SchoolConnect_DomainLayer.Models.Teacher", "TeacherNP")
                        .WithMany("GroupsNP")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("GroupNP");

                    b.Navigation("ParentNP");

                    b.Navigation("TeacherNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Learner", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.School", "LearnerSchoolNP")
                        .WithMany("SchoolLearnersNP")
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LearnerSchoolNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.LearnerParent", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.Learner", "Learner")
                        .WithMany("Parents")
                        .HasForeignKey("LearnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolConnect_DomainLayer.Models.Parent", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Learner");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Principal", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.School", "PrincipalSchoolNP")
                        .WithOne("SchoolPrincipalNP")
                        .HasForeignKey("SchoolConnect_DomainLayer.Models.Principal", "SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrincipalSchoolNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.School", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.SysAdmin", "SchoolSysAdminNP")
                        .WithOne("SysAdminSchoolNP")
                        .HasForeignKey("SchoolConnect_DomainLayer.Models.School", "SystemAdminId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("SchoolSysAdminNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Teacher", b =>
                {
                    b.HasOne("SchoolConnect_DomainLayer.Models.School", "TeacherSchoolNP")
                        .WithMany("SchoolTeachersNP")
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TeacherSchoolNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Group", b =>
                {
                    b.Navigation("GroupActorNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Learner", b =>
                {
                    b.Navigation("Parents");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Parent", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("GroupsNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Principal", b =>
                {
                    b.Navigation("AnnouncementsNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.School", b =>
                {
                    b.Navigation("SchoolAddress")
                        .IsRequired();

                    b.Navigation("SchoolAnnouncementNP");

                    b.Navigation("SchoolGroupsNP");

                    b.Navigation("SchoolLearnersNP");

                    b.Navigation("SchoolPrincipalNP");

                    b.Navigation("SchoolTeachersNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.SysAdmin", b =>
                {
                    b.Navigation("SysAdminSchoolNP");
                });

            modelBuilder.Entity("SchoolConnect_DomainLayer.Models.Teacher", b =>
                {
                    b.Navigation("AnnouncementsNP");

                    b.Navigation("GroupsNP");
                });
#pragma warning restore 612, 618
        }
    }
}