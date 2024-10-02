using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchoolConnect_DomainLayer.CustomAttributes;

namespace SchoolConnect_DomainLayer.Models
{
    /// <summary>
    /// A class that represents a single Principal entity.
    /// </summary>
    public class Principal : BaseActor
    {
        public Principal()
        {
            PrincipalSchoolNP = new School
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

        [Required(ErrorMessage = "Please provide email address.")]
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

        #region Navigation Properties
        public School PrincipalSchoolNP { get; set; }
        public IList<Announcement>? AnnouncementsNP { get; set; }
        #endregion
    }
}
