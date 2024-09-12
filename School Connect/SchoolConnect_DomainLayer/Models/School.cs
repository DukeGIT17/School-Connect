using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SchoolConnect_DomainLayer.Models
{
    public class School
    {
        [Key]
        public long Id {  get; set; }

        [Required(ErrorMessage = "School Emis Number is required.")]
        [Display(Name = "EMIS Number")]
        [NumberLength(8, ErrorMessage = "School Emis Number should have exactly 8 digits.")]
        public long EmisNumber { get; set; }

        [Display(Name = "School Logo")]
        [DataType(DataType.Upload)]
        public string? Logo { get; set; }

        [Required(ErrorMessage = "School name is required.")]
        [Display(Name = "School Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "School name should be between 30 and 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "A school name should consist of only letters. (a-z and A-Z)")]
        public string Name { get; set; }

        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Required(ErrorMessage = "School level is required.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "School level should be between 15 and 3 characters. (e.g., High, Primary)")]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Type { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(SchoolSysAdminNP))]
        public long? SystemAdminId { get; set; }
        #endregion

        #region Navigation Properties
        public Address SchoolAddress { get; set; }
        public SysAdmin? SchoolSysAdminNP { get; set; }
        public Principal? SchoolPrincipalNP { get; set; }
        public ICollection<Learner>? SchoolLearnersNP { get; set; }
        public ICollection<Teacher>? SchoolTeachersNP { get; set; }
        public ICollection<Announcement>? SchoolAnnouncementNP { get; set; }
        public ICollection<Group>? SchoolGroupsNP { get; set; }
        #endregion
    }
}
