using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// A class that represents a single Teacher entity.
    /// </summary>
    public class Teacher : BaseActor
    {
        [Required(ErrorMessage = "Please provide staff number.")]
        [Display(Name = "Staff Number")]
        [StringLength(7, MinimumLength = 5, ErrorMessage = "Staff Number should contain between 5 and 7 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please specify a proper Emis Number. Only numerical values allowed.")]
        public string StaffNr { get; set; }

        public IList<string> Subjects { get; set; }

        [Required(ErrorMessage = "Please provide phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(TeacherSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public SubGrade? MainClass { get; set; }
        public IList<TeacherGrade>? Classes { get; set; }
        public School? TeacherSchoolNP { get; set; }
        public IList<GroupActor>? GroupsNP { get; set; }
        public IList<Announcement>? AnnouncementsNP { get; set; }
        #endregion
    }
}
