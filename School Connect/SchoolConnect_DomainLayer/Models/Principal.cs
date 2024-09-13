using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    public class Principal : BaseActor
    {
        [Required(ErrorMessage = "Please provide staff number.")]
        [Display(Name = "Staff Number")]
        [Range(10000, 9999999, ErrorMessage = "Staff Number should contain between 5 and 7 digits.")]
        public long StaffNr { get; set; }

        [Required(ErrorMessage = "Please provide email address.")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please provide phone number.")]
        [Display(Name = "Phone Number")]
        [NumberLength(10, ErrorMessage = "Phone number should contain exactly 10 digits")]
        [DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(PrincipalSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Properties
        public School PrincipalSchoolNP { get; set; }
        public ICollection<Announcement>? AnnouncementsNP { get; set; }
        #endregion
    }
}
