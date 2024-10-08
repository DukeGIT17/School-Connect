using Microsoft.EntityFrameworkCore;
using SchoolConnect_DomainLayer.Models;

namespace SchoolConnect_DomainLayer.Data
{
    public class SchoolConnectDbContext : DbContext
    {
        public SchoolConnectDbContext(DbContextOptions<SchoolConnectDbContext> options) : base(options) { }
        public DbSet<School> Schools { get; set; }
        public DbSet<SysAdmin> SystemAdmins { get; set; }
        public DbSet<Principal> Principals { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<LearnerParent> LearnerParents { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SubGrade> SubGrade { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().ToTable("Schools");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<SysAdmin>().ToTable("System Admins");
            modelBuilder.Entity<Principal>().ToTable("Principals");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<Parent>().ToTable("Parents");
            modelBuilder.Entity<Learner>().ToTable("Learners");
            modelBuilder.Entity<Announcement>().ToTable("Announcements");
            modelBuilder.Entity<Group>().ToTable("Groups");

            modelBuilder.Entity<School>()
                .HasIndex(school => school.EmisNumber)
                .IsUnique();

            modelBuilder.Entity<School>()
                .HasIndex(school => school.TelePhoneNumber)
                .IsUnique();

            modelBuilder.Entity<School>()
                .HasOne(school => school.SchoolAddress)
                .WithOne(address => address.School)
                .HasForeignKey<Address>(address => address.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<School>()
                .HasOne(systemAdmin => systemAdmin.SchoolSysAdminNP)
                .WithOne(school => school.SysAdminSchoolNP)
                .HasForeignKey<School>(school => school.SystemAdminId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<SysAdmin>()
                .HasIndex(admin => admin.StaffNr)
                .IsUnique();
            
            modelBuilder.Entity<SysAdmin>()
                .HasIndex(admin => admin.EmailAddress)
                .IsUnique();
            
            modelBuilder.Entity<SysAdmin>()
                .HasIndex(admin => admin.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<School>()
                .HasOne(principal => principal.SchoolPrincipalNP)
                .WithOne(school => school.PrincipalSchoolNP)
                .HasForeignKey<Principal>(principal => principal.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<School>()
                .HasMany(grades => grades.SchoolGradeNP)
                .WithOne(school => school.GradeSchoolNP)
                .HasForeignKey(grade => grade.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Principal>()
                .HasIndex(principal => principal.StaffNr)
                .IsUnique();
            
            modelBuilder.Entity<Principal>()
                .HasIndex(principal => principal.EmailAddress)
                .IsUnique();
            
            modelBuilder.Entity<Principal>()
                .HasIndex(principal => principal.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<School>()
                .HasMany(teachers => teachers.SchoolTeachersNP)
                .WithOne(school => school.TeacherSchoolNP)
                .HasForeignKey(teachers => teachers.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Teacher>()
                .HasIndex(teacher => teacher.StaffNr)
                .IsUnique();
            
            modelBuilder.Entity<Teacher>()
                .HasIndex(teacher => teacher.EmailAddress)
                .IsUnique();
            
            modelBuilder.Entity<Teacher>()
                .HasIndex(teacher => teacher.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasOne(grade => grade.MainClass)
                .WithOne(teacher => teacher.MainTeacher)
                .HasForeignKey<SubGrade>(grade => grade.MainTeacherId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<TeacherGrade>()
                .HasKey(tg => new { tg.TeacherID, tg.ClassID });

            modelBuilder.Entity<TeacherGrade>()
                .HasIndex(tg => tg.StaffNr);
            
            modelBuilder.Entity<TeacherGrade>()
                .HasIndex(tg => tg.ClassDesignate);

            modelBuilder.Entity<TeacherGrade>()
                .HasOne(teacher => teacher.Teacher)
                .WithMany(grades => grades.Classes)
                .HasForeignKey(tg => tg.TeacherID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherGrade>()
                .HasOne(grade => grade.Class)
                .WithMany(teachers => teachers.Teachers)
                .HasForeignKey(tg => tg.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<School>()
                .HasMany(learners => learners.SchoolLearnersNP)
                .WithOne(school => school.LearnerSchoolNP)
                .HasForeignKey(learners => learners.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<School>()
                .HasMany(announcements => announcements.SchoolAnnouncementNP)
                .WithOne(school => school.AnnouncementSchoolNP)
                .HasForeignKey(announcement => announcement.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<School>()
                .HasMany(groups => groups.SchoolGroupsNP)
                .WithOne(school => school.GroupSchoolNP)
                .HasForeignKey(groups => groups.SchoolID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Principal>()
                .HasMany(announcements => announcements.AnnouncementsNP)
                .WithOne(principal => principal.PrincipalAnnouncementNP)
                .HasForeignKey(announcements => announcements.PrincipalID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Group>()
                .HasIndex(g => g.GroupMemberIDs)
                .IsUnique(false);

            modelBuilder.Entity<GroupActor>()
                .HasKey(ga => new { ga.GroupId, ga.TeacherId, ga.ParentId });

            modelBuilder.Entity<GroupActor>()
                .HasIndex(ga => new { ga.TeacherId, ga.ParentId }).IsUnique(false);

            modelBuilder.Entity<GroupActor>()
                .HasOne(ga => ga.GroupNP)
                .WithMany(groups => groups.GroupActorNP)
                .HasForeignKey(ga => ga.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupActor>()
                .HasOne(ga => ga.TeacherNP)
                .WithMany(teachers => teachers.GroupsNP)
                .HasForeignKey(ga => ga.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupActor>()
                .HasOne(ga => ga.ParentNP)
                .WithMany(parents => parents.GroupsNP)
                .HasForeignKey(ga => ga.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>()
                .HasMany(announcements => announcements.AnnouncementsNP)
                .WithOne(teacher => teacher.TeacherAnnouncementNP)
                .HasForeignKey(announcements => announcements.TeacherID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Parent>()
                .HasIndex(parent => parent.IdNo)
                .IsUnique();
            
            modelBuilder.Entity<Parent>()
                .HasIndex(parent => parent.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<Parent>()
                .HasIndex(parent => parent.PhoneNumber)
                .IsUnique();
            
            modelBuilder.Entity<Learner>()
                .HasIndex(learner => learner.IdNo)
                .IsUnique();

            modelBuilder.Entity<LearnerParent>()
                .HasKey(lp => new { lp.LearnerID, lp.ParentID });

            modelBuilder.Entity<LearnerParent>()
                .HasIndex(lp => lp.LearnerIdNo)
                .IsUnique(false);

            modelBuilder.Entity<LearnerParent>()
                .HasIndex(lp => lp.ParentIdNo)
                .IsUnique(false);

            modelBuilder.Entity<LearnerParent>()
                .HasOne(learner => learner.Learner)
                .WithMany(parents => parents.Parents)
                .HasForeignKey(lp => lp.LearnerID)
                .IsRequired();

            modelBuilder.Entity<LearnerParent>()
                .HasOne(parent => parent.Parent)
                .WithMany(learners => learners.Children)
                .HasForeignKey(lp => lp.ParentID)
                .IsRequired();

            modelBuilder.Entity<Learner>()
                .HasIndex(learner => learner.Subjects)
                .IsUnique(false);

            modelBuilder.Entity<Learner>()
                .HasOne(grade => grade.Class)
                .WithMany(learners => learners.Learners)
                .HasForeignKey(learner => learner.ClassID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasIndex(grade => grade.GradeDesignate)
                .IsUnique(false);
            
            modelBuilder.Entity<SubGrade>()
                .HasIndex(subGrade => subGrade.ClassDesignate)
                .IsUnique(false);

            modelBuilder.Entity<Grade>()
                .HasMany(subGrades => subGrades.Classes)
                .WithOne(grade => grade.Grade)
                .HasForeignKey(subGrade => subGrade.GradeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
