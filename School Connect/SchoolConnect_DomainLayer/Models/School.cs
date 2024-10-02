using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// School class whose object represents an individual school entity within the system.
    /// </summary>
    public class School
    {
        /// <summary>
        /// A school's primary form of identification within the School Connect database.
        /// </summary>
        [Key]
        public long Id {  get; set; }

        /// <summary>
        /// A school's primary form of identification in the real world, and secondary form of identification within the application.
        /// </summary>
        [Required(ErrorMessage = "School Emis Number is required.")]
        [Display(Name = "EMIS Number")]
        [NumberLength(8, ErrorMessage = "School Emis Number should have exactly 8 digits.")]
        public long EmisNumber { get; set; }

        /// <summary>
        /// The unique identifier of the school's logo within the database.
        /// </summary>
        [Display(Name = "School Logo")]
        [DataType(DataType.Upload)]
        public string? Logo { get; set; }

        /// <summary>
        /// The name of this particular school.
        /// </summary>
        [Required(ErrorMessage = "School name is required.")]
        [Display(Name = "School Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "School name should be between 30 and 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "A school name should consist of only letters. (a-z and A-Z)")]
        public string Name { get; set; }

        /// <summary>
        /// The date on which this school was registered on the application.
        /// </summary>
        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }

        /// <summary>
        /// The type or level of school this is, e.g., High School.
        /// </summary>
        [AllowedValues("Primary", "High", "Combined", ErrorMessage = "Please specify propery school type value. (e.g., Primary, High, Combined)")]
        [Required(ErrorMessage = "School level is required.")]
        public string Type { get; set; }

        #region Foreign Key Properties
        /// <summary>
        /// The foreign key referencing the system admin of this school.
        /// </summary>
        [ForeignKey(nameof(SchoolSysAdminNP))]
        public long? SystemAdminId { get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// A navigation property that references a singular row in the School Address table.
        /// </summary>
        public Address SchoolAddress { get; set; }
        public SysAdmin? SchoolSysAdminNP { get; set; }
        public Principal? SchoolPrincipalNP { get; set; }
        public IList<Learner>? SchoolLearnersNP { get; set; }
        public IList<Teacher>? SchoolTeachersNP { get; set; }
        public IList<Announcement>? SchoolAnnouncementNP { get; set; }
        public IList<Group>? SchoolGroupsNP { get; set; }
        #endregion
    }
}
