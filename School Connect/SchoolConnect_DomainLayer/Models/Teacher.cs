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
        public Teacher()
        {
            TeacherSchoolNP = new School
            {
                EmisNumber = 15985654,
                Logo = "Default Pic",
                Name = "Dummy School",
                DateRegistered = DateTime.Now,
                Type = "High",
                SystemAdminId = 1,
                SchoolAddress = new Address
                {
                    Street = "1234",
                    Suburb = "Dummy Address",
                    City = "Dummy City",
                    PostalCode = 1234,
                    Province = "Dummy Province"
                },
            };
        }

        [Required(ErrorMessage = "Please provide staff number.")]
        [Display(Name = "Staff Number")]
        [Range(10000, 9999999, ErrorMessage = "Staff Number should contain between 5 and 7 digits.")]
        public long StaffNr { get; set; }

        [Display(Name = "Main Class")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only alpha-numeric values allowed.")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Main class should between 1 and 5 characters long.")]
        public string? MainClass { get; set; }

        public IList<string> Subjects { get; set; }

        public IList<string> ClassIDs { get; set; }

        [Required(ErrorMessage = "Please provide phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(TeacherSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public School TeacherSchoolNP { get; set; }
        public ICollection<Announcement>? AnnouncementsNP { get; set; }
        public ICollection<GroupActor>? GroupsNP { get; set; }
        #endregion
    }
}
