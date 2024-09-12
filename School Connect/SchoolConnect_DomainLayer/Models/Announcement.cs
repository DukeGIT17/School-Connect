using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class Announcement
    {
        [Key]
        [Display(Name = "Announcement ID")]
        public int AnnouncementId { get; set; }

        [Required(ErrorMessage = "Please provide a title for the announcement.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title should have between 1 and 200 characters.")]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please specify at least one recipient group.")]
        public IEnumerable<string> Recipients { get; set; }

        [Required(ErrorMessage = "Please provide content for your announcement.")]
        [StringLength(650, MinimumLength = 1, ErrorMessage = "Announcment limited to between 1 and 650 characters.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Send Email")]
        public bool SendEmail { get; set; }

        [Display(Name = "Send SMS")]
        public bool SendSMS { get; set; }

        [Display(Name = "ViewedRecipients")]
        public IEnumerable<string>? ViewedRecipients { get; set; }

        [Display(Name = "Schedule For Later")]
        public bool ScheduleForLater { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Show At")]
        [DataType(DataType.DateTime)]
        public DateTime TimeToPost { get; set; }

        #region Foreign Key Properties
        [Display(Name = "Creator ID")]
        [ForeignKey(nameof(TeacherAnnouncementNP))]
        public long? TeacherID { get; set; }

        [Display(Name = "Creator ID")]
        [ForeignKey(nameof(PrincipalAnnouncementNP))]
        public long? PrincipalID { get; set; }

        [Display(Name = "School Emis Number")]
        [ForeignKey(nameof(AnnouncementSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public Principal? PrincipalAnnouncementNP { get; set; }
        public Teacher? TeacherAnnouncementNP { get; set; }
        public School AnnouncementSchoolNP { get; set; }
        #endregion
    }
}
