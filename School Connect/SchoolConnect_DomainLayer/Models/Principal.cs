using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// A class that represents a single Principal entity.
    /// </summary>
    public class Principal : BaseActor
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

        #region Foreign Key Properties
        [ForeignKey(nameof(PrincipalSchoolNP))]
        public long SchoolID { get; set; }
        #endregion

#nullable enable
        #region Navigation Properties
        public School? PrincipalSchoolNP { get; set; }
        public IList<Announcement>? AnnouncementsNP { get; set; }
        #endregion
    }
}
