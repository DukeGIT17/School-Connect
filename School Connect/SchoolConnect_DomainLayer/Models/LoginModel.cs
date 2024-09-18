using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [MustNotMatch(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MustMatch(nameof(ConfirmPassword))]
        [MustNotMatch(nameof(Password))]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [MustMatch(nameof(NewPassword))]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
