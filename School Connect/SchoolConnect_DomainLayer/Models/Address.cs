using SchoolConnect_DomainLayer.CustomAttributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolConnect_DomainLayer.Models
{
    public class Address
    {
        [Key]
        [Display(Name = "Address ID")]
        public int AddressID { get; set; }

        [Required(ErrorMessage = "Please provide street name.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Street name should be between 30 and 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Please provide a proper street name. No special characters or symbols allowed.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please provide suburb name.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Suburb name should be between 30 and 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Please provide a proper suburb name. No special characters, symbols, or numerical values allowed.")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = "Please provide city of residence.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "City name should be between 30 and 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Please provide a proper city name. No special characters, symbols, or numerical values allowed.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please provide postal code.")]
        [NumberLength(4, ErrorMessage = "Postal code should have exactly 4 digits.")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "Please provide province of residence.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Province name should be between 30 and 3 characters long.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Please provide a proper province name. No special characters, symbols, or numerical values allowed.")]
        public string Province { get; set; }

        #region Foreign Key Properties
        [ForeignKey(nameof(School))]
        public long SchoolID { get; set; }
        #endregion

        #region Navigation Property
        public School? School { get; set; }
        #endregion
    }
}
