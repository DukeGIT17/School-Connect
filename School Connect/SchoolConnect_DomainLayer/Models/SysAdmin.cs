using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// A class that represents a single System Admin entity.
    /// </summary>
    public class SysAdmin : BaseActor
    {
        [Required(ErrorMessage = "Please provide staff number.")]
        [Display(Name = "Staff Number")]
        [StringLength(7, MinimumLength = 5, ErrorMessage = "Staff Number should contain between 5 and 7 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please specify a proper Emis Number. Only numerical values allowed.")]
        public string StaffNr { get; set; }

        [Required(ErrorMessage = "Please provide email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please provide phone number.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        #region Navigation Properties
        public School? SysAdminSchoolNP { get; set; }
        #endregion

    }
}
