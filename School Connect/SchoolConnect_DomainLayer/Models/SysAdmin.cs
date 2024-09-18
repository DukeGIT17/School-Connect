using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class SysAdmin : BaseActor
    {
        [Required(ErrorMessage = "Please provide staff number.")]
        [Display(Name = "Staff Number")]
        [Range(10000, 9999999, ErrorMessage = "Staff Number should contain between 5 and 7 digits.")]
        public long StaffNr { get; set; }

        [Required(ErrorMessage = "Please provide email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        //[Required(ErrorMessage = "Please provide phone number.")]
        [Display(Name = "Phone Number")]
        //[Phone]
        public long? PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        #region Navigation Properties
        public School? SysAdminSchoolNP { get; set; }
        #endregion

    }
}
